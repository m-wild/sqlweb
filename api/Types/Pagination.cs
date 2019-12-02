using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class Pagination
    {
        [JsonPropertyName("rows_count")]
        public long Rows { get; set; }

        [JsonPropertyName("page")]
        public long Page { get; set; }

        [JsonPropertyName("pages_count")]
        public long Pages { get; set; }

        [JsonPropertyName("per_page")]
        public long PerPage { get; set; }
    }
}