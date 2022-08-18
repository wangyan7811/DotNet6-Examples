using SqlSugar;

namespace TimescaleDBExample.models
{
    [SugarTable("tag_history_value")]
    public class TagHistoryValue122
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int TagId { get; set; }

        public double? Val { get; set; }
        [SugarColumn(ColumnName = "time")]
        public DateTime Ts { get; set; }

        public TagValueQuality Quality { get; set; }

    }
}
