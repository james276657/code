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
    class GearIndicator : IGEARDoubleObserver
    {
        Canvas park, reverse, neutral, drive, third, second;

        public GearIndicator()
        {
            park = null;
            reverse = null;
            neutral = null;
            drive = null;
            third = null;
            second = null;
        }

        public void addGearIndicator(Canvas gearLight, int gearNumber)
        {
            switch (gearNumber)
            {
                case 0: park = gearLight; break;
                case 1: reverse = gearLight; break;
                case 2: neutral = gearLight; break;
                case 3: drive = gearLight; break;
                case 4: third = gearLight; break;
                case 5: second = gearLight; break;
                default: park = gearLight; break; 
            }
        }


        void IGEARDoubleObserver.GEARUpdate(double o)
        {
            int temp = (int)o;

            park.Opacity = 0;
            reverse.Opacity = 0;
            neutral.Opacity = 0;
            drive.Opacity = 0;
            third.Opacity = 0;
            second.Opacity = 0;

            switch (temp)
            {
                case 0: park.Opacity = 255; break;
                case 1: reverse.Opacity = 255; break;
                case 2: neutral.Opacity = 255; break;
                case 3: drive.Opacity = 255; break;
                case 4: third.Opacity = 255; break;
                case 5: second.Opacity = 255; break;
                default: park.Opacity = 255; break;
            }
        }
    }
}
