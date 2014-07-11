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
    class DigitalFUEL : IFUELDoubleObserver
    {
        const int XPOS = 20;
        const int YPOS = 10;
        const int XSIZE = 7;
        const int YSIZE = 7;
        const int STROKETHICKNESS = 1;


        private Digit digit1;
        private Digit digit2;
        private Digit digit3;

        Canvas canvas1;

        private int FUELTens, FUELOnes, FUELTenth;
        private bool blankleading;

        private IFUELDoubleSubject fuelModel;


        public DigitalFUEL(IFUELDoubleSubject fuelModel, Canvas canvas1)
        {

            this.canvas1 = canvas1;


            this.digit1 = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit2 = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit3 = new Digit(XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);

            this.fuelModel = fuelModel;
            fuelModel.RegisterObserver(this);
        }

        public DigitalFUEL(IFUELDoubleSubject fuelModel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;


            this.digit1 = new Digit(xpos + 0 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit2 = new Digit(xpos + 1 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit3 = new Digit(8 + xpos + 2 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.fuelModel = fuelModel;
            fuelModel.RegisterObserver(this);
        }

        public void FUELUpdate(double FUEL)
        {
            //This is the notification method called by the model.

            FUEL = FUEL % 1000;


            FUELTens = (int)(FUEL / 10);
            FUEL = FUEL % 10;

            FUELOnes = (int)(FUEL);
            FUEL = FUEL % 1;

            FUELTenth = (int)(FUEL / 0.10);

            Display();
        }

        public void Display()
        {
            if (blankleading)
            {

                digit1.blankDigit(canvas1);

                if (FUELTens != 0) { digit3.displayDigit(FUELTens, canvas1); }
                digit2.displayDigit(FUELOnes, canvas1);
                digit3.displayDigit(FUELTenth, canvas1);
            }
            else
            {
                digit1.displayDigit(FUELTens, canvas1);
                digit2.displayDigit(FUELOnes, canvas1);
                digit3.displayDigit(FUELTenth, canvas1);
            }

        }


    }
}
