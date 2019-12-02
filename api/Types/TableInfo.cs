using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class TableInfo
    {
        [JsonPropertyName("data_size")]
        public string DataSize { get; set; }

        [JsonPropertyName("index_size")]
        public string IndexSize { get; set; }

        [JsonPropertyName("rows_count")]
        public long Rows { get; set; }

        [JsonPropertyName("total_size")]
        public string TotalSize { get; set; }
    }
}