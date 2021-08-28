using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator
{
    public interface IPodController
    {
        Task<string> MovePod(int value, MoveDirection moveDirection);
    }

    public class ElevatorPodController : IPodController
    {
		private IPod elevatorPod;
        public ElevatorPodController(IPod _elevatorPod)
        {
			elevatorPod = _elevatorPod;

		}
        public async Task<string> MovePod(int floor,MoveDirection moveDirection)
        {
			string errorMessage = string.Empty;
			switch (moveDirection)
			{
				case MoveDirection.Down:
					errorMessage = await elevatorPod.Descend(floor);
					break;
				case MoveDirection.Up:
					errorMessage = await elevatorPod.Ascend(floor);
					break;
				default:
					break;
			}
			return errorMessage;
		}
	}
	public enum MoveDirection
	{
		Up,
		Down
	}
}
