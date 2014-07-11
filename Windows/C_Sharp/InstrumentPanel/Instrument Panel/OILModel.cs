using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace Instrument_Panel
{
    class OILModel : ITimeObserver, IOILDoubleSubject, IRPMDoubleObserver, ISTARTDoubleObserver
    {
        private static readonly Lazy<OILModel> _instance = new Lazy<OILModel>(() => new OILModel());

        private ArrayList observers;
        private double oilp, rpm, start;


        private OILModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            RPMModel rm = RPMModel.Instance;
            rm.RegisterObserver(this);
            STARTModel sm = STARTModel.Instance;
            sm.RegisterObserver(this);
        }
        public static OILModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(IOILDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IOILDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double rpm)
        {
            foreach (IOILDoubleObserver observer in observers)
            {
                observer.OILUpdate(rpm);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public void RPMUpdate(double rpm)
        {
            this.rpm = rpm;
        }


        public void TimeUpdate(DateTime dt)
        {
            this.oilp = 50 + this.rpm / 900;
            if (this.start == 0) { oilp = 0; }
            NotifyObserver(oilp);
        }

        public void STARTUpdate(double start)
        {
            this.start = start;
        }



    }
}
