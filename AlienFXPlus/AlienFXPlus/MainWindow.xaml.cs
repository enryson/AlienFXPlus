using LightFX;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlienFXPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool exit = false;

        public MainWindow()
        {
            InitializeComponent();

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active).ToList();
            comboBox1.ItemsSource = devices;

            var lightFX = new LightFXController();
            var result = lightFX.LFX_Initialize();



            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Tick += new EventHandler((sender, e) => dispatcherTimer_Tick(sender, e, lightFX));
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcherTimer.Start();
        }

        private void setWinColor(LightFXController lightFX)
        {
            string colorStr = SystemParameters.WindowGlassBrush.ToString();
            colorStr = colorStr.Replace("#", string.Empty);
            var a = (byte)System.Convert.ToUInt32(colorStr.Substring(0, 2), 16);
            var r = (byte)System.Convert.ToUInt32(colorStr.Substring(2, 2), 16);
            var g = (byte)System.Convert.ToUInt32(colorStr.Substring(4, 2), 16);
            var b = (byte)System.Convert.ToUInt32(colorStr.Substring(6, 2), 16);

            var c1 = new LFX_ColorStruct(255, r, g, b);
            useLFXLights(lightFX, c1);
        }



        private void dispatcherTimer_Tick(object sender, EventArgs e, LightFXController lightFX)
        {
            var c1 = new LFX_ColorStruct(255, 0, 0, 0);

            if (comboBox1.SelectedItem != null)
            {
                var device = (MMDevice)comboBox1.SelectedItem;
                int varPuls = (int)(device.AudioMeterInformation.MasterPeakValue * 100);
                //label1.Content = varPuls;                           

                if (varPuls > 5)
                    c1 = new LFX_ColorStruct(255, 0, 0, 0);
                if (varPuls > 10)
                    c1 = new LFX_ColorStruct(255, 255, 233, 0);
                if (varPuls > 20)
                    c1 = new LFX_ColorStruct(255, 255, 255, 0);
                if (varPuls > 30)
                    c1 = new LFX_ColorStruct(255, 255, 0, 255);
                if (varPuls > 35)
                    c1 = new LFX_ColorStruct(255, 40, 255, 0);
                if (varPuls > 40)
                    c1 = new LFX_ColorStruct(255, 200, 200, 255);
                if (varPuls > 45)
                    c1 = new LFX_ColorStruct(255, 0, 0, 100);
                if (varPuls > 48)
                    c1 = new LFX_ColorStruct(255, 0, 255, 100);
                if (varPuls > 50)
                    c1 = new LFX_ColorStruct(255, 255, 0, 255);
                if (varPuls > 53)
                    c1 = new LFX_ColorStruct(255, 255, 233, 0);
                if (varPuls > 56)
                    c1 = new LFX_ColorStruct(255, 255, 255, 0);
                if (varPuls > 57)
                    c1 = new LFX_ColorStruct(255, 255, 0, 255);
                if (varPuls > 60)
                    c1 = new LFX_ColorStruct(255, 40, 255, 0);
                useLFXLights(lightFX, c1);
            }
        }

        private static void useLFXLights(LightFXController lightFX, LFX_ColorStruct color)
        {
            lightFX.LFX_Light(LFX_Position.LFX_All, color);
            lightFX.LFX_Update();
        }

        private void RadioMedia_Checked(object sender, RoutedEventArgs e)
        {

            if (RadioMedia.IsChecked == true)
                label1.Content = "alienFX Media";
            if (RadioWinTheme.IsChecked == true)
            {
                label1.Content = "alienFX Win";
            }
        }
    }
}
