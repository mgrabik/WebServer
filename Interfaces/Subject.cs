using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Interfaces
{
    public interface Subject
    {
        public void RegisterObserver(Observer observer);
        public void UnRegisterObserver(Observer observer);
        public void notifyObservers();
    }
}
