using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace Instrument_Panel
{
    class BATModel : ITimeObserver, IBATDoubleSubject, ISTARTDoubleObserver
    {
        private static readonly Lazy<BATModel> _instance = new Lazy<BATModel>(() => new BATModel());

        private DateTime startTime, currentTime;
        private double temp, start;

        private ArrayList observers;


        private BATModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            startTime = DateTime.Now;
            this.currentTime = DateTime.Now;
            STARTModel sm = STARTModel.Instance;
            sm.RegisterObserver(this);

        }
        public static BATModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(IBATDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IBATDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double bat)
        {
            foreach (IBATDoubleObserver observer in observers)
            {
                observer.BATUpdate(bat);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }



        public void TimeUpdate(DateTime dt)
        {
            this.currentTime = DateTime.Now;
            TimeSpan elapsed = this.currentTime - this.startTime;
            temp = 13.2;
            if (elapsed.TotalSeconds > 10)
            {
                if (this.start == 0) { temp = 0; }
                NotifyObserver(temp);
            }
            else
            {
                temp = 12.2 + (elapsed.TotalSeconds * 1 / 10);
                if (this.start == 0) { temp = 0; }
                NotifyObserver(temp);
            }
        }
        public void STARTUpdate(double start)
        {
            this.start = start;
        }



    }
}
