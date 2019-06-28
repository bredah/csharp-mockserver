using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockServer.Tests.Models
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "description")]
        public String Description { get; set; }
        [JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }
}
