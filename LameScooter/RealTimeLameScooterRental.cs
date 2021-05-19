using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace LameScooter
{
    public class RealTimeLameScooterRental : ILameScooterRental
    {
        private const string Url = "https://raw.githubusercontent.com/marczaku/GP20-2021-0426-Rest-Gameserver/main/assignments/scooters.json";
        
        public async Task<int> GetScooterCountInStation(string stationName)
        {
            //TO DO FINISH THIS 
        }
    }
}