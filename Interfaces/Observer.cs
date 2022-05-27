using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Interfaces
{
    public interface Observer
    {
        public void Update(double airTemperature, double airHumidity, double soilTemperature, double soilHumidity, double rainfall);
    }
}
