using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface IElevatorPodController
    {
        bool Running { get; set; }
		void AddFloorFromInside(int value);
		void AddFloorFromOutside(int value);
		Task MovePodUpAndDown();
	}

    public class ElevatorPodController : IElevatorPodController
    {
        #region Public Fields
        public bool Running { get; set; }  = false;
        #endregion

        #region Private Fields

        private IElevatorPod elevatorPod;
        ControlElevator controlElevator = new ControlElevator();
        private static readonly object padlock = new object();
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
            if (Running)
            {
                lock (padlock)
                {
                    elevatorPod.FloorReady[value] = true;
                    elevatorPod.SensorData.PassengersToFloorsList.Add(value);
                }
            }
        }
        public void AddFloorFromOutside(int value)
        {
            if (value == -1)
            {
                controlElevator.Stop();
                return;
            }
            if(!elevatorPod.SensorData.IsAboveMaxAllowedWeight) //Bonus Enhancement Enhance the application as follows: If the elevator has reached its weight limit, it should stop only at floors that were selected from inside the elevator (to let passengers out), until it is no longer at the max weight limit.
                elevatorPod.FloorReady[value] = true;
        }
        public async Task MovePodUpAndDown()
        {
            while (Running)
            {
                if(elevatorPod.SensorData.CurrentFloor == 1 || elevatorPod.SensorData.CurrentFloor == 0)
                await elevatorPod.Ascend(); 
                else
                await elevatorPod.Descend();
                if (elevatorPod.FloorReady.All(x => x == false) && elevatorPod.SensorData.PassengersToFloorsList.Count() < 1)
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
