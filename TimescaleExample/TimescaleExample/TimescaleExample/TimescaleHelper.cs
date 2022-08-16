using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace TimescaleExample
{
    public class TimescaleHelper
    {
        private static string Host = "";
        private static string User = "";
        private static string DBname = "";
        private static string Password = "";
        private static string Port = "";
        private static string conn_str = "";

        //
        // This is the constructor for our TimescaleHelper class
        //
        public TimescaleHelper(string host = "<HOSTNAME>", string user = "<USERNAME>",
            string dbname = "<DATABASE_NAME>", string password = "<PASSWORD>", string port = "<PORT>")
        {
            Host = host;
            User = user;
            DBname = dbname;
            Password = password;
            Port = port;
            // Build connection string using the parameters above
            conn_str = String.Format("Server={0};Username={1};Database={2};Port={3};Password={4};SSLMode=Prefer",
                Host,
                User,
                DBname,
                Port,
                Password);
        }

        // Helper method to get a connection for the execute function
        NpgsqlConnection getConnection()
        {
            var Connection = new NpgsqlConnection(conn_str);
            Connection.Open();
            return Connection;
        }

        //
        // Procedure - Connecting .NET to TimescaleDB:
        // Check the connection TimescaleDB and verify that the extension
        // is installed in this database
        //
        public void CheckDatabaseConnection()
        {
            // get one connection for all SQL commands below
            using (var conn = getConnection())
            {

                var sql = "SELECT default_version, comment FROM pg_available_extensions WHERE name = 'timescaledb';";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using NpgsqlDataReader rdr = cmd.ExecuteReader();

                    if (!rdr.HasRows)
                    {
                        Console.WriteLine("Missing TimescaleDB extension!");
                        conn.Close();
                        return;
                    }

                    while (rdr.Read())
                    {
                        Console.WriteLine("TimescaleDB Default Version: {0}\n{1}", rdr.GetString(0), rdr.GetString(1));
                    }

                    conn.Close();
                }
            }

        }

        //
        // Procedure - Creating a relational table:
        // Create a table for basic relational data and
        // populate it with a few fake sensors
        //
        public void CreateRelationalData()
        {
            //use one connection to use for all three commands below.
            using (var conn = getConnection())
            {
                using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS tagvalues cascade", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished dropping table (if existed)");
                }

                using (var command = new NpgsqlCommand("CREATE TABLE tagvalues (id SERIAL PRIMARY KEY, tag TEXT, val TEXT, ts TEXT, quality TEXT);", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Finished creating the sensors table");
                }

                // Create the list of sensors as key/value pairs to insert next
                var sensors = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("a","floor"),
                    new KeyValuePair<string, string>("a","ceiling"),
                    new KeyValuePair<string, string>("b","floor"),
                    new KeyValuePair<string, string>("b","ceiling")
                };

                // Iterate over the list to insert it into the newly
                // created relational table using parameter substitution
                foreach (KeyValuePair<string, string> kvp in sensors)
                {
                    using (var command = new NpgsqlCommand("INSERT INTO sensors (type, location) VALUES (@type, @location)", conn))
                    {
                        command.Parameters.AddWithValue("type", kvp.Key);
                        command.Parameters.AddWithValue("location", kvp.Value);

                        int nRows = command.ExecuteNonQuery();
                        Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                    }
                }
            }
        }

        //
        // Procedure - Creating a hypertable:
        // Create a new table to store time-series data and create
        // a new TimescaleDB hypertable using the new table. It is
        // partitioned on the 'time' column
        public void CreateHypertable()
        {
            //use one connection to use for all three commands below.
            using (var conn = getConnection())
            {
                using (var command = new NpgsqlCommand("DROP TABLE IF EXISTS tag_data CASCADE;", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Dropped tag_data table if it existed");
                }

                using (var command = new NpgsqlCommand(@"CREATE TABLE tag_data (
                                           time TIMESTAMPTZ NOT NULL,
                                           id INTEGER,
                                           tag_name TEXT,
                                           val DOUBLE PRECISION,
                                           quality TEXT
                                           
                                           );", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Created sensor_data table to store time-series data");
                }

                using (var command = new NpgsqlCommand("SELECT create_hypertable('tag_data', 'time');", conn))
                {
                    command.ExecuteNonQuery();
                    Console.Out.WriteLine("Converted the sensor_data table into a TimescaleDB hypertable!");
                }
            }
        }

        //
        // Procedure - Insert time-series data:
        // With the hypertable in place, insert data using the PostgreSQL
        // supplied 'generate_series()' function, iterating over our small list
        // of sensors from Step 2.
        public void InsertData()
        {
            using (var conn = getConnection())
            {
                // This query creates one row of data every minute for each
                // sensor_id, for the last 24 hours ~= 1440 readings per sensor
                var sql = @"INSERT INTO tag_data
                            SELECT generate_series(now() - interval '24 hour',
                                                    now(),
                                                    interval '1 minute') AS time,
                            @sid as id,
                            'c1.H_1AH_BMM_1\\MMXU1\\A\\phsA' as tag_name,
                            random()*100 AS val,
                            'Good' AS quality";

                // We created four sensors in Step 2 and so we iterate over their
                // auto generated IDs to insert data. This could be modified
                // using a larger list or updating the SQL to JOIN on the 'sensors'
                // table to get the IDs for data creation.

                using (var command = new NpgsqlCommand(sql, conn))
                {
                    for (int i = 1; i <= 220000*24; i++)
                    {
                        command.Parameters.AddWithValue("sid", i);

                        int nRows = command.ExecuteNonQuery();
                        Console.Out.WriteLine(String.Format("Number of rows inserted={0}", nRows));
                    }
                }
            }
        }
    }
}
