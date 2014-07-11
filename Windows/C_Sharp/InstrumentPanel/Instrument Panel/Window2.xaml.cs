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

namespace Instrument_Panel
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            this.Left = 250;
            this.Top = 125;
            createDigitalGauges();
        }
        private void createDigitalGauges()
        {
            MPHModel mphM = MPHModel.Instance;
            RPMModel rpmM = RPMModel.Instance;
            ODOModel odoM = ODOModel.Instance;
            TEMPModel tempM = TEMPModel.Instance;
            FUELModel fuelM = FUELModel.Instance;
            OILModel oilM = OILModel.Instance;
            TRIPModel tripM = TRIPModel.Instance;
            BATModel batM = BATModel.Instance;
            TimeModel timeM = TimeModel.Instance;
            GEARModel gearM = GEARModel.Instance;

            SolidColorBrush brush1 = new SolidColorBrush(Colors.Black);
            SolidColorBrush brush2 = new SolidColorBrush(Colors.LightSteelBlue);

            DigitalMPH digitalmph = new DigitalMPH(mphM, canvasMPH, brush1, 90, 75, 25, 20,2,true);
            DigitalRPM digitalrpm = new DigitalRPM(rpmM, canvasRPM, brush1, 9, 75, 25, 20, 2, true);
            DigitalFUEL digitalfuel = new DigitalFUEL(fuelM, canvasFuel, brush1, 20, 61, 12, 12, 1, false);
            DigitalOIL digitaloil = new DigitalOIL(oilM, canvasOil, brush1, 25, 12, 12, 12, 1, false);
            DigitalBAT digitalbat = new DigitalBAT(batM, canvasBat, brush1, 20, 12, 12, 12, 1, false);
            DigitalTEMP digitaltemp = new DigitalTEMP(tempM, canvasTemperature, brush1, 22, 61, 12, 12, 1, false);
            DigitalClock digitalclock = new DigitalClock(timeM, canvasTime, brush1, 0, 28, 12, 12, 1, false, true);
            DigitalOdometer odo = new DigitalOdometer(odoM, canvasODO, brush2, 20, 10, 7, 7, 1, false);
            DigitalTripometer trip = new DigitalTripometer(tripM, canvasTRIP, brush2, 20, 10, 7, 7, 1, false);
            
            

            //Note values are negative to trick the gauge into working backwards.


            GearIndicator gearLight = new GearIndicator();
            gearLight.addGearIndicator(gearPark, 0);
            gearLight.addGearIndicator(gearReverse, 1);
            gearLight.addGearIndicator(gearNeutral, 2);
            gearLight.addGearIndicator(gearDrive, 3);
            gearLight.addGearIndicator(gearThird, 4);
            gearLight.addGearIndicator(gearSecond, 5);
            gearM.RegisterObserver(gearLight);


//            gaugeMPH.update(15);
//            gaugeRPM.update(3.75);
//            gaugeFuel.update(80);
//            gaugeTemperature.update((-1 * 200));
//            gaugeOil.update(70);
//            gaugeBat.update((-1 * 67));


//              mphM.RegisterObserver(digitalmph);
//            rpmM.RegisterObserver(gaugeRPM);
//            tempM.RegisterObserver(gaugeTemperature);
//            fuelM.RegisterObserver(gaugeFuel);
//            oilM.RegisterObserver(gaugeOil);
//            batM.RegisterObserver(gaugeBat);
//            timeM.RegisterObserver(gaugeTimeHour);
//            timeM.RegisterObserver(gaugeTimeMinute);
//            gearM.RegisterObserver(gearLight);
        }

    }
}
