using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace Instrument_Panel
{
    class STARTModel : ITimeObserver, ISTARTDoubleSubject, IFUELDoubleObserver
    {
        private static readonly Lazy<STARTModel> _instance = new Lazy<STARTModel>(() => new STARTModel());

        private ArrayList observers;
        private double fuel, START;

        private STARTModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            FUELModel fm = FUELModel.Instance;
            fm.RegisterObserver(this);
            this.START = 0;

        }
        public static STARTModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(ISTARTDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(ISTARTDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double START)
        {
            foreach (ISTARTDoubleObserver observer in observers)
            {
                observer.STARTUpdate(START);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }



        public void TimeUpdate(DateTime dt)
        {
            NotifyObserver(this.START);
        }

        public void FUELUpdate(double fuel)
        {
            this.fuel = fuel;
        }

        public void UpdateSTART(double START)
        {

            if (this.fuel > 0) { this.START = START; }

        }
    }
}
