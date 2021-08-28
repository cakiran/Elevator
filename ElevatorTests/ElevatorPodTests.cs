using Elevator;
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
        private IPod _elevatorPod;
        [SetUp]
        public void Setup()
        {
            _elevatorPod = new ElevatorPod();
        }
        [Test]
        public void Ascend_WrongDirectionChoice_ReturnsErrorMessage()
        {
            //arrange
            _elevatorPod.CurrentFloor = 5;
            //act
            var res = _elevatorPod.Ascend(3);
            //assert
            Assert.AreSame(res.Result, "Wrong Direction Choice.");
        }
        [Test]
        public void Descend_WrongDirectionChoice_ReturnsErrorMessage()
        {
            //arrange
            _elevatorPod.CurrentFloor = 3;
            //act
            var res = _elevatorPod.Descend(5);
            //assert
            Assert.AreSame(res.Result, "Wrong Direction Choice.");
        }
        [Test]
        public void Ascend_CorrectDirectionChoice_ReturnsEmptyString()
        {
            //arrange
            _elevatorPod.CurrentFloor = 5;
            //act
            var res = _elevatorPod.Ascend(8);
            //assert
            Assert.AreSame(res.Result, "");
        }
        [Test]
        public void Descend_CorrectDirectionChoice_ReturnsEmptyString()
        {
            //arrange
            _elevatorPod.CurrentFloor = 9;
            //act
            var res = _elevatorPod.Descend(8);
            //assert
            Assert.AreSame(res.Result, "");
        }
    }
}
