using System;
using Newtonsoft.Json;

namespace Tutorial3
{
    public class Action
    {
        [JsonProperty("startTime")] public DateTime StartTime { get; set; }
        
        [JsonProperty("endTime")] public DateTime EndTime { get; set; }
        
        [JsonProperty("assignedTime")] public DateTime AssignedTime { get; set; }
        
        [JsonProperty("numberOfFirefighters")] public int NumOfFirefight { get; set; }
    }
}