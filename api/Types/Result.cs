using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class Result
    {
        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; }

        [JsonPropertyName("columns")]
        public List<string> Columns { get; set; } = new List<string>();

        [JsonPropertyName("rows")]
        public List<object[]> Rows { get; set; } = new List<object[]>();

        public void SetRowsAffected(int rows)
        {
            Columns.Clear();
            Columns.Add("Rows Affected");
            
            Rows.Clear();
            Rows.Add(new object[]{ rows });
        }
    }
}