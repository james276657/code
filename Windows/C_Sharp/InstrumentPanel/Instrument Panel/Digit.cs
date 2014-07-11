using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace Instrument_Panel
{
    public class Digit
    {
        //Digit class for GUI_Clock  J. Brooks  5/10/11
        //Draws a seven segment digit on a canvas with dimension parameters
        //Display adds individual red lines after blanking depending on input number
        //Called by DigitalClock

        
        Line[] lines = new Line[7];

        //neumonic for lines TH = Top Horizontal  BLV = Bottom Left Vertical
        enum lineindexes : int { TH, MH, BH, TLV, TRV, BLV, BRV };

        int Xpos, Ypos, Xsize, Ysize, Strokethickness, i;

        public Digit(int xpos, int ypos, int xsize, int ysize, int strokethickness)
        {

            //Constructor creates the lines for a seven segment digit display

            //Save the inputs
            this.Xpos = xpos;
            this.Ypos = ypos;
            this.Xsize = xsize;
            this.Ysize = ysize;
            this.Strokethickness = strokethickness;

            //Set up the digit line positions
            for (i = 0; i < 7; i++)
            {
                //Common for all lines
                lines[i] = new Line();
                lines[i].Stroke = System.Windows.Media.Brushes.Red;  //Runtime change color?
                lines[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lines[i].VerticalAlignment = VerticalAlignment.Center;
                lines[i].StrokeThickness = this.Strokethickness;

                //Specific segment line dimensions
                switch (i)
                {
                    case 0:  //Top Horizontal
                        lines[i].X1 = this.Xpos;
                        lines[i].X2 = lines[i].X1 + this.Xsize;
                        lines[i].Y1 = this.Ypos;
                        lines[i].Y2 = lines[i].Y1;
                        break;

                    case 1:  //Middle Horizontal
                        lines[i].X1 = this.Xpos;
                        lines[i].X2 = lines[i].X1 + this.Xsize;
                        lines[i].Y1 = this.Ypos + this.Strokethickness + this.Ysize + this.Strokethickness;
                        lines[i].Y2 = lines[i].Y1;
                        break;

                    case 2:  //Bottom Horizontal
                        lines[i].X1 = this.Xpos;
                        lines[i].X2 = lines[i].X1 + this.Xsize;
                        lines[i].Y1 = this.Ypos + 2 * (this.Strokethickness + this.Ysize + this.Strokethickness);
                        lines[i].Y2 = lines[i].Y1;
                        break;

                    case 3:  //Top left vertical
                        lines[i].X1 = this.Xpos - this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + this.Strokethickness;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    case 4:  //Top right vertical
                        lines[i].X1 = this.Xpos + this.Xsize + this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + this.Strokethickness;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    case 5:  //Bottom left vertical
                        lines[i].X1 = this.Xpos - this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + 3 * this.Strokethickness + this.Ysize;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    case 6:  //Bottom right vertical
                        lines[i].X1 = this.Xpos + this.Xsize + this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + 3 * this.Strokethickness + this.Ysize;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    default:
                        break;


                }
            }
        }

        public Digit(int xpos, int ypos, int xsize, int ysize, int strokethickness, Brush brush1)
        {

            //Constructor creates the lines for a seven segment digit display with changeable color

            //Save the inputs
            this.Xpos = xpos;
            this.Ypos = ypos;
            this.Xsize = xsize;
            this.Ysize = ysize;
            this.Strokethickness = strokethickness;

            //Set up the digit line positions
            for (i = 0; i < 7; i++)
            {
                //Common for all lines
                lines[i] = new Line();
                lines[i].Stroke = brush1;  //Runtime change color?
                lines[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lines[i].VerticalAlignment = VerticalAlignment.Center;
                lines[i].StrokeThickness = this.Strokethickness;

                //Specific segment line dimensions
                switch (i)
                {
                    case 0:  //Top Horizontal
                        lines[i].X1 = this.Xpos;
                        lines[i].X2 = lines[i].X1 + this.Xsize;
                        lines[i].Y1 = this.Ypos;
                        lines[i].Y2 = lines[i].Y1;
                        break;

                    case 1:  //Middle Horizontal
                        lines[i].X1 = this.Xpos;
                        lines[i].X2 = lines[i].X1 + this.Xsize;
                        lines[i].Y1 = this.Ypos + this.Strokethickness + this.Ysize + this.Strokethickness;
                        lines[i].Y2 = lines[i].Y1;
                        break;

                    case 2:  //Bottom Horizontal
                        lines[i].X1 = this.Xpos;
                        lines[i].X2 = lines[i].X1 + this.Xsize;
                        lines[i].Y1 = this.Ypos + 2 * (this.Strokethickness + this.Ysize + this.Strokethickness);
                        lines[i].Y2 = lines[i].Y1;
                        break;

                    case 3:  //Top left vertical
                        lines[i].X1 = this.Xpos - this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + this.Strokethickness;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    case 4:  //Top right vertical
                        lines[i].X1 = this.Xpos + this.Xsize + this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + this.Strokethickness;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    case 5:  //Bottom left vertical
                        lines[i].X1 = this.Xpos - this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + 3 * this.Strokethickness + this.Ysize;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    case 6:  //Bottom right vertical
                        lines[i].X1 = this.Xpos + this.Xsize + this.Strokethickness;
                        lines[i].X2 = lines[i].X1;
                        lines[i].Y1 = this.Ypos + 3 * this.Strokethickness + this.Ysize;
                        lines[i].Y2 = lines[i].Y1 + this.Ysize;
                        break;

                    default:
                        break;


                }
            }
        }

        public void blankDigit(Canvas canvas)
        {
            //Turn off the digit lines one at a time
            for (i = 0; i < 7; i++)
            {
                canvas.Children.Remove(lines[i]);
            }
        }
        public void displayDigit(int number, Canvas canvas)
        {
            //Turn it off first
            blankDigit(canvas);

            switch (number)
            {
                case 0:

                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.BH]);
                    canvas.Children.Add(lines[(int)lineindexes.TLV]);
                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BLV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 1:

                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 2:

                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.BH]);
                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BLV]);
                    break;

                case 3:

                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.BH]);
                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 4:

                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.TLV]);
                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 5:

                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.BH]);
                    canvas.Children.Add(lines[(int)lineindexes.TLV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 6:

                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.BH]);
                    canvas.Children.Add(lines[(int)lineindexes.TLV]);
                    canvas.Children.Add(lines[(int)lineindexes.BLV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 7:

                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 8:

                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.BH]);
                    canvas.Children.Add(lines[(int)lineindexes.TLV]);
                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BLV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                case 9:

                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.TLV]);
                    canvas.Children.Add(lines[(int)lineindexes.TRV]);
                    canvas.Children.Add(lines[(int)lineindexes.BRV]);
                    break;

                //Show E for error if number>9 | number<0
                default:
                    canvas.Children.Add(lines[(int)lineindexes.TH]);
                    canvas.Children.Add(lines[(int)lineindexes.MH]);
                    canvas.Children.Add(lines[(int)lineindexes.BH]);
                    canvas.Children.Add(lines[(int)lineindexes.TLV]);
                    canvas.Children.Add(lines[(int)lineindexes.BLV]);
                    break;
            }
        }
    }
}
