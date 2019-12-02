using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class QueryRequest
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }
    }
}