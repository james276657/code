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
    class DigitalRPM : IRPMDoubleObserver

    {
        const int XPOS = 20;
        const int YPOS = 10;
        const int XSIZE = 30;
        const int YSIZE = 25;
        const int STROKETHICKNESS = 3;

        private Digit digit1;
        private Digit digit2;
        private Digit digit3;
        private Digit digit4;

        Canvas canvas1;

        private int RPMThousands, RPMHundreds, RPMTens, RPMOnes;
        private bool blankleading;

        private IRPMDoubleSubject rpmmodel;


        public DigitalRPM(IRPMDoubleSubject rpmmodel, Canvas canvas1)
        {

            this.canvas1 = canvas1;

            this.digit1 = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit2 = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit3 = new Digit(5 + XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit4 = new Digit(5 + XPOS + 3 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);

            this.rpmmodel = rpmmodel;
            rpmmodel.RegisterObserver(this);
        }

        public DigitalRPM(IRPMDoubleSubject rpmmodel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;
            this.digit1 = new Digit(xpos + 0 * (xsize + 6 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit2 = new Digit(xpos + 1 * (xsize + 6 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit3 = new Digit(xpos + 2 * (xsize + 6 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit4 = new Digit(xpos + 3 * (xsize + 6 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.rpmmodel = rpmmodel;
            rpmmodel.RegisterObserver(this);
        }


        public void RPMUpdate(double rpm)
        {
            //This is the notification method called by the model.

            RPMThousands = (int)rpm / 1000;
            RPMHundreds = (int)((rpm - (double)RPMThousands* 1000) / 100);
            RPMTens = (int)((rpm - (double)RPMThousands * 1000 - (double)RPMHundreds * 100) / 10);
            RPMOnes = (int)((rpm - (double)RPMThousands * 1000 - (double)RPMHundreds * 100 - (double) RPMTens* 10)/1);
 
            Display();
        }

        public void Display()
        {
            if (blankleading)
            {

                digit1.blankDigit(canvas1);
                digit2.blankDigit(canvas1);
                digit3.blankDigit(canvas1);

                if (RPMThousands != 0) { digit1.displayDigit(RPMThousands, canvas1); }
                if (!(RPMThousands == 0 & RPMHundreds == 0)) { digit2.displayDigit(RPMHundreds, canvas1); }
                if (!(RPMThousands == 0 & RPMHundreds == 0 & RPMTens == 0)) { digit3.displayDigit(RPMTens, canvas1); }
                digit4.displayDigit(RPMOnes, canvas1);
            }
            else
            {
                digit1.displayDigit(RPMThousands, canvas1);
                digit2.displayDigit(RPMHundreds, canvas1);
                digit3.displayDigit(RPMTens, canvas1);
                digit4.displayDigit(RPMOnes, canvas1);
            }

        }

    }
}
