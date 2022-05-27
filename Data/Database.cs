using System;
using System.Data.SqlClient;
using WebServer.Data.Dto;

namespace WebServer.Data
{
    public class Database
    {
        private int MAX = 3;

        public AirTemperatureDto[] airTemperatureDataSet;
        public AirHumidityDto[] airHumidityDataSet;
        public SoilTemperatureDto[] soilTemperatureDataSet;
        public SoilHumidityDto[] soilHumidityDataSet;

        private const string connectionString = @"Data Source=DESKTOP-5M6N983\SQLEXPRESS;Initial Catalog=test_database;Integrated Security=True";

        private Database()
        {
            airTemperatureDataSet = new AirTemperatureDto[MAX];
            airHumidityDataSet = new AirHumidityDto[MAX];
            soilTemperatureDataSet = new SoilTemperatureDto[MAX];
            soilHumidityDataSet = new SoilHumidityDto[MAX];
            SetInstance();
            getAllData();
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
        

        public void getAllData()
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [Measurements] ORDER BY [Timestamp] DESC", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime timestamp = (DateTime)reader["Timestamp"];
                        airTemperatureDataSet[i].AirTemperature = Double.Parse(String.Format("{0}", reader["airTemperature"]));
                        airTemperatureDataSet[i].Date = timestamp;

                        airHumidityDataSet[i].AirHumidity = Double.Parse(String.Format("{0}", reader["airHumidity"]));
                        airHumidityDataSet[i].Date = timestamp;

                        soilTemperatureDataSet[i].SoilTemperature = Double.Parse(String.Format("{0}", reader["soilTemperature"]));
                        soilTemperatureDataSet[i].Date = timestamp;

                        soilHumidityDataSet[i].SoilHumidity = Double.Parse(String.Format("{0}", reader["soilHumidity"]));
                        soilHumidityDataSet[i].Date = timestamp;

                        i++;
                    }
                }
            }
        }

        private void SetInstance()
        {
            for(int i = 0; i < MAX; i++)
            {
                airTemperatureDataSet[i] = new AirTemperatureDto();
                airHumidityDataSet[i] = new AirHumidityDto();
                soilTemperatureDataSet[i] = new SoilTemperatureDto();
                soilHumidityDataSet[i] = new SoilHumidityDto();
            }
        }
    }
}
