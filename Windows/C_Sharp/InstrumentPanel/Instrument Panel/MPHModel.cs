using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Instrument_Panel
{
    //This is the model portion of the MVC It serves up system time which may be adjusted by the controller.
    //Its implemented as a singleton to prevent multiple time models

    public sealed class MPHModel : IMPHDoubleSubject, ITimeObserver, IRPMDoubleObserver, IGEARDoubleObserver
    {

        private static readonly Lazy<MPHModel> _instance = new Lazy<MPHModel>(() => new MPHModel());

        private ArrayList observers;

        private double gas, brake, rpm, jitter, mph, lastrpm, gradientrpm, gradientadd, rpmdiff, gear;
        private int uptimecount;


        private MPHModel()
        {
            observers = new ArrayList();

            TimeModel tm = TimeModel.Instance;
            tm.RegisterObserver(this);
            RPMModel rm = RPMModel.Instance;
            rm.RegisterObserver(this);
            GEARModel gm = GEARModel.Instance;
            gm.RegisterObserver(this);
            jitter = 0;
            mph = 0;
            this.gear = 0;

  
        }
        public static MPHModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void RegisterObserver(IMPHDoubleObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IMPHDoubleObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(double mph)
        {
            foreach (IMPHDoubleObserver observer in observers)
            {
                observer.MPHUpdate(mph);
            }
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }



        public void TimeUpdate(DateTime dt)
        {

            jitter = (double)RandomNumber(-1, 1);
            if (uptimecount > 0)
            {
                uptimecount--;
                this.gradientrpm = this.gradientrpm + gradientadd;
            }
            mph = this.gradientrpm / 7 + jitter;
            if (this.gradientrpm < 700) { mph = 0; }
            if (this.gear == 0 | this.gear == 1 | this.gear == 2) { mph = 0; }
            NotifyObserver(mph);
        }

        public void RPMUpdate(double rpm)
        {
            this.rpm = rpm;
            if (this.rpm > 700)
            {
                if (this.rpm > (lastrpm + 10))
                {
                    this.rpmdiff = this.rpm - this.lastrpm;
                    this.uptimecount = (int)this.rpmdiff / 100;
                    this.gradientrpm = this.lastrpm;
                    this.gradientadd = this.uptimecount;
                    this.lastrpm = this.rpm;
                }
                if (this.rpm < (lastrpm - 10))
                {
                    this.rpmdiff = this.rpm - this.lastrpm;
                    this.uptimecount = (int)this.rpmdiff / 100;
                    this.gradientrpm = this.lastrpm;
                    this.gradientadd = this.uptimecount;
                    this.lastrpm = this.rpm;
                }
            }
            else
            {
                this.gradientrpm = this.rpm;
                this.lastrpm = this.rpm;
                this.uptimecount = 0;
                this.gradientadd = 0;
                this.rpmdiff = 0;
            }
        }
        public void GasUpdate(double gas)
        {
            this.gas = gas;

        }
        public void BrakeUpdate(double brake)
        {
            this.brake = brake;
        }

        public void GEARUpdate(double gear)
        {
            this.gear = gear;
        }
    }
}
