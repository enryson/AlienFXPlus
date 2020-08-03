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
        public int option;

        public System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            //------------Hide App When starts-------------//
            this.Hide();
            //------------START NAUDIO DEVICES-------------//
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active).ToList();
            //------------List Devices Naudio Library-------------//
            comboBox1.ItemsSource = devices;
            comboBox1.SelectedIndex = 0;
            //------------Start AlienFX Controller-------------//
            var lightFX = new LightFXController();
            var result = lightFX.LFX_Initialize();

            RadioWinTheme.IsChecked = true;

            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Tick += new EventHandler((sender, e) => dispatcherTimer_Tick(sender, e, lightFX));
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
            label1.Content = "A : "+ a +" RED :" + r + " Green :" + g + " Blue :" + b + "    HEX :" + colorStr;
            Brush SpeedColor = new SolidColorBrush(Color.FromRgb(r, g, b));
            label1.Foreground = SpeedColor;
            useLFXLights(lightFX, c1);
        }

        private void fxMusicAnalyse(LightFXController lightFX)
        {
            var c1 = new LFX_ColorStruct(255, 0, 0, 0);
            if (comboBox1.SelectedItem != null)
            {
                Random rnd = new Random();
                var device = (MMDevice)comboBox1.SelectedItem;
                //int varPuls = (int)(device.AudioMeterInformation.MasterPeakValue * 90);

                int varRed = (int)(device.AudioMeterInformation.MasterPeakValue * rnd.Next(3, 8));
                int varGreen = (int)(device.AudioMeterInformation.MasterPeakValue * rnd.Next(5, 9));
                int varBlue = (int)(device.AudioMeterInformation.MasterPeakValue * rnd.Next(1, 7));

                int intRed = varRed * rnd.Next(varRed, 100);
                int intGreen = varGreen * rnd.Next(varGreen, 100);
                int intBlue = varGreen * rnd.Next(varGreen, 100);
                byte red = Convert.ToByte(intRed >= 255? 255 : intRed);
                byte green = Convert.ToByte(intGreen >= 255 ? 255 : intGreen);
                byte blue = Convert.ToByte(intBlue >= 255 ? 255 : intBlue);
                label1.Content = "R__" + varRed.ToString()+ "  G__"+varGreen.ToString() + "  B__" + varBlue.ToString();

                c1 = new LFX_ColorStruct(255, red, green, blue);

                /*
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
                */
                useLFXLights(lightFX, c1);
            }
        }

        private static void useLFXLights(LightFXController lightFX, LFX_ColorStruct color)
        {
            lightFX.LFX_Light(LFX_Position.LFX_All, color);
            lightFX.LFX_Update();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e, LightFXController lightFX)
        {
            int caseSwitch = option;
            switch (caseSwitch)
            {
                case 1:
                    fxMusicAnalyse(lightFX);
                    break;
                case 2:
                    setWinColor(lightFX);
                    break;
                default:
                    setWinColor(lightFX);
                    break;
            }

        }

        private void RadioMedia_Checked(object sender, RoutedEventArgs e)
        {
            if (RadioMedia.IsChecked == true)
            {
                label1.Content = "alienFX Media";
                option = 1;
                dispatcherTimer.Stop();
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
                dispatcherTimer.Start();
            }
            if (RadioWinTheme.IsChecked == true)
            {
                dispatcherTimer.Stop();
                option = 2;
                label1.Content = "alienFX Win";
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                dispatcherTimer.Start();
            }
        }

        private void FxWin_Click(object sender, RoutedEventArgs e)
        {
            RadioWinTheme.IsChecked = true;
        }

        private void FxMedia_Click(object sender, RoutedEventArgs e)
        {
            RadioMedia.IsChecked = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OpenAlienFX_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
        }

        private bool m_isExplicitClose = false;

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (m_isExplicitClose == false)//NOT a user close request? ... then hide
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void OnTaskBarMenuItemExitClick(object sender, RoutedEventArgs e)
        {
            m_isExplicitClose = true;//Set this to unclock the Minimize on close 

            this.Close();
        }
    }
}
