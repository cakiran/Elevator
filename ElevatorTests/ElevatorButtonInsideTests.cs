using Elevator;
using Moq;
using NUnit.Framework;
using System;

namespace ElevatorTests
{
    [TestFixture]
    public class ElevatorButtonInsideTests
    {
        [Test]
        public void Press_ValidDirectionAndFloorInput_Runs_AddFloorFromInside_LogFloorAsync_Methods()
        {
            //arrange
            var elevatorPodControllerMock = new Mock<IPodController>();
            var loggerMock = new Mock<ILogger>();
            var elevatorButtonInside = new ElevatorButtonInside(elevatorPodControllerMock.Object, loggerMock.Object);
            //act
            var res = elevatorButtonInside.Press("8");
            //assert
            elevatorPodControllerMock.Verify(m => m.AddFloorFromInside(It.IsAny<int>()), Times.Exactly(1));
            loggerMock.Verify(m => m.LogFloorAsync(It.IsAny<string>()), Times.Exactly(1));
        }
    }
}
