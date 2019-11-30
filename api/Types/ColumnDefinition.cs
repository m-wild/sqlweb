using Newtonsoft.Json;

namespace SqlWeb.Types
{
    public class ColumnDefinition
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("data_type")]
        public string DataType { get; set; }
        
        [JsonProperty("max_length")]
        public int MaxLength { get; set; }
        
        [JsonProperty("is_nullable")]
        public bool IsNullable { get; set; }
        
        [JsonProperty("precision")]
        public int Precision { get; set; }
        
        [JsonProperty("scale")]
        public int Scale { get; set; }
        
        [JsonProperty("is_identity")]
        public bool IsIdentity { get; set; }
        
        [JsonProperty("column_default")]
        public string ColumnDefault { get; set; }
        
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}