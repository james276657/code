using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace Instrument_Panel
{
    class GEARModel : ITimeObserver, IGEARDoubleSubject
    {

        enum Gears { PARK = 0, REVERSE, NEUTRAL, DRIVE, THIRD, SECOND };

        private static readonly Lazy<GEARModel> _instance = new Lazy<GEARModel>(() => new GEARModel());

        private ArrayList observers;
        private double gear;


        private GEARModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            this.gear = 0;
        }
        public static GEARModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(IGEARDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IGEARDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double rpm)
        {
            foreach (IGEARDoubleObserver observer in observers)
            {
                observer.GEARUpdate(rpm);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }



        public void TimeUpdate(DateTime dt)
        {
            NotifyObserver(this.gear);
        }
 
        public void UpdateGEAR(double GEAR)
        {

            this.gear = GEAR;

        }
    }
}
