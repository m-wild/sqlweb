using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class Objects
    {
        [JsonPropertyName("table")]
        public List<string> Tables { get; } = new List<string>();

        [JsonPropertyName("view")]
        public List<string> Views { get; } = new List<string>();

        [JsonPropertyName("procedure")]
        public List<string> Procedures { get; } = new List<string>();

        [JsonPropertyName("function")]
        public List<string> Functions { get; } = new List<string>();
        
        [JsonPropertyName("materialized_view")]
        public List<string> MaterializedViews { get; } = new List<string>();

        [JsonPropertyName("sequence")]
        public List<string> Sequences { get; } = new List<string>();
    }
}