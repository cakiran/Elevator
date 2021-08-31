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
		SensorData SensorData { get; set; }
		int MaxWeight { get; set; }
		Task<string> Ascend();
		Task<string> Descend();
		Task Stay();
	}
    public class ElevatorPod : IPod
    {
		private int _currentFloor = 1;
		public int CurrentFloor { get => _currentFloor; set => _currentFloor = value; }
		private int _topFloor;
		public int TopFloor { get => _topFloor; set => _topFloor = value; }
		private bool[] _floorReady = new bool[11];
		public bool[] FloorReady { get => _floorReady; set => _floorReady = value; }
		private SensorData _sensorData = new SensorData();
        public SensorData SensorData { get => _sensorData; set => _sensorData = value; }
		private int _maxWeight;
        public int MaxWeight { get => _maxWeight; set => _maxWeight = value; }

        public ElevatorPod(int NumberOfFloors = 10)
		{
			TopFloor = NumberOfFloors;
		}

		public async Task<string> Descend()
		{

			for (int i = TopFloor - 1; i >= 1; i--)
			{
				if (FloorReady[i])
				{
					SensorData.CurrentFloor = i;
					SensorData.NextFloor = i + 1;
					await Stay();
					FloorReady[i] = false;
					await Task.Delay(TimeSpan.FromSeconds(3));
				}
				else
				{
					SensorData.CurrentFloor = i;
					SensorData.NextFloor = i + 1;
					SensorData.PodDirection = PodDirection.Down;
					SensorData.PodStatus = PodStatus.Moving;
					await LogWriter.LogAsync(SensorData);
					await Task.Delay(TimeSpan.FromSeconds(3));
					continue;
				}
			}
			return string.Empty;
		}

		public async Task<string> Ascend()
		{
			for (int i = 1; i <= TopFloor; i++)
			{
				if (FloorReady[i])
				{
					SensorData.CurrentFloor = i;
					SensorData.NextFloor = i + 1;
					await Stay();
					FloorReady[i] = false;
					await Task.Delay(TimeSpan.FromSeconds(3));
				}
				else
				{
					SensorData.CurrentFloor = i;
					SensorData.NextFloor = i + 1;
					SensorData.PodDirection = PodDirection.Up;
					SensorData.PodStatus = PodStatus.Moving;
					if (SensorData.PodStatus == PodStatus.Moving && SensorData.CurrentFloor == 1)
					{
						continue;
					}
					await LogWriter.LogAsync(SensorData);
					await Task.Delay(TimeSpan.FromSeconds(3));
					continue;
				}
			}
			return string.Empty;
		}

		public async Task Stay()
        {
			SensorData.PodStatus = PodStatus.Stopped;
			await LogWriter.LogAsync(SensorData);
			await Task.Delay(TimeSpan.FromSeconds(1));
			--SensorData.NumberOfPassengers;
		}
    }

	public class SensorData
    {
		public PodDirection PodDirection { get; set; } = PodDirection.Up;
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

    public enum PodDirection
    {
		Up,
		Down
    }
	public enum PodStatus
    {
		Stopped,
		Moving
    }
}
