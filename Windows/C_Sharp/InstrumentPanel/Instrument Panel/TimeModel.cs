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

    public sealed class TimeModel : ITimeSubject
	{

        private static readonly Lazy<TimeModel> _instance = new Lazy<TimeModel>(() => new TimeModel());

        private ArrayList observers;
        private DateTime startTime, currentTime, adjustTime;
        private int Adjust_Hour, Adjust_Minute, Adjust_Second;

        private TimeModel()
		{
            observers = new ArrayList();
            this.startTime = DateTime.Now;

            //Initialize adjustment to 0 for system time display start up.
            this.Adjust_Hour = 0;
            this.Adjust_Minute = 0;
            this.Adjust_Second = 0;

            //Get a one tenth second tick for the model
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += new EventHandler(dispatcherTimer_Tick);
            timer.Start();
 
        }
        public static TimeModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }   

        public void RegisterObserver(ITimeObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(ITimeObserver o)
        {
            int i = observers.IndexOf(o);
            if (i >= 0)
            {
                observers.Remove(o);
            }
        }

        public void NotifyObserver(DateTime currentTime)
        {
            //Adjust the time before handing it out
            currentTime = currentTime + new TimeSpan(this.Adjust_Hour, this.Adjust_Minute, this.Adjust_Second);

            foreach (ITimeObserver observer in observers)
            {
                observer.TimeUpdate(currentTime);
            }
        }
        public void UpdateTime()
        {

            currentTime = DateTime.Now;
            TimeSpan elapsed = currentTime - startTime;

            if (elapsed.TotalSeconds >= .1)
            {
                startTime = currentTime;
                NotifyObserver(currentTime);

            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Update and notify each second
            UpdateTime();
        }

        public void AdjustTime(int hour, int minute, int second)
        {

            adjustTime = DateTime.Now;
            this.Adjust_Hour = -adjustTime.Hour + hour;
            this.Adjust_Minute = -adjustTime.Minute + minute;
            this.Adjust_Second = -adjustTime.Second + second;

        }
    }
}
