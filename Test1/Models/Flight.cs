using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tutorial3.Models
{
    public class Flight
    {
        [JsonProperty("plane")] public Plane Plane { get; set; }
        
        [JsonProperty("cityName")] public string CityName { get; set; }
        
    }
}