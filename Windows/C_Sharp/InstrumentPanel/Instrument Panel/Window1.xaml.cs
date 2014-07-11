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
using System.Windows.Shapes;

namespace Instrument_Panel
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.Left = 0;
            this.Top = 0;
        }

        void gas_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RPMModel rm = RPMModel.Instance;

            rm.UpdateGas(e.NewValue);

            MPHModel mm = MPHModel.Instance;
            mm.GasUpdate(e.NewValue);

            brake.Value = 0;

        }

        void brake_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            RPMModel rm = RPMModel.Instance; 
            rm.UpdateBrake(e.NewValue);

            MPHModel mm = MPHModel.Instance;
            mm.BrakeUpdate(e.NewValue);

            gas.Value = 0;

        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            FUELModel fm = FUELModel.Instance;
            fm.UpdateFuel(1);

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            STARTModel startM = STARTModel.Instance;
            startM.UpdateSTART(1);


        }
        private void TBtn1_Checked(object sender, RoutedEventArgs e)
        {
            GEARModel gm = GEARModel.Instance;
            gm.UpdateGEAR(0);
        }
        private void TBtn2_Checked(object sender, RoutedEventArgs e)
        {
            GEARModel gm = GEARModel.Instance;
            gm.UpdateGEAR(1);

        }
        private void TBtn3_Checked(object sender, RoutedEventArgs e)
        {
            GEARModel gm = GEARModel.Instance;
            gm.UpdateGEAR(2);
        }
        private void TBtn4_Checked(object sender, RoutedEventArgs e)
        {
            GEARModel gm = GEARModel.Instance;
            gm.UpdateGEAR(3);
        }
        private void TBtn5_Checked(object sender, RoutedEventArgs e)
        {
            GEARModel gm = GEARModel.Instance;
            gm.UpdateGEAR(4);
        }
        private void TBtn6_Checked(object sender, RoutedEventArgs e)
        {
            GEARModel gm = GEARModel.Instance;
            gm.UpdateGEAR(5);
        }
        
        private void DBtn1_Checked(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Show();
            //(App.Current.MainWindow as Window2).Hide();
        }
        private void DBtn2_Checked(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Hide();

        }
        private void DBtn3_Checked(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).Show();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

            TRIPModel tm = TRIPModel.Instance;
            tm.ResetTrip();

        }
    }
}
