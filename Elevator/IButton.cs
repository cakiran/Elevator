using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface IButton
    {
        Task Press(string inputDetails);
    }

    #region ElevatorButtonOutside
    public class ElevatorButtonOutside : IButton
    {
        private IElevatorPodController elevatorPodController;
        private ILogger logger;
        public ElevatorButtonOutside(IElevatorPodController _podController, ILogger _logger)
        {
            elevatorPodController = _podController;
            logger = _logger;
        }
        public async Task Press(string directionAndFloor)
        {
            int floor;
            char[] directionAndFloorArr = directionAndFloor.ToCharArray();
            char direction = directionAndFloorArr[0];
            if (direction.ToString().ToLower() == "q")
                elevatorPodController.AddFloorFromOutside(-1);
            MoveDirection moveDirection = directionAndFloorArr[0] == 'u' ? MoveDirection.Up : MoveDirection.Down;
            string directionRemoved = directionAndFloor.Remove(0, 1);
            if (int.TryParse(directionRemoved, out floor))
            {
                if (floor < 1 || floor > 11)
                {
                    Console.WriteLine("Invalid floor entry. Please select floors from 1 to 10.");
                }
                elevatorPodController.AddFloorFromOutside(floor);
                await logger.LogFloorAsync(directionAndFloor);
                if (!elevatorPodController.Running)
                {
                    elevatorPodController.Running = true;
                    await elevatorPodController.MovePodUpAndDown();
                }
            }
        }
    }
    #endregion

    #region ElevatorButtonInside

    public class ElevatorButtonInside : IButton
    {
        private IElevatorPodController elevatorPodController;
        private ILogger logger;
        public ElevatorButtonInside(IElevatorPodController _podController, ILogger _logger)
        {
            elevatorPodController = _podController;
            logger = _logger;
        }
        public async Task Press(string directionAndFloor)
        {
            int floor;
            var isNumeric = int.TryParse(directionAndFloor, out floor);
            if (isNumeric)
            {
                if (floor < 1 || floor > 11)
                {
                    Console.WriteLine("Invalid floor entry. Please select floors from 1 to 10.");
                }
                elevatorPodController.AddFloorFromInside(floor);
            }
            await logger.LogFloorAsync(directionAndFloor);
        }
    } 
    #endregion
}
