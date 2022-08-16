// See https://aka.ms/new-console-template for more information

using TimescaleExample;

Console.WriteLine("Hello, World!");
TimescaleHelper ts = new TimescaleHelper("127.0.0.1","postgres","postgres","000000","5432");

// Procedure - Connecting .NET to TimescaleDB:
// Verify that the program can connect
// to the database and that TimescaleDB is installed!
ts.CheckDatabaseConnection();

ts.CreateHypertable();

ts.InsertData();