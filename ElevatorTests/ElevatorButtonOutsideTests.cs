using Elevator;
using Moq;
using NUnit.Framework;
using System;

namespace ElevatorTests
{
    [TestFixture]
    public class ElevatorButtonOutsideTests
    {
        [Test]
        public void Press_ValidDirectionAndFloorInput_Runs_MovePodUpAndDown_AddFloorFromOutside_LogFloorAsync_Methods()
        {
            //arrange
            var elevatorPodControllerMock = new Mock<IElevatorPodController>();
            var loggerMock = new Mock<ILogger>();
            var elevatorButtonOutside = new ElevatorButtonOutside(elevatorPodControllerMock.Object, loggerMock.Object);
            //act
            var res = elevatorButtonOutside.Press("u8");
            //assert
            elevatorPodControllerMock.Verify(m => m.AddFloorFromOutside(It.IsAny<int>()), Times.Exactly(1));
            elevatorPodControllerMock.Verify(m => m.MovePodUpAndDown(), Times.Exactly(1));
            loggerMock.Verify(m => m.LogFloorAsync(It.IsAny<string>()), Times.Exactly(1));
        }
    }
}
