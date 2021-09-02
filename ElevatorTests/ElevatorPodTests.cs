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
    public class ElevatorPodTests
    {
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(9)]
        public async Task Descend_WhenExactFloorIsReached_ResetsTrueFloorFlagToFalse(int floor)
        {
            //arrange
            var loggerMock = new Mock<ILogger>();
            var elevatorPod = new ElevatorPod(loggerMock.Object);
            bool[] floorReady = new bool[11];
            floorReady[floor] = true;
            elevatorPod.FloorReady = floorReady;
            //act
            await elevatorPod.Descend();
            //assert
            Assert.IsFalse(floorReady[floor]);
        }
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public async Task Ascend_WhenExactFloorIsReached_ResetsTrueFloorFlagToFalse(int floor)
        {
            //arrange
            var loggerMock = new Mock<ILogger>();
            var elevatorPod = new ElevatorPod(loggerMock.Object);
            bool[] floorReady = new bool[11];
            floorReady[floor] = true;
            elevatorPod.FloorReady = floorReady;
            //act
            await elevatorPod.Ascend();
            //assert
            Assert.IsFalse(floorReady[floor]);
        }
        [Test]
        public async Task Stay_WhenExactFloorIsReached_StopsThePod()
        {
            //arrange
            var loggerMock = new Mock<ILogger>();
            var elevatorPod = new ElevatorPod(loggerMock.Object);
            bool[] floorReady = new bool[11];
            floorReady[4] = true;
            elevatorPod.FloorReady = floorReady;
            //act
            await elevatorPod.Stay(4);
            //assert
            Assert.That(elevatorPod.SensorData.PodStatus == PodStatus.Stopped);
            loggerMock.Verify(m => m.LogAsync(elevatorPod.SensorData), Times.Exactly(1));
        }
    }
}
