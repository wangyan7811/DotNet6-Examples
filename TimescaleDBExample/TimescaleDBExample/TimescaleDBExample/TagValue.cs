using SqlSugar;

namespace TimescaleDBExample
{
    [SugarTable("tag_history_value")]
    public class TagHistoryValue
    {
        public string TagName { get; set; }

        public double? Val { get; set; }
        [SugarColumn(ColumnName = "time")]
        public DateTime Ts { get; set; }

        public TagValueQuality Quality { get; set; }


    }
}
