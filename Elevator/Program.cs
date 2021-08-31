using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator
{
    class Program
    {
        private static ConcurrentQueue<string> _floorRequestQueue = new ConcurrentQueue<string>();
        private static bool _running = true;
        static void Main(string[] args)
        {
            var performElevatorPodMove = Task.Run(() => ProcessElevatorQueue());
            while (_running)
            {
                Console.WriteLine("Button presses from outside the elevator is done using '5D' or '8U' and from inside is done using just number of the floor like '8'");
                string directionAndFloor = Console.ReadLine();
                _floorRequestQueue.Enqueue(directionAndFloor);
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void ProcessElevatorQueue()
        {
            bool _running = true;
            IPod elevatorPod = new ElevatorPod();
            IPodController podController = new ElevatorPodController(elevatorPod);
            IButton elevatorButton = new ElevatorButton(podController);
            ControlElevator controlElevator = new ControlElevator();
            controlElevator.ElevatorEvent += new EventHandler(controlElevator_Stop);
            while (_running)
            {
                if (_floorRequestQueue.Count == 0) Thread.Sleep(100);
                else
                {
                    string directionAndFloor;
                    while (_floorRequestQueue.TryDequeue(out directionAndFloor))
                    {
                        if(directionAndFloor.Trim().ToLower() == "q")
                            controlElevator.Stop();
                         elevatorButton.Press(directionAndFloor);
                    }
                }
            }
        }

        private static void controlElevator_Stop(object sender, EventArgs e)
        {
            _running = false;
        }
    }

    public class ControlElevator
    {
        public event EventHandler ElevatorEvent;

        public void Stop()
        {
            ElevatorEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
