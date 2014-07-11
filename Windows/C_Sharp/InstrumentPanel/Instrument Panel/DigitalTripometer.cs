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
    class DigitalTripometer : ITRIPDoubleObserver
    {
        const int XPOS = 20;
        const int YPOS = 10;
        const int XSIZE = 7;
        const int YSIZE = 7;
        const int STROKETHICKNESS = 1;


        private Digit digit4;
        private Digit digit5;
        private Digit digit6;
        private Digit digit7;

        Canvas canvas1;

        private int ODOHundreds, ODOTens, ODOOnes, ODOTenth;
        private bool blankleading;

        private ITRIPDoubleSubject odomodel;


        public DigitalTripometer(ITRIPDoubleSubject tripModel, Canvas canvas1)
        {

            this.canvas1 = canvas1;


            this.digit4 = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit5 = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit6 = new Digit(XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit7 = new Digit(8 + XPOS + 3 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);

            this.odomodel = tripModel;
            tripModel.RegisterObserver(this);
        }

        public DigitalTripometer(ITRIPDoubleSubject tripModel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;


            this.digit4 = new Digit(xpos + 0 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit5 = new Digit(xpos + 1 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit6 = new Digit(xpos + 2 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit7 = new Digit(8 + xpos + 3 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.odomodel = tripModel;
            tripModel.RegisterObserver(this);
        }

        public void TRIPUpdate(double odo)
        {
            //This is the notification method called by the model.

            odo = odo % 1000;

            ODOHundreds = (int)(odo / 100);
            odo = odo % 100;

            ODOTens = (int)(odo / 10);
            odo = odo % 10;

            ODOOnes = (int)(odo);
            odo = odo % 1;

            ODOTenth = (int)(odo / 0.10);

            Display();
        }

        public void Display()
        {
            if (blankleading)
            {

                digit4.blankDigit(canvas1);
                digit5.blankDigit(canvas1);

                if (ODOHundreds != 0) { digit4.displayDigit(ODOHundreds, canvas1); }
                if (!(ODOHundreds == 0 & ODOTens == 0)) { digit5.displayDigit(ODOHundreds, canvas1); }
                digit6.displayDigit(ODOOnes, canvas1);
                digit7.displayDigit(ODOTenth, canvas1);
            }
            else
            {
                digit4.displayDigit(ODOHundreds, canvas1);
                digit5.displayDigit(ODOTens, canvas1);
                digit6.displayDigit(ODOOnes, canvas1);
                digit7.displayDigit(ODOTenth, canvas1);
            }

        }


    }
}
