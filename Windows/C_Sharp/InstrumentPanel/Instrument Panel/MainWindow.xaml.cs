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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Left = 250;
            this.Top = 125;

            Window1 dcwindow = new Window1();
            dcwindow.Show();

            Window2 dwindow = new Window2();
            dwindow.Show();

            createAnalogGauges();
            
        }

        private void createAnalogGauges()
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


            AnalogGauge gaugeMPH = new AnalogGauge(canvasMPH, 75, (Math.PI / 4), 0, 120, 12, true, true);
            AnalogGauge gaugeKMPH = new AnalogGauge(canvasKMPH, 70, (Math.PI / 4), 0, 180, 9, true, false);
            AnalogGauge gaugeRPM = new AnalogGauge(canvasRPM, 75, (Math.PI / 4), 0, 9, 9, true, true, RPMEllipse1,6, 7, 7, 10, Colors.White, Colors.Lime, Colors.OrangeRed);
            AnalogGauge gaugeFuel = new AnalogGauge(canvasFuel, 25, (Math.PI / 4), 0, 15, 2, false, true, fuelEllipse1, 0, .25, 5, 15, Colors.White, Colors.Orange, Colors.LightYellow);
            AnalogGauge gaugeOil = new AnalogGauge(canvasOil, 25, (Math.PI / 4), 0, 100, 2, false, true, oilEllipse1, 0, 20, 80, 100, Colors.White, Colors.OrangeRed, Colors.OrangeRed);
            AnalogGauge gaugeTimeHour = new AnalogGauge(canvasTime, 100, (-Math.PI / 2), 0, 12, 12, true, true);
            AnalogGauge gaugeTimeMinute = new AnalogGauge(canvasTime, 100, (-Math.PI / 2), 0, 60, 0, false, false);

            //Note values are negative to trick the gauge into working backwards.
            AnalogGauge gaugeTemperature = new AnalogGauge(canvasTemperature, 25, ((5 * Math.PI) / 4), -250, -100, 2, false, true, temperatureEllipse1, -120, -100, -250, -200, Colors.White, Colors.LightBlue, Colors.OrangeRed);
            AnalogGauge gaugeBat = new AnalogGauge(canvasBat, 25, ((5 * Math.PI) / 4), -26, 0, 1, false, false);

            DigitalOdometer odo = new DigitalOdometer(odoM, canvasODO);
            DigitalTripometer trip = new DigitalTripometer(tripM, canvasTRIP);

            GearIndicator gearLight = new GearIndicator();
            gearLight.addGearIndicator(gearPark, 0);
            gearLight.addGearIndicator(gearReverse, 1);
            gearLight.addGearIndicator(gearNeutral, 2);
            gearLight.addGearIndicator(gearDrive, 3);
            gearLight.addGearIndicator(gearThird, 4);
            gearLight.addGearIndicator(gearSecond, 5);


            gaugeMPH.update(15);
            gaugeRPM.update(3.75);
            gaugeFuel.update(80);
            gaugeTemperature.update((-1 * 200));
            gaugeOil.update(70);
            gaugeBat.update((-1 * 67));


            mphM.RegisterObserver(gaugeMPH);
            rpmM.RegisterObserver(gaugeRPM);
            tempM.RegisterObserver(gaugeTemperature);
            fuelM.RegisterObserver(gaugeFuel);
            oilM.RegisterObserver(gaugeOil);
            batM.RegisterObserver(gaugeBat);
            timeM.RegisterObserver(gaugeTimeHour);
            timeM.RegisterObserver(gaugeTimeMinute);
            gearM.RegisterObserver(gearLight);
        }

        private void analog_Click(object sender, RoutedEventArgs e)
        {
            

        }

    }
}
