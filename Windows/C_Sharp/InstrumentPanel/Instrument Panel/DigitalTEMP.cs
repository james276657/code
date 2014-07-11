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
    class DigitalTEMP : ITEMPDoubleObserver
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

        private int TEMPHundreds, TEMPTens, TEMPOnes;
        private bool blankleading;

        private ITEMPDoubleSubject TEMPModel;


        public DigitalTEMP(ITEMPDoubleSubject TEMPModel, Canvas canvas1)
        {

            this.canvas1 = canvas1;


            this.digit1 = new Digit(XPOS + 0 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit2 = new Digit(XPOS + 1 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);
            this.digit3 = new Digit(XPOS + 2 * (XSIZE + 8 * STROKETHICKNESS), YPOS, XSIZE, YSIZE, STROKETHICKNESS);

            this.TEMPModel = TEMPModel;
            TEMPModel.RegisterObserver(this);
        }

        public DigitalTEMP(ITEMPDoubleSubject TEMPModel, Canvas canvas1, Brush brush1, int xpos, int ypos, int xsize, int ysize, int strokethickness, bool blankleading)
        {

            this.canvas1 = canvas1;
            this.blankleading = blankleading;


            this.digit1 = new Digit(xpos + 0 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit2 = new Digit(xpos + 1 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);
            this.digit3 = new Digit(xpos + 2 * (xsize + 8 * strokethickness), ypos, xsize, ysize, strokethickness, brush1);

            this.TEMPModel = TEMPModel;
            TEMPModel.RegisterObserver(this);
        }

        public void TEMPUpdate(double TEMP)
        {
            //This is the notification method called by the model.

            TEMP = TEMP % 1000;


            TEMPHundreds = (int)(TEMP / 100);
            TEMP = TEMP % 100;

            TEMPTens = (int)(TEMP / 10);
            TEMP = TEMP % 10;

            TEMPOnes = (int)(TEMP);
            TEMP = TEMP % 1;


            Display();
        }

        public void Display()
        {
            if (blankleading)
            {

                digit1.blankDigit(canvas1);

                if (TEMPHundreds != 0) { digit3.displayDigit(TEMPHundreds, canvas1); }
                digit2.displayDigit(TEMPTens, canvas1);
                digit3.displayDigit(TEMPOnes, canvas1);
            }
            else
            {
                digit1.displayDigit(TEMPHundreds, canvas1);
                digit2.displayDigit(TEMPTens, canvas1);
                digit3.displayDigit(TEMPOnes, canvas1);
            }

        }


    }
}
