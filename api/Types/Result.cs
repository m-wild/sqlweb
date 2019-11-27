using System.Collections.Generic;
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
    
    public class Result
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; } = new List<string>();

        [JsonProperty("rows")]
        public List<object[]> Rows { get; set; } = new List<object[]>();
    }
    
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

    public static class ResultExtensions
    {
        public static Dictionary<string, Objects> ToObjects(this Result result)
        {
            var objects = new Dictionary<string, Objects>();

            foreach (var row in result.Rows)
            {
                var schema = (string)row[0];
                var name = (string)row[1];
                var type = (string)row[2];

                if (!objects.ContainsKey(schema))
                {
                    objects[schema] = new Objects();
                }

                switch (type)
                {
                    case "table":
                        objects[schema].Tables.Add(name);
                        break;
                    case "view":
                        objects[schema].Views.Add(name);
                        break;
                    case "procedure":
                        objects[schema].Procedures.Add(name);
                        break;
                    case "function":
                        objects[schema].Functions.Add(name);
                        break;
                    case "sequence":
                        objects[schema].Sequences.Add(name);
                        break;
                }
            }

            return objects;
        }
        
    }
}