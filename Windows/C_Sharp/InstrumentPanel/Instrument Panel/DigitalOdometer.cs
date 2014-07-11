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
    class DigitalOdometer : IODODoubleObserver, IFUELDoubleObserver
    {
        const int XPOS = 20;
        const int YPOS = 10;
        const int XSIZE = 7;
        const int YSIZE = 7;
        const int STROKETHICKNESS = 1;

        private Digit digit1;
        private Digit digit2;
        private Digit digit3;
        private Digit digit4;
        private Digit digit5;
        private Digit digit6;
        private Digit digit7;

        Canvas canvas1;

        private int ODOHundredsOfThousands, ODOTensOfThousands, ODOThousands, ODOHundreds, ODOTens, ODOOnes, ODOTenth;
        private bool blankleading;
        private IODODoubleSubject odomodel;
        private FUELModel fuelM;

        public DigitalOdometer(IODODoubleSubject odomodel, Canvas canvas1)
        {

            this.canvas1 = canvas1;

            this.digit1 = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit2 = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit3 = new Digit(XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit4 = new Digit(XPOS + 3 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit5 = new Digit(XPOS + 4 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit6 = new Digit(XPOS + 5 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit7 = new Digit(8 + XPOS + 6 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);

            this.odomodel = odomodel;
            odomodel.RegisterObserver(this);
            FUELModel fuelM = FUELModel.Instance;
            this.fuelM = fuelM;

        }

        public DigitalOdometer(IODODoubleSubject odomodel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;

            this.digit1 = new Digit(xpos + 0 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit2 = new Digit(xpos + 1 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit3 = new Digit(xpos + 2 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit4 = new Digit(xpos + 3 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit5 = new Digit(xpos + 4 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit6 = new Digit(xpos + 5 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit7 = new Digit(8 + xpos + 6 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.odomodel = odomodel;
            odomodel.RegisterObserver(this);
            FUELModel fuelM = FUELModel.Instance;
            this.fuelM = fuelM;
        }

        public void ODOUpdate(double odo)
        {
            //This is the notification method called by the model.
            
            fuelM.UpdateODOM(odo);

            ODOHundredsOfThousands = (int)(odo / 100000);
            odo = odo % 100000;

            ODOTensOfThousands = (int)(odo / 10000);
            odo = odo % 10000;

            ODOThousands = (int)(odo / 1000);
            odo = odo % 1000;

            ODOHundreds = (int)(odo / 100);
            odo = odo % 100;

            ODOTens = (int)(odo / 10);
            odo = odo % 10;

            ODOOnes = (int)(odo);
            odo = odo % 1;

            ODOTenth = (int)(odo /0.10);

            Display();
        }

        public void Display()
        {
            if (blankleading)
            {

                digit1.blankDigit(canvas1);
                digit2.blankDigit(canvas1);
                digit3.blankDigit(canvas1);
                digit4.blankDigit(canvas1);
                digit5.blankDigit(canvas1);

                if (ODOHundredsOfThousands != 0) { digit1.displayDigit(ODOHundredsOfThousands, canvas1); }
                if (!(ODOHundredsOfThousands == 0 & ODOTensOfThousands == 0)) { digit2.displayDigit(ODOTensOfThousands, canvas1); }
                if (!(ODOHundredsOfThousands == 0 & ODOTensOfThousands == 0 & ODOThousands == 0)) { digit3.displayDigit(ODOThousands, canvas1); }
                if (!(ODOHundredsOfThousands == 0 & ODOTensOfThousands == 0 & ODOThousands == 0 & ODOHundreds == 0)) { digit4.displayDigit(ODOHundreds, canvas1); }
                if (!(ODOHundredsOfThousands == 0 & ODOTensOfThousands == 0 & ODOThousands == 0 & ODOHundreds == 0 & ODOTens == 0)) { digit5.displayDigit(ODOTens, canvas1); }
                digit6.displayDigit(ODOOnes, canvas1);
                digit7.displayDigit(ODOTenth, canvas1);
            }
            else
            {
                digit1.displayDigit(ODOHundredsOfThousands, canvas1);
                digit2.displayDigit(ODOTensOfThousands, canvas1);
                digit3.displayDigit(ODOThousands, canvas1);
                digit4.displayDigit(ODOHundreds, canvas1);
                digit5.displayDigit(ODOTens, canvas1);
                digit6.displayDigit(ODOOnes, canvas1);
                digit7.displayDigit(ODOTenth, canvas1);
            }
        }
        public void FUELUpdate(double fuel)
        {
        }

    }
}
