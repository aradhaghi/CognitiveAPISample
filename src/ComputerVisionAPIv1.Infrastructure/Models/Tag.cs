using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVisionAPIv1.Infrastructure.Models
{
    public class Tag
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "confidence")]
        public decimal Confidence { get; set; }

        [JsonProperty(PropertyName = "hint")]
        public string Hint { get; set; }
    }
}
