using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface IPodController
    {
		void AddFloorFromInside(int value);
		void AddFloorFromOutside(int value);
		Task MovePodUpAndDown();
	}

    public class ElevatorPodController : IPodController
    {
		private bool _running = true;
		private IPod elevatorPod;
		ControlElevator controlElevator = new ControlElevator();
		public ElevatorPodController(IPod _elevatorPod)
        {
			elevatorPod = _elevatorPod;
			
			controlElevator.ElevatorEvent += new EventHandler(controlElevator_Stop);
		}
        private void controlElevator_Stop(object sender, EventArgs e)
        {
			_running = false;
		}
		public void AddFloorFromInside(int value)
        {
			_running = true;
			elevatorPod.FloorReady[value] = true;
			elevatorPod.PassengerIdentifierList[value] = true;
			++elevatorPod.SensorData.NumberOfPassengers;
			elevatorPod.PassengersToFloorsList.Add(value);
		}
		public void AddFloorFromOutside(int value)
		{
			_running = true;
			if (value == -1)
			{
				controlElevator.Stop();
				return;
			}
			elevatorPod.FloorReady[value] = true;
		}
		public async Task MovePodUpAndDown()
        {
			while (_running)
			{
				await elevatorPod.Ascend();
				await elevatorPod.Descend();
                if (elevatorPod.FloorReady.All(x => x == false) && elevatorPod.PassengerIdentifierList.All(x => x == false) && elevatorPod.PassengersToFloorsList.Count() < 1)
                    controlElevator.Stop();
            }
		}
    }
	public enum MoveDirection
	{
		Up,
		Down
	}
}
