using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Instrument_Panel
{
    class DigitalClock : ITimeObserver
    //DigitalClock generates the Digital clock view by instantiating 6 digits and updating them when notified by the model
    {
        const int XPOS = 20;
        const int YPOS = 10;
        const int XSIZE = 30;
        const int YSIZE = 25;
        const int STROKETHICKNESS = 3;

        private Digit digithl;
        private Digit digithr;
        private Digit digitml;
        private Digit digitmr;
        private Digit digitsl;
        private Digit digitsr;

        Canvas canvas1;

        private int HourTens, HourOnes, MinuteTens, MinuteOnes, SecondTens, SecondOnes;
        private bool blankleading, fourdigit;
        
        private ITimeSubject timemodel;
        

        public DigitalClock(ITimeSubject timemodel, Canvas canvas1)
        {

            this.canvas1 = canvas1;

            this.digithl = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digithr = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digitml = new Digit(5 + XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digitmr = new Digit(5 + XPOS + 3 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digitsl = new Digit(10 + XPOS + 4 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digitsr = new Digit(10 + XPOS + 5 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);

            this.timemodel = timemodel;
            timemodel.RegisterObserver(this);
        }

        public DigitalClock(ITimeSubject timemodel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading, bool fourdigit)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;
            this.fourdigit = fourdigit;

            this.digithl = new Digit(xpos + 0 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digithr = new Digit(xpos + 1 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digitml = new Digit(8 + xpos + 2 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digitmr = new Digit(8 + xpos + 3 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digitsl = new Digit(10 + xpos + 4 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digitsr = new Digit(10 + xpos + 5 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.timemodel = timemodel;
            timemodel.RegisterObserver(this);
        }

        public void TimeUpdate(DateTime currentTime)
        {
            //This is the notification method called by the model.
            //We're doing a twelve hour clock.  System time is 24 hour based
            HourTens = currentTime.Hour;
            if (HourTens > 12) { HourTens = HourTens - 12; }
            if (HourTens == 0) { HourTens = 12; }
            HourOnes = HourTens % 10;
            HourTens = HourTens / 10;

            MinuteOnes = (int)(currentTime.Minute) % 10;
            MinuteTens = (int)(currentTime.Minute) / 10;

            SecondOnes = (int)(currentTime.Second) % 10;
            SecondTens = (int)(currentTime.Second) / 10;
            
            Display();
        }

        public void Display()
        {
            digithl.displayDigit(HourTens, canvas1);
            digithr.displayDigit(HourOnes, canvas1);
            digitml.displayDigit(MinuteTens, canvas1);
            digitmr.displayDigit(MinuteOnes, canvas1);

            if(!(fourdigit))
            {
                digitsl.displayDigit(SecondTens, canvas1);
                digitsr.displayDigit(SecondOnes, canvas1);
            }

        }

    }
}
