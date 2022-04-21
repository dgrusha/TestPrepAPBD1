using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tutorial3.Models
{
    public class Plane
    {
        [JsonProperty("idPlane")] public int IDPlane { get; set; }
        
        [JsonProperty("name")] public string Name { get; set; }
        
        [JsonProperty("maxSeats")] public int MaxSeats { get; set; }
        
    }
}