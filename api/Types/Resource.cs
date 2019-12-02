using System.Security;
using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class Resource
    {
        [JsonPropertyName("resource_id")]
        public string ResourceId { get; set; }
        
        [JsonPropertyName("engine")]
        public string Engine { get; set; }

        [JsonIgnore]
        public string ConnectionString { get; set; }
    }
}