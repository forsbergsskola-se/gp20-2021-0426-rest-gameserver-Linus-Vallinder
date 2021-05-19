using System;
using System.Linq;
using System.Threading.Tasks;

namespace LameScooter
{
    static class Program
    {
        private static async Task Main(string[] args)
        {
            ILameScooterRental rental = null;
            
            if (args[0].Length == 0)
            {
                throw new ArgumentException("Station name must be provided.");
            }
            
            if (args[0].Any(char.IsDigit))
            {
                throw new ArgumentException("Station name must not contain any numbers.");
            }

            if (args.Length > 1)
            {
                switch (args[1])
                {
                    case "offline":
                        rental = new OfflineLameScooterRental();
                        break;
                    case "deprecated":
                        rental = new DeprecatedLameScooterRental();
                        break;
                    case "realtime":
                        rental = new RealTimeLameScooterRental();
                        break;
                }
            }
            else
            {
                rental = new OfflineLameScooterRental();
            } 
            
            switch (args[1])
            {
                case "offline":
                    rental = new OfflineLameScooterRental();
                    break;
                case "deprecated":
                    rental = new DeprecatedLameScooterRental();
                    break;
            }

            if (rental != null)
            {
                var count = await rental.GetScooterCountInStation(args[0]);
                Console.WriteLine($"Number of Scooters at this Station is {count}");
            }
        }
    }
}
