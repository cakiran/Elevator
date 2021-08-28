using Elevator;
using Moq;
using NUnit.Framework;
using System;

namespace ElevatorTests
{
    [TestFixture]
    public class ElevatorButtonTests
    {
        private IButton _elevatorButton;
        [SetUp]
        public void Setup()
        {
            var elevatorPodControllerMock = new Mock<IPodController>();
            _elevatorButton = new ElevatorButton(elevatorPodControllerMock.Object);
        }
        [Test]
        public void Press_InvalidDirection_Input_ReturnsErrorMessage()
        {
            //arrange
            //act
          var res =  _elevatorButton.Press("a9");
            //assert
            Assert.AreSame(res.Result, "Please enter valid direction and floor. E.g. u9 or d3");
        }
        [Test]
        public void Press_InvalidFloor_Input_ReturnsErrorMessage()
        {
            //arrange
            //act
            var res = _elevatorButton.Press("u9999");
            //assert
            Assert.AreSame(res.Result, "Invalid floor entry. Please select floors from 1 to 10.");
        }
        [Test]
        public void Press_ValidDirectionAndFloor_Input_CallsMovePodOnce()
        {
            //arrange
            var elevatorPodControllerMock = new Mock<IPodController>();
            var elevatorButton = new ElevatorButton(elevatorPodControllerMock.Object);
            //act
            var res = elevatorButton.Press("u8");
            //assert
            elevatorPodControllerMock.Verify(m => m.MovePod(It.IsAny<int>(), It.IsAny<MoveDirection>()), Times.Exactly(1));
        }
    }
}
