using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebServer.Data.Dto;

namespace WebServer.Data
{
    public class Database
    {
        public List<AirTemperatureDto> airTemperatureDataSet = new List<AirTemperatureDto>();
        public List<AirHumidityDto> airHumidityDataSet = new List<AirHumidityDto>();
        public List<SoilTemperatureDto> soilTemperatureDataSet = new List<SoilTemperatureDto>();
        public List<SoilHumidityDto> soilHumidityDataSet = new List<SoilHumidityDto>();

        private const string connectionString = @"Data Source=DESKTOP-5M6N983\SQLEXPRESS;Initial Catalog=test_database;Integrated Security=True";

        private Database()
        {
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [Measurements] ORDER BY [Timestamp] DESC", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime timestamp = (DateTime)reader["Timestamp"];

                        AirTemperatureDto airTemperatureDto = new AirTemperatureDto(Double.Parse(String.Format("{0}", reader["airTemperature"])), timestamp);
                        airTemperatureDataSet.Add(airTemperatureDto);

                        AirHumidityDto airHumidityDto = new AirHumidityDto(Double.Parse(String.Format("{0}", reader["airHumidity"])), timestamp);
                        airHumidityDataSet.Add(airHumidityDto);

                        SoilTemperatureDto soilTemperatureDto = new SoilTemperatureDto(Double.Parse(String.Format("{0}", reader["soilTemperature"])), timestamp);
                        soilTemperatureDataSet.Add(soilTemperatureDto);

                        SoilHumidityDto soilHumidityDto = new SoilHumidityDto(Double.Parse(String.Format("{0}", reader["soilHumidity"])), timestamp);
                        soilHumidityDataSet.Add(soilHumidityDto);
                    }
                }
            }
        }
    }
}
