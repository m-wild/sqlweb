using Newtonsoft.Json;

namespace SqlWeb.Types
{
    public class QueryRequest
    {
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}