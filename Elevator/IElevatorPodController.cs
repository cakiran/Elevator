using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface IElevatorPodController
    {
		void AddFloorFromInside(int value);
		void AddFloorFromOutside(int value);
		Task MovePodUpAndDown();
	}

    public class ElevatorPodController : IElevatorPodController
    {
        #region Public Fields
        public bool Running = true;
        #endregion

        #region Private Fields

        private IElevatorPod elevatorPod;
        ControlElevator controlElevator = new ControlElevator(); 
        #endregion


        #region Constructor
        public ElevatorPodController(IElevatorPod _elevatorPod)
        {
            elevatorPod = _elevatorPod;
            controlElevator.ElevatorEvent += new EventHandler(controlElevator_Stop);
        } 
        #endregion

        #region Public Methods
        public void AddFloorFromInside(int value)
        {
            Running = true;
            elevatorPod.FloorReady[value] = true;
            elevatorPod.PassengerIdentifierList[value] = true;
            ++elevatorPod.SensorData.NumberOfPassengers;
            elevatorPod.PassengersToFloorsList.Add(value);
        }
        public void AddFloorFromOutside(int value)
        {
            Running = true;
            if (value == -1)
            {
                controlElevator.Stop();
                return;
            }
            elevatorPod.FloorReady[value] = true;
        }
        public async Task MovePodUpAndDown()
        {
            while (Running)
            {
                await elevatorPod.Ascend();
                await elevatorPod.Descend();
                if (elevatorPod.FloorReady.All(x => x == false) && elevatorPod.PassengerIdentifierList.All(x => x == false) && elevatorPod.PassengersToFloorsList.Count() < 1)
                    controlElevator.Stop();
            }
        }
        #endregion

        #region Private Methods
        private void controlElevator_Stop(object sender, EventArgs e)
        {
            Running = false;
        } 
        #endregion
    }

    #region Enum
    public enum MoveDirection
    {
        Up,
        Down
    } 
    #endregion
}
