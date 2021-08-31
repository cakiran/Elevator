using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public class LogWriter
    {
        public static async Task LogAsync(SensorData sensorData)
        {
            using StreamWriter file = new("ElevatorLog.txt", append: true);
            await file.WriteLineAsync($"Timestamp - {DateTime.Now} Floor - {sensorData.CurrentFloor} PodStatus - {sensorData.PodStatus} IsAboveMaxAllowedWeight(max 1000lbs) - {sensorData.IsAboveMaxAllowedWeight} Passengers - {sensorData.NumberOfPassengers}");
        }
        public static async Task LogFloorAsync(string floorRequest)
        {
            using StreamWriter file = new("ElevatorLog.txt", append: true);
            await file.WriteLineAsync($"Timestamp - {DateTime.Now} FloorRequest - {floorRequest}");
        }
    }
}
