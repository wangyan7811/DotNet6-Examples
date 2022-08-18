using SqlSugar;

namespace TimescaleDBExample.models
{
    [SugarTable("tag")]
    public class Tag
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string TagName { get; set; }

    }
}
