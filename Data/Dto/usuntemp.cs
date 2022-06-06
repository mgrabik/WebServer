using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Data.Dto
{
    public class usuntemp
    {
        public DateTime Date { get; set; }
        public double asd { get; set; }
        public usuntemp(double asd, DateTime date)
        {
            this.asd = asd;
            this.Date = date;
        }
    }
}
