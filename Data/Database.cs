using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebServer.Data.Dto;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;

namespace WebServer.Data
{
    public class Database
    {
        public List<AirTemperatureDto> airTemperatureDataSet = new List<AirTemperatureDto>();
        public List<AirHumidityDto> airHumidityDataSet = new List<AirHumidityDto>();
        public List<SoilTemperatureDto> soilTemperatureDataSet = new List<SoilTemperatureDto>();
        public List<SoilHumidityDto> soilHumidityDataSet = new List<SoilHumidityDto>();

        public double LiveAirTemperature;
        public double LiveAirHumidity;
        public double LiveSoilTemperature;
        public double LiveSoilHumidity;

        private Database()
        {
            setFirebaseClient();
            SelectFireBaseData();
            GetLiveData();
            //FakeData();
        }

        // Singleton implementation
        private static Database instance = null;
        public static Database Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }
        
        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "S9amkALnOl1aZTy4PAM5I47tjcE1FTHMhHDSf6RH",
            BasePath = "https://incz-soilmaster-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient fclinet;

        private void setFirebaseClient()
        {
            try
            {
                fclinet = new FireSharp.FirebaseClient(fcon);
            }
            catch
            {
                throw new Exception("There was a problem in the internet!");
            }
        }
        public void SelectFireBaseData()
        {
            clearDatabase();
            var result = fclinet.Get("Data");
            var json = result.ResultAs < Dictionary<string, Dictionary<string, string>>>();
            var allDictionaries = json.Values;
            foreach (var element in allDictionaries)
            {
                DateTime timestamp = DateTime.ParseExact(Validation(element["timestamp"]), "yyyy-MM-dd HH:mm:ss ", null);
                
                airTemperatureDataSet.Add(new AirTemperatureDto(Convert.ToDouble(Regex.Replace(element["temperatura"], @"\.", ",")), timestamp));
                airHumidityDataSet.Add(new AirHumidityDto(Convert.ToDouble(Regex.Replace(element["wilgotnosc"], @"\.", ",")), timestamp));
                soilTemperatureDataSet.Add(new SoilTemperatureDto(Convert.ToDouble(Regex.Replace(element["temperatura_gleby"], @"\.", ",")), timestamp));
                soilHumidityDataSet.Add(new SoilHumidityDto(Convert.ToDouble(Regex.Replace(element["wilgotnosc_gleby"], @"\.", ",")), timestamp));
            }
        }
        public void GetLiveData()
        {
            var result = fclinet.Get("Data");
            var json = result.ResultAs<Dictionary<string, Dictionary<string, string>>>();
            var allDictionaries = json.Values;
            int maxiteration = allDictionaries.Count;
            int i = 0;
            foreach (var element in allDictionaries)
            {
                if (i < maxiteration - 1)
                {
                    i++;
                    continue;
                }
                LiveAirTemperature = Convert.ToDouble(Regex.Replace(element["temperatura"], @"\.", ","));
                LiveAirHumidity = Convert.ToDouble(Regex.Replace(element["wilgotnosc"], @"\.", ","));
                LiveSoilTemperature = Convert.ToDouble(Regex.Replace(element["temperatura_gleby"], @"\.", ","));
                LiveSoilHumidity = Convert.ToDouble(Regex.Replace(element["wilgotnosc_gleby"], @"\.", ","));
            }
        }

        private string Validation(string date)
        {
            var month = Regex.Match(date, "-(.*?)-(.*?) ").Groups[1];
            var day = Regex.Match(date, "-(.*?)-(.*?) ").Groups[2];
            var hour = Regex.Match(date, " (.*?):").Groups[1];
            var minute = Regex.Match(date, ":(.*?):").Groups[1];
            var second = Regex.Match(date, ":(.*?):(.*?) ").Groups[2];

            var regex = new Regex(Regex.Escape("-"));
            string result = regex.Replace(date, "%", 1);

            regex = new Regex(Regex.Escape(":"));
            result = regex.Replace(result, "#", 1);

            if (month.Length == 1)
            {
                string newMonth = "%0" + month.Value + "-";
                result = Regex.Replace(result, "%(.*?)-", newMonth);
            }
            if (day.Length == 1)
            {
                string newDay = "-0" + day.Value + " ";
                result = Regex.Replace(result, "-(.*?) ", newDay);
            }
            if (hour.Length == 1)
            {
                string newHour = " 0" + hour.Value + "#";
                result = Regex.Replace(result, " (.*?)#", newHour);
            }
            if (minute.Length == 1)
            {
                string newMinute = "#0" + minute.Value + ":";
                result = Regex.Replace(result, "#(.*?):", newMinute);
            }
            if (second.Length == 1)
            {
                string newSecond = ":0" + second.Value + " ";
                result = Regex.Replace(result, ":(.*?) ", newSecond);
            }
            regex = new Regex(Regex.Escape("%"));
            string result3 = regex.Replace(result, "-");
            regex = new Regex(Regex.Escape("#"));
            string result4 = regex.Replace(result3, ":");

            return result4;
        }

        private void clearDatabase()
        {
            airTemperatureDataSet.Clear();
            airHumidityDataSet.Clear();
            soilTemperatureDataSet.Clear();
            soilHumidityDataSet.Clear();
        }

        public void FakeData()
        {
            Random random = new Random();

            for (int i = 0; i < 30; i++)
            {
                DateTime timestamp = new DateTime(2022, 6, i + 1, 12, 0, 0);

                airTemperatureDataSet.Add(new AirTemperatureDto((double)(random.Next(120, 230) / 10), timestamp));
                airHumidityDataSet.Add(new AirHumidityDto((double)(random.Next(720, 870) / 10), timestamp));
                soilTemperatureDataSet.Add(new SoilTemperatureDto((double)(random.Next(60, 130) / 10), timestamp));
                soilHumidityDataSet.Add(new SoilHumidityDto((double)(random.Next(320, 500) / 10), timestamp));
            }
        }
    }
}
