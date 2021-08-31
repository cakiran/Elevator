using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface IButton
    {
        void Press(string inputDetails);
    }

    public class ElevatorButton : IButton
    {
        private IPodController elevatorPodController;
		public ElevatorButton(IPodController _podController)
        {
            elevatorPodController = _podController;
            elevatorPodController.StartPod();
        }
        public async void Press(string directionAndFloor)
        {
            int floor;
            var isNumeric = int.TryParse(directionAndFloor, out floor);
            if (isNumeric)
            {
                if(floor < 1 || floor > 10)
                {
                    // return "Invalid floor entry. Please select floors from 1 to 10.";
                }
                elevatorPodController.AddFloorFromInside(floor);
            }
            if (string.IsNullOrEmpty(directionAndFloor))
            {
               // return "Please enter valid direction and floor. E.g. u9 or d3";
            }
            char[] directionAndFloorArr = directionAndFloor.ToCharArray();
            char direction = directionAndFloorArr[0]; 
            if(direction != 'u' && direction != 'd')
            {
                //return "Please enter valid direction and floor. E.g. u9 or d3";
            }
            MoveDirection moveDirection = directionAndFloorArr[0] == 'u' ? MoveDirection.Up : MoveDirection.Down;
            string directionRemoved = directionAndFloor.Remove(0, 1);
            if (int.TryParse(directionRemoved, out floor))
            {
                if (floor < 1 || floor > 10)
                {
                    // return "Invalid floor entry. Please select floors from 1 to 10.";
                }
                elevatorPodController.AddFloorFromOutside(floor);
            }
            
            await  LogWriter.LogFloorAsync(directionAndFloor);
        }
		
	}
}
