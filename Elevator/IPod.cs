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
		Task<string> Ascend(int value);
		Task<string> Descend(int value);
		Task Stay(int value);
	}
    public class ElevatorPod : IPod
    {
		private int _currentFloor;
		public int CurrentFloor { get => _currentFloor; set => _currentFloor = value; }
		private int _topFloor;
		public int TopFloor { get => _topFloor; set => _topFloor = value; }

		public ElevatorPod(int NumberOfFloors = 10)
		{
			TopFloor = NumberOfFloors;
		}

		public async Task<string> Descend(int floor)
		{
			if(floor > CurrentFloor)
            {
				return "Wrong Direction Choice.";
            }
			for (int i = CurrentFloor; i >= 1; i--)
			{
				if (floor == i)
					await Stay(floor);
				else
					continue;
			}
			return string.Empty;
		}

		public async Task<string> Ascend(int floor)
		{
			if (floor < CurrentFloor)
			{
				return "Wrong Direction Choice.";
			}
			for (int i = CurrentFloor; i <= TopFloor; i++)
			{
				if (floor == i)
					await Stay(floor);
				else
					continue;
			}
			return string.Empty;
		}

		public async Task Stay(int floor)
        {
			Console.WriteLine($"You have reached your floor - {floor}. Elevator will wait for 5 seconds.");
			CurrentFloor = floor;
			await Task.Delay(TimeSpan.FromSeconds(5));
		}
    }
}
