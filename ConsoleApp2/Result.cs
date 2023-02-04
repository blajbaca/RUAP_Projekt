    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CallRequestResponseService
{
    public class Result
    {
        [JsonProperty("output1")]
        public Output Output { get; set; }
    }

    public class Output
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public class Value
    {
        [JsonProperty("ColumnNames")]
        public List<string> ColumnNames { get; set; }

        [JsonProperty("ColumnTypes")]
        public List<string> ColumnTypes { get; set; }

        [JsonProperty("Values")]
        public List<List<object>> Values { get; set; }
    }
}
