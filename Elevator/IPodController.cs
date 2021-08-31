using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface IPodController
    {
        void StartPod();
		void AddFloorFromInside(int value);
		void AddFloorFromOutside(int value);
	}

    public class ElevatorPodController : IPodController
    {
		private bool _running = true;
		private IPod elevatorPod;

        public ElevatorPodController(IPod _elevatorPod)
        {
			elevatorPod = _elevatorPod;
			ControlElevator controlElevator = new ControlElevator();
			controlElevator.ElevatorEvent += new EventHandler(controlElevator_Stop);
		}

        private void controlElevator_Stop(object sender, EventArgs e)
        {
			_running = false;
		}

        public async void StartPod()
        {
			await Task.Run(() => MovePodUpAndDown());
		}

		public void AddFloorFromInside(int value)
        {
			elevatorPod.FloorReady[value] = true;
			elevatorPod.SensorData.NumberOfPassengers++;
		}
		public void AddFloorFromOutside(int value)
		{
			elevatorPod.FloorReady[value] = true;
		}
		private async Task MovePodUpAndDown()
        {
			while (_running)
			{
				await elevatorPod.Ascend();
				await elevatorPod.Descend();
			}
		}
    }
	public enum MoveDirection
	{
		Up,
		Down
	}
}
