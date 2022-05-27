using System;

namespace WebServer.Data.Dto
{
    public class AirHumidityDto
    {
        public DateTime Date { get; set; }
        public double AirHumidity { get; set; }
        public AirHumidityDto(double airHumidity, DateTime date)
        {
            this.AirHumidity = airHumidity;
            this.Date = date;
        }
    }
}
