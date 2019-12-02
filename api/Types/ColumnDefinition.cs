using System.Text.Json.Serialization;

namespace SqlWeb.Types
{
    public class ColumnDefinition
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("data_type")]
        public string DataType { get; set; }
        
        [JsonPropertyName("max_length")]
        public int MaxLength { get; set; }
        
        [JsonPropertyName("is_nullable")]
        public bool IsNullable { get; set; }
        
        [JsonPropertyName("precision")]
        public int Precision { get; set; }
        
        [JsonPropertyName("scale")]
        public int Scale { get; set; }
        
        [JsonPropertyName("is_identity")]
        public bool IsIdentity { get; set; }
        
        [JsonPropertyName("column_default")]
        public string ColumnDefault { get; set; }
        
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }
}