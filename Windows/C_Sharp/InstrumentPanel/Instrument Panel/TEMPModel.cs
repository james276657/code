using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace Instrument_Panel
{
    class TEMPModel : ITimeObserver, ITEMPDoubleSubject, ISTARTDoubleObserver
    {
        private static readonly Lazy<TEMPModel> _instance = new Lazy<TEMPModel>(() => new TEMPModel());

        private ArrayList observers;
        private DateTime startTime, currentTime;
        private double temp, start;
        private bool timestarted;

        private TEMPModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            this.startTime = DateTime.Now;
            this.currentTime = DateTime.Now;
            STARTModel sm = STARTModel.Instance;
            sm.RegisterObserver(this);
        }
        public static TEMPModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(ITEMPDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(ITEMPDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double rpm)
        {
            foreach (ITEMPDoubleObserver observer in observers)
            {
                observer.TEMPUpdate(rpm);
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
            temp = 180;
            if (this.start == 0) { temp = 0; }
            if (elapsed.TotalSeconds > 120)
            {
                NotifyObserver(temp);
            }
            else
            {
                temp = 100 + (elapsed.TotalSeconds * 80 / 120);
                if (this.start == 0) { temp = 0; }
                NotifyObserver(temp);
            }


        }
        public void STARTUpdate(double start)
        {
            this.start = start;
            if (this.start == 1 & (!(timestarted))) { this.startTime = DateTime.Now; timestarted = true; }
            if (this.start == 0 & (timestarted)) { timestarted = false; }
        }


    }
}
