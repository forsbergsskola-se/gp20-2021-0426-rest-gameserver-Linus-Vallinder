using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LameScooter
{
    public class OfflineLameScooterRental : ILameScooterRental
    {
        public async Task<int> GetScooterCountInStation(string stationName)
        {
            var reader = new StreamReader("scooters.json");
            var result = await reader.ReadToEndAsync();
            var stations = JsonSerializer.Deserialize<List<StationList>>(result, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            
            if (stations != null && stations.Find(station => station.Name == stationName) == null)
            {
                throw new NotFoundException($"Station {stationName} could not be found");
            }
            
            return (stations ?? throw new InvalidOperationException()).Where(station => station.Name == stationName).Select(station => station.BikesAvailable).FirstOrDefault();
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string exceptionString) : base(exceptionString)
        {
            
        }
    }
}