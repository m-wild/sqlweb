using Newtonsoft.Json;

namespace SqlWeb.Types
{
    public class TableInfo
    {
        [JsonProperty("data_size")]
        public string DataSize { get; set; }

        [JsonProperty("index_size")]
        public string IndexSize { get; set; }

        [JsonProperty("rows_count")]
        public long Rows { get; set; }

        [JsonProperty("total_size")]
        public string TotalSize { get; set; }
    }
}