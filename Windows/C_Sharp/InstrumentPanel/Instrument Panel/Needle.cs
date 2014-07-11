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
    class AnalogNeedle
    {
        

        //Sets the Center of the clock arms
        Point center;

        //Determines the size of the clock face
        double radius;

        //This value determines where the zero (start point) is.
        double adjust;

        //Allows line to not extend all the way to the center point
        double innerRadius;


        public AnalogNeedle(Point center, double radius, double adjust)
        {
            this.center = center;
            this.radius = radius;
            this.adjust = adjust;
            innerRadius = 0;
        }

        public AnalogNeedle(Point center, double radius, double adjust, double innerRadius)
            : this (center, radius, adjust)
        {
            this.innerRadius = innerRadius;
        }



        /// <summary>
        /// This will take any double from 0 to 99 and set the needle around the circle accordingly. 0 = 0 degree, 100 = 360 degree.
        /// </summary>
        /// <param name="level">The position of the gauge</param>
        /// <returns>The needle as a line object</returns>
        public Line setNeedle(double level)
        {
            if (level < 0)
            {
                level = 0;
            }
            level = level % 100;

            Point outerPoint = new Point();
            Point innerPoint = new Point();
            if (innerRadius <= 0)
            {
                innerPoint = center;
            }
            else
            {
                innerPoint = setPoint(level, innerRadius, center, adjust);
            }
            outerPoint = setPoint(level, radius, center, adjust);

            return setLine(innerPoint, outerPoint);
        }


        public void setRadiusOfInnerPoint(double innerRadius)
        {
            if (innerRadius < 0)
            {
                this.innerRadius = 0;
            }
            else if (innerRadius > this.radius)
            {
                this.innerRadius = this.radius;
            }
            else
            {
                this.innerRadius = innerRadius;
            }
        }

        public static Point setPoint(double level, double radius, Point center, double adjust)
        {
            Point point = new Point();

            double positionRadian = (level * 360) / 100 * Math.PI / 180;//Converst the level value to radians
            positionRadian = positionRadian - (adjust);//rotates Counter Clockwise
            point.X = radius * Math.Cos(positionRadian);
            point.Y = radius * Math.Sin(positionRadian);

            point.X = point.X + center.X;
            point.Y = point.Y + center.Y;

            return point;
        }

        public static Line setLine(Point c, Point e)
        {
            return setLine(c.X, c.Y, e.X, e.Y);
        }

        public static Line setLine(double cX, double cY, double eX, double eY)
        {
            Line temp = new Line();

            temp.X1 = cX;
            temp.Y1 = cY;
            temp.X2 = eX;
            temp.Y2 = eY;

            temp.Stroke = System.Windows.Media.Brushes.Red;
            temp.VerticalAlignment = VerticalAlignment.Center;
            temp.StrokeThickness = 3;

            return temp;
        }
    }
}
