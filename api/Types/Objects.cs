using System.Collections.Generic;
using Newtonsoft.Json;

namespace SqlWeb.Types
{
    public class Objects
    {
        [JsonProperty("table")]
        public List<string> Tables { get; } = new List<string>();

        [JsonProperty("view")]
        public List<string> Views { get; } = new List<string>();

        [JsonProperty("procedure")]
        public List<string> Procedures { get; } = new List<string>();

        [JsonProperty("function")]
        public List<string> Functions { get; } = new List<string>();
        
        [JsonProperty("materialized_view")]
        public List<string> MaterializedViews { get; } = new List<string>();

        [JsonProperty("sequence")]
        public List<string> Sequences { get; } = new List<string>();
    }
}