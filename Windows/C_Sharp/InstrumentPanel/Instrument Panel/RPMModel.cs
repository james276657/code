using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace Instrument_Panel
{
    //This is the model portion of the MVC It serves up system time which may be adjusted by the controller.
    //Its implemented as a singleton to prevent multiple time models

    public sealed class RPMModel : IRPMDoubleSubject, ITimeObserver, ISTARTDoubleObserver
    {

        private static readonly Lazy<RPMModel> _instance = new Lazy<RPMModel>(() => new RPMModel());

        private ArrayList observers;

        private double gas, brake, rpm, jitter, start;

        private RPMModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            STARTModel sm = STARTModel.Instance;
            sm.RegisterObserver(this);
            jitter = 0;
        }
        public static RPMModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(IRPMDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IRPMDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double rpm)
        {
            foreach (IRPMDoubleObserver observer in observers)
            {
                observer.RPMUpdate(rpm);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }



        public void TimeUpdate(DateTime dt)
        {
            jitter = (double)RandomNumber(-10, 10);
            rpm = gas * 1000 + jitter;
            if (rpm < 600) { rpm = 600 + jitter; }
            if (this.start == 0) { rpm = 0; }
            NotifyObserver(rpm);
        }

        public void STARTUpdate(double start)
        {
            this.start = start;
        }

        public void UpdateGas(double gas)
        {
            this.gas = gas;

        }
        public void UpdateBrake(double brake)
        {
            this.brake = brake;

        }

        
    }
}
