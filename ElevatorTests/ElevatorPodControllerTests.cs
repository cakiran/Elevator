using Elevator;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorTests
{
    [TestFixture]
    public class ElevatorPodControllerTests
    {

        [Test]
        public void MovePod_ValidInput_ReturnsEmptyString()
        {
            //arrange
            var elevatorPodMock = new Mock<IPod>();
            elevatorPodMock.Setup(x => x.CurrentFloor).Returns(4);
            elevatorPodMock.Setup(x => x.Descend(1)).Returns(Task.FromResult(string.Empty));
            var elevatorPodController = new ElevatorPodController(elevatorPodMock.Object);
            //act
            var res = elevatorPodController.MovePod(1,MoveDirection.Down);
            //assert
            Assert.AreSame(res.Result, string.Empty);
        }
        [Test]
        public async Task MovePod_InvalidFloorInput_ReturnsErrorMessage()
        {
            //arrange
            var elevatorPodMock = new Mock<IPod>();
            elevatorPodMock.Setup(x => x.CurrentFloor).Returns(4);
            elevatorPodMock.Setup(x => x.Ascend(9999)).Returns(Task.FromResult("Invalid floor entry. Please select floors from 1 to 10."));
            var elevatorPodController = new ElevatorPodController(elevatorPodMock.Object);
            //act
            var res = await elevatorPodController.MovePod(9999, MoveDirection.Up);
            //assert
            Assert.AreSame(res, "Invalid floor entry. Please select floors from 1 to 10.");
        }
    }
}
