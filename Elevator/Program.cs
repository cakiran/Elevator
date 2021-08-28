using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator
{
    class Program
    {
        private static ConcurrentQueue<string> _floorRequestQueue = new ConcurrentQueue<string>();
        static void Main(string[] args)
        {
            var performElevatorPodMove = Task.Run(() => ProcessElevatorQueue());
            bool _running = true;
            while (_running)
            {
                Console.WriteLine("Please enter direction up or down (u/d) followed by floor number to go (1 - 10). E.g. u9 or d3");
                string directionAndFloor = Console.ReadLine();
                _floorRequestQueue.Enqueue(directionAndFloor);
            }
            Console.ReadKey();
        }

        private static async Task ProcessElevatorQueue()
        {
            bool _running = true;
            IPod elevatorPod = new ElevatorPod();
            IPodController podController = new ElevatorPodController(elevatorPod);
            IButton elevatorButton = new ElevatorButton(podController);
            while (_running)
            {
                if (_floorRequestQueue.Count == 0) Thread.Sleep(100);
                else
                {
                    string directionAndFloor;
                    while (_floorRequestQueue.TryDequeue(out directionAndFloor))
                    {
                        string errorMessage = await elevatorButton.Press(directionAndFloor);
                        Console.WriteLine(errorMessage);
                    }
                }
            }
        }
    }
}
