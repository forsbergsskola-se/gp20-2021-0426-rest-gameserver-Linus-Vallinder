using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LameScooter
{
    public class DeprecatedLameScooterRental : ILameScooterRental
    {
        public async Task<int> GetScooterCountInStation(string stationName)
        {
            List<StationList> stationList = new List<StationList>();
            
            foreach (var stationText in await System.IO.File.ReadAllLinesAsync(@"scooters.txt"))
            {
                var station = new StationList();
                var stationInfo = stationText.Split(" : ");

                station.Name = stationInfo[0];
                station.BikesAvailable = Int32.Parse(stationInfo[1]);
                
                stationList.Add(station);
            }
            return (stationList ?? throw new InvalidOperationException()).Where(station => station.Name == stationName).Select(station => station.BikesAvailable).FirstOrDefault();
        }
    }
}