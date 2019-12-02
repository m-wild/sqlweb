using System.Collections.Generic;
using Newtonsoft.Json;

namespace SqlWeb.Types
{
    public class Result
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; } = new List<string>();

        [JsonProperty("rows")]
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