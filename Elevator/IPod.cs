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
		Task<string> Ascend();
		Task<string> Descend();
		Task Stay(int value);
	}
	public class ElevatorPod : IPod
	{
		public int CurrentFloor { get;set; } = 1;
		public int TopFloor { get ; set ; }
		public bool[] FloorReady { get ; set; } = new bool[11];
		public SensorData SensorData { get; set; } = new SensorData();
		public int MaxWeight { get; set; }
		public bool[] PassengerIdentifierList { get; set; } = new bool[11];
		public List<int> PassengersToFloorsList { get; set; } = new  List<int>();
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
					/*
				Bonus Enhancement:
				If the elevator has reached its weight limit, 
				it should stop only at floors that were selected from inside the elevator (to let passengers out), 
				until it is no longer at the max weight limit.
				  */
					if (SensorData.IsAboveMaxAllowedWeight && PassengerIdentifierList[i] || !SensorData.IsAboveMaxAllowedWeight)
						await Stay(i);
					else
						await LogWriter.LogAsync(SensorData);
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
					if (SensorData.IsAboveMaxAllowedWeight && PassengerIdentifierList[i] || !SensorData.IsAboveMaxAllowedWeight) //Bonus Enhancement Logic
						await Stay(i);
					else
						await LogWriter.LogAsync(SensorData);
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

		public async Task Stay(int passengerIdentifier)
		{
			SensorData.PodStatus = PodStatus.Stopped;
			if (PassengerIdentifierList[passengerIdentifier])
			{
				PassengerIdentifierList[passengerIdentifier] = false;
				foreach(var item in PassengersToFloorsList.Where(x => x == passengerIdentifier))
                {
					--SensorData.NumberOfPassengers;
				}
				PassengersToFloorsList.RemoveAll(x => x == passengerIdentifier);
			}
			await LogWriter.LogAsync(SensorData);
			await Task.Delay(TimeSpan.FromSeconds(1));
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
