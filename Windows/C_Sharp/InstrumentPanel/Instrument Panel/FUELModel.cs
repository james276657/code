using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace Instrument_Panel
{
    class FUELModel : IFUELDoubleSubject, ITimeObserver
    {
        private static readonly Lazy<FUELModel> _instance = new Lazy<FUELModel>(() => new FUELModel());

        private ArrayList observers;
        private double gas, fuel, odom;

        private FUELModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);

        }
        public static FUELModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(IFUELDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IFUELDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double rpm)
        {
            foreach (IFUELDoubleObserver observer in observers)
            {
                observer.FUELUpdate(rpm);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }



        public void TimeUpdate(DateTime dt)
        {
            NotifyObserver(this.fuel);
        }

        public void UpdateGas(double gas)
        {
            this.gas = gas;

        }

        public void UpdateFuel(double fuel)
        {
            this.fuel += fuel;

        }
        public void UpdateODOM(double odom)
        {
            this.odom = odom;
            this.fuel -= this.odom / 20000;
            if (this.fuel <= 0) 
            { 
                this.fuel = 0;
                STARTModel startM = STARTModel.Instance;
                startM.UpdateSTART(0);


            }
        }
    }
}
