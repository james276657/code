using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;


namespace Instrument_Panel
{
    class TRIPModel : ITimeObserver, ITRIPDoubleSubject, IMPHDoubleObserver
    {
        private static readonly Lazy<TRIPModel> _instance = new Lazy<TRIPModel>(() => new TRIPModel());

        private ArrayList observers;

        private double gas, brake, mph, miles;

        private TRIPModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            MPHModel mm = MPHModel.Instance;
            mm.RegisterObserver(this);
            miles = 0;
        }
        public static TRIPModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(ITRIPDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(ITRIPDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double miles)
        {
            foreach (ITRIPDoubleObserver observer in observers)
            {
                observer.TRIPUpdate(miles);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }



        public void TimeUpdate(DateTime dt)
        {
            miles = miles + mph / 360000;
            NotifyObserver(miles);
        }

        public void UpdateGas(double gas)
        {
            this.gas = gas;

        }
        public void UpdateBrake(double brake)
        {
            this.brake = brake;

        }
        public void MPHUpdate(double mph)
        {
            this.mph = mph;
        }
        public void ResetTrip()
        {
            this.miles = 0;
        }
    }
}
