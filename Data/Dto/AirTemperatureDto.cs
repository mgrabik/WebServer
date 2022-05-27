using System;

namespace WebServer.Data.Dto
{
    public class AirTemperatureDto
    {
        public DateTime Date { get; set; }
        public double AirTemperature { get; set; }

        public AirTemperatureDto(double airTemperature, DateTime date)
        {
            this.AirTemperature = airTemperature;
            this.Date = date;
        }
    }
}
