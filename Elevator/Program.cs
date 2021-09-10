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
                Console.WriteLine("Button presses from outside the elevator is done for example using 'D5' or 'U8', and from inside is done using just number of the floor like '8' and then please press enter.");
                string directionAndFloor = Console.ReadLine();
                _floorRequestQueue.Enqueue(directionAndFloor);
            }
            Console.WriteLine("Elevator turned Off!");
            Console.ReadKey();
        }

        #region Private Methods
        private  static void ProcessElevatorQueue()
        {
            ILogger logger = new LogWriter();
            IElevatorPod elevatorPod = new ElevatorPod(logger);
            IElevatorPodController podController = new ElevatorPodController(elevatorPod);
            IButton elevatorButtonOutside = new ElevatorButtonOutside(podController, logger);
            IButton elevatorButtonInside = new ElevatorButtonInside(podController, logger);
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
                        if (directionAndFloor.Trim().ToLower() == "q")
                            controlElevator.Stop();
                        if (int.TryParse(directionAndFloor, out int floor))
                            elevatorButtonInside.Press(directionAndFloor);
                        if (directionAndFloor.Trim().ToLower().StartsWith("u") || directionAndFloor.Trim().ToLower().StartsWith("d") || directionAndFloor.Trim().ToLower().StartsWith("q"))
                            elevatorButtonOutside.Press(directionAndFloor);
                    }
                }
            }
        }
        private static void controlElevator_Stop(object sender, EventArgs e)
        {
            _running = false;
        } 
        #endregion
    }

    #region ControlElevator Event
    public class ControlElevator
    {
        public event EventHandler ElevatorEvent;

        public void Stop()
        {
            ElevatorEvent?.Invoke(this, EventArgs.Empty);
        }
    } 
    #endregion
}
