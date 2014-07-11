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
    class DigitalOIL : IOILDoubleObserver
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

        private int OILHundreds, OILTens, OILOnes;
        private bool blankleading;

        private IOILDoubleSubject OILModel;


        public DigitalOIL(IOILDoubleSubject OILModel, Canvas canvas1)
        {

            this.canvas1 = canvas1;


            this.digit1 = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit2 = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit3 = new Digit(XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);

            this.OILModel = OILModel;
            OILModel.RegisterObserver(this);
        }

        public DigitalOIL(IOILDoubleSubject OILModel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;


            this.digit1 = new Digit(xpos + 0 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit2 = new Digit(xpos + 1 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit3 = new Digit(xpos + 2 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.OILModel = OILModel;
            OILModel.RegisterObserver(this);
        }

        public void OILUpdate(double OIL)
        {
            //This is the notification method called by the model.

            OIL = OIL % 1000;


            OILHundreds = (int)(OIL / 100);
            OIL = OIL % 100;

            OILTens = (int)(OIL / 10);
            OIL = OIL % 10;

            OILOnes = (int)(OIL);
            OIL = OIL % 1;


            Display();
        }

        public void Display()
        {
            if (blankleading)
            {

                digit1.blankDigit(canvas1);

                if (OILHundreds != 0) { digit3.displayDigit(OILHundreds, canvas1); }
                digit2.displayDigit(OILTens, canvas1);
                digit3.displayDigit(OILOnes, canvas1);
            }
            else
            {
                digit1.displayDigit(OILHundreds, canvas1);
                digit2.displayDigit(OILTens, canvas1);
                digit3.displayDigit(OILOnes, canvas1);
            }

        }


    }
}
