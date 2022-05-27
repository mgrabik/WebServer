using System;

namespace WebServer.Data.Dto
{
    public class SoilHumidityDto
    {
        public DateTime Date { get; set; }
        public double SoilHumidity { get; set; }
        public SoilHumidityDto(double soilHumidity, DateTime date)
        {
            this.SoilHumidity = soilHumidity;
            this.Date = date;
        }
    }
}
