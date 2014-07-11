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
    class DigitalMPH : IMPHDoubleObserver
    {
        const int XPOS = 20;
        const int YPOS = 10;
        const int XSIZE = 30;
        const int YSIZE = 25;
        const int STROKETHICKNESS = 3;

        private Digit digit1;
        private Digit digit2;
        private Digit digit3;

  
        Canvas canvas1;

        private int MPHHundreds, MPHTens, MPHOnes;
        private bool blankleading;

        private IMPHDoubleSubject mphmodel;



        public DigitalMPH(IMPHDoubleSubject mphmodel, Canvas canvas1)
        {

            this.canvas1 = canvas1;

            this.digit1 = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit2 = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit3 = new Digit(5 + XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
 
            this.mphmodel = mphmodel;
            mphmodel.RegisterObserver(this);
        }

        public DigitalMPH(IMPHDoubleSubject mphmodel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;
            this.digit1 = new Digit(xpos + 0 * (xsize + 6 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit2 = new Digit(xpos + 1 * (xsize + 6 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit3 = new Digit(xpos + 2 * (xsize + 6 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.mphmodel = mphmodel;
            mphmodel.RegisterObserver(this);
        }

        public void MPHUpdate(double mph)
        {
            //This is the notification method called by the model.

            MPHHundreds = (int)((mph) / 1000);
            MPHTens = (int)((mph - (double)MPHHundreds * 1000) / 100);
            MPHOnes = (int)((mph - (double)MPHHundreds * 1000 - (double)MPHTens * 100) / 10);

            Display();
        }

        public void Display()
        {
            if (blankleading)
            {

                digit1.blankDigit(canvas1);
                digit2.blankDigit(canvas1);

                if (MPHHundreds != 0) { digit1.displayDigit(MPHHundreds, canvas1); }
                if (!(MPHHundreds == 0 & MPHTens == 0)) { digit2.displayDigit(MPHTens, canvas1); }
                digit3.displayDigit(MPHOnes, canvas1);
            }
            else
            {
                digit1.displayDigit(MPHHundreds, canvas1);
                digit2.displayDigit(MPHTens, canvas1);
                digit3.displayDigit(MPHOnes, canvas1);
            }

        }

    }
}
