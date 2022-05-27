using System;

namespace WebServer.Data.Dto
{
    public class SoilTemperatureDto
    {
        public DateTime Date { get; set; }
        public double SoilTemperature { get; set; }
        public SoilTemperatureDto(double soilTemperature, DateTime date)
        {
            this.SoilTemperature = soilTemperature;
            this.Date = date;
        }
    }
}
