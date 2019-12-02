using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class ConnectRequest
    {
        [JsonPropertyName("engine")]
        public string Engine { get; set; }

        [JsonPropertyName("connection_string")]
        public string ConnectionString { get; set; }
    }
}