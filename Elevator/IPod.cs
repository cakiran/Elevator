using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator
{
    
    public interface IPod
    {
        int CurrentFloor { get; set; }
        int TopFloor { get; set; }
        bool[] FloorReady { get; set; }
        bool[] PassengerIdentifierList { get; set; }
        List<int> PassengersToFloorsList { get; set; }
        SensorData SensorData { get; set; }
        int MaxWeight { get; set; }
        Task Ascend();
        Task Descend();
        Task Stay(int value);
    } 
    
    public class ElevatorPod : IPod
	{
        #region Public Properties
        public int CurrentFloor { get; set; } = 1;
        public int TopFloor { get; set; }
        public bool[] FloorReady { get; set; } = new bool[11];
        public SensorData SensorData { get; set; } = new SensorData();
        public int MaxWeight { get; set; }
        public bool[] PassengerIdentifierList { get; set; } = new bool[11];
        public List<int> PassengersToFloorsList { get; set; } = new List<int>();
        #endregion

        #region Private Fields
        private ILogger logger;
        #endregion

        #region Constructor

        public ElevatorPod(ILogger _logger, int NumberOfFloors = 10)
        {
            TopFloor = NumberOfFloors;
            logger = _logger;
        }
        #endregion

        #region Public Methods

        public async Task Descend()
        {
            SensorData.PodDirection = Direction.Down;
            for (int i = TopFloor - 1; i >= 1; i--)
            {
                if (FloorReady[i])
                {
                    SensorData.CurrentFloor = i;
                    SensorData.NextFloor = i + 1;
                    await Stay(i);
                    FloorReady[i] = false;
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
                else
                {
                    SensorData.CurrentFloor = i;
                    SensorData.NextFloor = i + 1;
                    SensorData.PodDirection = Direction.Down;
                    SensorData.PodStatus = PodStatus.Moving;
                    await logger.LogAsync(SensorData);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    continue;
                }
            }
        }

        public async Task Ascend()
        {
            SensorData.PodDirection = Direction.Up;
            for (int i = 1; i <= TopFloor; i++)
            {
                if (FloorReady[i])
                {
                    SensorData.CurrentFloor = i;
                    SensorData.NextFloor = i + 1;
                    await Stay(i);
                    FloorReady[i] = false;
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
                else
                {
                    SensorData.CurrentFloor = i;
                    SensorData.NextFloor = i + 1;
                    SensorData.PodDirection = Direction.Up;
                    SensorData.PodStatus = PodStatus.Moving;
                    if (SensorData.PodStatus == PodStatus.Moving && SensorData.CurrentFloor == 1)
                    {
                        continue;
                    }
                    await logger.LogAsync(SensorData);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    continue;
                }
            }
        }

        public async Task Stay(int passengerIdentifier)
        {
            SensorData.PodStatus = PodStatus.Stopped;
            if (PassengerIdentifierList[passengerIdentifier])
            {
                PassengerIdentifierList[passengerIdentifier] = false;
                foreach (var item in PassengersToFloorsList.Where(x => x == passengerIdentifier))
                {
                    --SensorData.NumberOfPassengers;
                }
                PassengersToFloorsList.RemoveAll(x => x == passengerIdentifier);
            }
            await logger.LogAsync(SensorData);
            await Task.Delay(TimeSpan.FromSeconds(1));
        } 
        #endregion
    }

    #region SensorData
    public class SensorData
    {
        public Direction PodDirection { get; set; } = Direction.Up;
        public int CurrentFloor { get; set; } = 1;
        public int NextFloor { get; set; } = 2;
        public PodStatus PodStatus { get; set; } = PodStatus.Stopped;
        public bool IsAboveMaxAllowedWeight
        {
            get
            {
                if (NumberOfPassengers * 100 > 1000)
                    return true;
                else return false;
            }
        }
        public int NumberOfPassengers { get; set; } = 0;

    }
    #endregion

    #region Enums

    public enum Direction
    {
        Up,
        Down
    }
    public enum PodStatus
    {
        Stopped,
        Moving
    } 
    #endregion
}
