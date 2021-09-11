using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface ILogger
    {
        Task LogAsync(SensorData sensorData);
        Task LogFloorAsync(string floorRequest);
    }
    public class LogWriter : ILogger
    {
        #region Constructor
        public LogWriter()
        {

        }
        #endregion

        #region Public Methods
        public async Task LogAsync(SensorData sensorData)
        {
            using StreamWriter file = new("ElevatorLog.txt", append: true);
            await file.WriteLineAsync($"Timestamp - {DateTime.Now} Floor - {sensorData.CurrentFloor} PodStatus - {sensorData.PodStatus} IsAboveMaxAllowedWeight(max 1500lbs) - {sensorData.IsAboveMaxAllowedWeight} Passengers - {sensorData.NumberOfPassengers} Direction - {sensorData.PodDirection.ToString().ToUpper()} PassengerList - {string.Join(",",sensorData.PassengersToFloorsList)}");
        }
        public async Task LogFloorAsync(string floorRequest)
        {
            using StreamWriter file = new("ElevatorLog.txt", append: true);
            await file.WriteLineAsync($"Timestamp - {DateTime.Now} FloorRequest - {floorRequest}");
        } 
        #endregion
    }
}
