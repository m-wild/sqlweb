using Newtonsoft.Json;

namespace SqlWeb.Types
{
    public class Pagination
    {
        [JsonProperty("rows_count")]
        public long Rows { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("pages_count")]
        public long Pages { get; set; }

        [JsonProperty("per_page")]
        public long PerPage { get; set; }
    }
}