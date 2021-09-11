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
        public void AddFloorFromInside_WhenFloorIsInput_SetFloorReadyAndAddsToPassengersList()
        {
            //arrange
            var elevatorPod = new Mock<IElevatorPod>();
            elevatorPod.Setup(x => x.FloorReady).Returns(new bool[11]);
            elevatorPod.Setup(x => x.SensorData).Returns(SensorData.Instance);
            var elevatorController = new ElevatorPodController(elevatorPod.Object);
            elevatorController.Running = true;
            //act
             elevatorController.AddFloorFromInside(8);
            //assert
            Assert.That(elevatorPod.Object.FloorReady[8] == true);
            Assert.That(elevatorPod.Object.SensorData.NumberOfPassengers > 0);
        }

    }
}
