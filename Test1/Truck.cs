using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tutorial3
{
    public class Truck
    {
        [JsonProperty("idTruck")] public int IDTuck { get; set; }
        
        [JsonProperty("operationNumber")] public string OperationNumber { get; set; }
        
        [JsonProperty("specialEquip")] public bool SpecialEquipment { get; set; }
        
        [JsonProperty("listOfActions")] public List<Action> Actions { get; set; }
    }
}