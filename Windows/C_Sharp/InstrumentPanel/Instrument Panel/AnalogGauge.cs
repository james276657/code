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
    class AnalogGauge : IMPHDoubleObserver, IRPMDoubleObserver, IOILDoubleObserver, ITEMPDoubleObserver, IBATDoubleObserver,
        IGEARDoubleObserver, IFUELDoubleObserver, ITimeObserver
    {
        AnalogNeedle needle;
        //The canvas the guage will be drawn on
        Canvas canvas;
        //The amount of circle to draw, 50 percent would be a half circle
        double percentOfCircle;
        //The default starting point is to the left(west). This rotates the starting point.
        double rotation;
        //The starting and ending values for the gauge, will effect how gauge displays input
        double lowEndInput, highEndInput;
        //The number of tick marks that get drawn on the guage
        ArrayList gaugeTicks;
        //Whether to draw lines between time marks or not;
        Boolean drawIntermidateMarks;
        //Whether to draw lables for tick marks;
        Boolean drawLabels;

        Line needleLine;
        Point center;
        double radius;

        Shape warningLight;
        double lowEndWarningRangeLowValue, lowEndWarningRangeHighValue, highEndWarningRangeLowValue, highEndWarningRangeHighValue;
        Color defaultColor, lowEndWarningColor, highEndWarningColor;
        Boolean warningLightEnabled;


        
        /// <summary>
        /// Core constructor that sets up the absolute basics needed for a guage.
        /// </summary>
        /// <param name="canvas">The canvas the gauge will be drawn on</param>
        /// <param name="percentOfCircle">How much of a circle to draw</param>
        /// <param name="rotation">Where to rotate the starting position to</param>
        public AnalogGauge(Canvas canvas, double percentOfCircle, double rotation)
        {
            warningLightEnabled = false;
            drawIntermidateMarks = false;
            gaugeTicks = new ArrayList();
            this.canvas = canvas;
            this.rotation = rotation + Math.PI;
            if (percentOfCircle > 100)
            {
                percentOfCircle = 100;
            }
            else if (percentOfCircle < 10)
            {
                percentOfCircle = 10;
            }
            this.percentOfCircle = percentOfCircle;

            //Initalize the startValue and endValue incase none are given
            lowEndInput = 0;
            highEndInput = 100;

            //Initalize the needle
            center = new Point((canvas.Width / 2), (canvas.Height / 2));
            if (canvas.Height > canvas.Width)
            {
                radius = canvas.Width / 2;
            }
            else
            {
                radius = canvas.Height / 2;
            }
            needle = new AnalogNeedle(center, radius, this.rotation);
        }


        /// <summary>
        /// Constructor that uses the core constructor but also allows low and high end values for the guage to be set
        /// </summary>
        /// <param name="canvas">The canvas the gauge will be drawn on</param>
        /// <param name="percentOfCircle">How much of a circle to draw</param>
        /// <param name="rotation">Where to rotate the starting position to</param>
        /// <param name="lowEndInput">The low end value of input given to the gauge</param>
        /// <param name="highEndInput">The high end value of input given to the gauge</param>
        public AnalogGauge(Canvas canvas, double percentOfCircle, double rotation, double lowEndInput, double highEndInput)
            : this(canvas, percentOfCircle, rotation)
        {
            //Start Value must be <= endValue
            if (lowEndInput > highEndInput)
            {
                lowEndInput = highEndInput - 1;
            }
            this.lowEndInput = lowEndInput;
            this.highEndInput = highEndInput;
        }


        /// <summary>
        /// Constructor that uses the core constructor but also allows low and high end values for the guage to be set
        /// </summary>
        /// <param name="canvas">The canvas the gauge will be drawn on</param>
        /// <param name="percentOfCircle">How much of a circle to draw</param>
        /// <param name="rotation">Where to rotate the starting position to</param>
        /// <param name="lowEndInput">The low end value of input given to the gauge</param>
        /// <param name="highEndInput">The high end value of input given to the gauge</param>
        /// <param name="ticks">The number of tick marks (lines) to be drawn evenly spaced on the gauge.</param>
        /// <param name="drawIntermidateMarks">Tells the gauge whether to add lines half way between lable tick marks</param>
        public AnalogGauge(Canvas canvas, double percentOfCircle, double rotation, double lowEndInput, double highEndInput, 
            int ticks, Boolean drawLabels, Boolean drawIntermidateMarks)
            : this(canvas, percentOfCircle, rotation, lowEndInput, highEndInput)
        {
            this.drawLabels = drawLabels;
            this.drawIntermidateMarks = drawIntermidateMarks;
            setNumberOfTickMarksOnGuage(ticks);
            
        }


        /// <summary>
        /// Constructor that uses the core constructor but also allows low and high end values for the guage to be set
        /// </summary>
        /// <param name="canvas">The canvas the gauge will be drawn on</param>
        /// <param name="percentOfCircle">How much of a circle to draw</param>
        /// <param name="rotation">Where to rotate the starting position to</param>
        /// <param name="lowEndInput">The low end value of input given to the gauge</param>
        /// <param name="highEndInput">The high end value of input given to the gauge</param>
        /// <param name="ticks">The number of tick marks (lines) to be drawn evenly spaced on the gauge.</param>
        /// <param name="drawIntermidateMarks">Tells the gauge whether to add lines half way between lable tick marks</param>
        public AnalogGauge(Canvas canvas, double percentOfCircle, double rotation, double lowEndInput, double highEndInput, 
            int ticks, Boolean drawLabels, Boolean drawIntermidateMarks, Shape warningLight, double lowEndWarningRangeLowValue, double lowEndWarningRangeHighValue, 
            double highEndWarningRangeLowValue, double highEndWarningRangeHighValue, Color defaultColor, Color lowEndWarningColor, Color highEndWarningColor)
            : this(canvas, percentOfCircle, rotation, lowEndInput, highEndInput, ticks, drawLabels, drawIntermidateMarks)
        {
            warningLightEnabled = true;
            this.warningLight = warningLight;

            this.lowEndWarningRangeLowValue = lowEndWarningRangeLowValue;
            this.lowEndWarningRangeHighValue = lowEndWarningRangeHighValue;

            this.highEndWarningRangeLowValue = highEndWarningRangeLowValue;
            this.highEndWarningRangeHighValue = highEndWarningRangeHighValue;

            this.defaultColor = defaultColor;
            this.lowEndWarningColor = lowEndWarningColor;
            this.highEndWarningColor = highEndWarningColor;

        }


        /// <summary>
        /// Sets the number of tick marks to be draw on the gauge. If the value hasn't changed, they will not be redrawn.
        /// </summary>
        /// <param name="ticks">Int greater then or equal to 0 with the number of ticks to be drawn. Any value given that is less then 0 will be set to 0.</param>
        public void setNumberOfTickMarksOnGuage(int ticks)
        {
            if (gaugeTicks.Count != ticks)
            {
                if (ticks < 0)
                {
                    ticks = 0;
                }
                updateTicksOnGauge(ticks);
            }
            else
            {
                //This number of ticks isn't changing so no need to redraw them
            }
        }

        /// <summary>
        /// Accepts an intger of zero or higher and draws that many tick marks on the gauge, plus and additional
        /// mark at the lowEndInput value.
        /// </summary>
        /// <param name="ticks">The number of tick marks to be drawn, plus an additional mark at the lowEndInput value.</param>
        private void updateTicksOnGauge(int ticks)
        {
            double percentageFromCenter = 0.87;
            double intermediateMarksFromCenter = 0.91;
            double textSpacer = 0.85;
            double textValueIncrement = ((highEndInput - lowEndInput) / ticks);
            double tickNumber = 0;
            Point insidePoint = new Point();
            Point outsidePoint = new Point();
            Point textLocation = new Point();

            Boolean isFirstLabel = true;

            //Remove the old tick marks from the canvas
            if (gaugeTicks.Count > 0)
            {
                foreach (Line i in gaugeTicks)
                {
                    canvas.Children.Remove(i);
                }
            }

            //create new tick marks
            for (int i = 0; i <= ticks; i++)
            {
                //Generate all the points that will be needed for the tick mark and its label
                outsidePoint = AnalogNeedle.setPoint( convertInputRangeToNeedleRange(i, 0, ticks), radius, center, rotation);
                insidePoint = AnalogNeedle.setPoint( convertInputRangeToNeedleRange(i, 0, ticks), (radius * percentageFromCenter), center, rotation);
                textLocation = AnalogNeedle.setPoint(convertInputRangeToNeedleRange(i, 0, ticks), (radius * percentageFromCenter * textSpacer), center, rotation);
                //Generate the line of the tick mark
                gaugeTicks.Add(AnalogNeedle.setLine(insidePoint, outsidePoint));


                //Generate labels for the tick marks based on the defined input range.
                if (drawLabels == true)
                {
                    TextBlock num = new TextBlock();
                    tickNumber = (lowEndInput + (i * textValueIncrement));
                    num.Text = "" + ((int)tickNumber);
                    num.Margin = new Thickness((textLocation.X - 7), (textLocation.Y - 9), 0, 0);


                    if (isFirstLabel && percentOfCircle > 99)
                    {
                        //on a full circle don't add the first label
                        isFirstLabel = false;
                    }
                    else
                    {
                        //add normally
                        canvas.Children.Add(num);
                    }
 
                }
                

                //If turned on draw Intermidate Marks
                if (this.drawIntermidateMarks == true)
                {
                    

                    float j = (float)(i + 0.5);
                    if (j < ticks)
                    {
                        outsidePoint = AnalogNeedle.setPoint(convertInputRangeToNeedleRange(j, 0, ticks), radius, center, rotation);
                        insidePoint = AnalogNeedle.setPoint(convertInputRangeToNeedleRange(j, 0, ticks), (radius * intermediateMarksFromCenter), center, rotation);
                        gaugeTicks.Add(AnalogNeedle.setLine(insidePoint, outsidePoint));
                    }
                    
                }

            }

            //add new tick marks to the canvas
            foreach(Line i in gaugeTicks)
            {
                canvas.Children.Add(i);
            }
        }

        /// <summary>
        /// Converts the input given from the input range defined by the user to the input range (0-99)
        /// that is used by the needle. If the value exceeds the highEndInput, then it will be capped at
        /// highEndInput + 1.
        /// </summary>
        /// <param name="input"> Input value that is: (startValue) lessThenEqualTo (inut) lessThenEqualTo (endValue) </param>
        /// <returns>A double from 0 to 100</returns>
        private double convertInputRangeToNeedleRange(double input, double lowEndInput, double highEndInput)
        {
            //If the input exceeds the defined input of the gauge, cap it.
            if (input > (highEndInput))
            {
                input = highEndInput;
            }

            double convertedValue = 0;
            double newHighEnd = 0;

            newHighEnd = (highEndInput + (-1 * lowEndInput));
            convertedValue = ((input - lowEndInput) / newHighEnd);
            convertedValue = convertedValue * percentOfCircle;

            return convertedValue;
        }

        /// <summary>
        /// Returns the center point used for the gauge on the canvas
        /// </summary>
        /// <returns>the center point used for the gauge</returns>
        public Point getCenter()
        {
            return center;
        }

        /// <summary>
        /// Returns the canvas used by this gauage
        /// </summary>
        /// <returns>the canvas used by this gauage</returns>
        public Canvas getCanvas()
        {
            return canvas;
        }


        /// <summary>
        /// This takes the input for the gauge and redraws it accordingly.
        /// </summary>
        /// <param name="input">A double value between lowEndInput and highEndInput inclusive.
        /// If not defined these are 0 to 100.</param>
        public void update(double input)
        {
            double needleInput = convertInputRangeToNeedleRange(input, lowEndInput, highEndInput);

            if (needleLine != null)
            {
                canvas.Children.Remove(needleLine);
            }

            needleLine = needle.setNeedle(needleInput);

            canvas.Children.Add(needleLine);

            if (warningLightEnabled)
            {
                //Light up warning lights, or not
                if (lowEndWarningRangeLowValue <= input && input <= lowEndWarningRangeHighValue)
                {
                    warningLight.Fill = new SolidColorBrush(lowEndWarningColor);
                }
                else if (highEndWarningRangeLowValue <= input && input <= highEndWarningRangeHighValue)
                {
                    warningLight.Fill = new SolidColorBrush(highEndWarningColor);
                }
                else
                {
                    warningLight.Fill = new SolidColorBrush(defaultColor);
                }
            }
            
        }


        void IMPHDoubleObserver.MPHUpdate(double mph)
        {
            update((mph/10));
        }

        void IRPMDoubleObserver.RPMUpdate(double rpm)
        {
            update(rpm/1000);
        }



        void IFUELDoubleObserver.FUELUpdate(double o)
        {
            this.update(o);
        }



        void IGEARDoubleObserver.GEARUpdate(double o)
        {
            throw new NotImplementedException();
        }



        void IBATDoubleObserver.BATUpdate(double o)
        {
            this.update((-1 * o));
        }



        void ITEMPDoubleObserver.TEMPUpdate(double o)
        {
            this.update((-1 * o));
        }



        void IOILDoubleObserver.OILUpdate(double o)
        {
            this.update(o);
        }



        void ITimeObserver.TimeUpdate(DateTime o)
        {
            if (gaugeTicks.Count < 30)
            {
                //Assume this is the hours
                this.update((o.Hour % 12));
            }
            else
            {
                //Assume this is the minutes
                this.update(o.Minute);
            }
        }

    }
}
