using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Management;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class ViewModel : ViewModelBase
    {
        private Visibility _fineImageVisibility;
        public Visibility FineImageVisibility
        {
            get { return _fineImageVisibility; }
            set
            {
                if (_fineImageVisibility != value)
                {
                    _fineImageVisibility = value;
                    RaisePropertyChanged("FineImageVisibility");
                }
            }
        }
        private Visibility _hotImageVisibility;
        public Visibility HotImageVisibility
        {
            get { return _hotImageVisibility; }
            set
            {
                if (_hotImageVisibility != value)
                {
                    _hotImageVisibility = value;
                    RaisePropertyChanged("HotImageVisibility");
                }
            }
        }
        private Visibility _coldImageVisibility;
        public Visibility ColdImageVisibility
        {
            get { return _coldImageVisibility; }
            set
            {
                if (_coldImageVisibility != value)
                {
                    _coldImageVisibility = value;
                    RaisePropertyChanged("ColdImageVisibility");
                }
            }
        }

        private Visibility _celsiusChartVisibility;
        public Visibility CelsiusChartVisibility
        {
            get { return _celsiusChartVisibility; }
            set
            {
                if (value != _celsiusChartVisibility)
                {
                    _celsiusChartVisibility = value;
                    RaisePropertyChanged("CelsiusChartVisibility");
                }
            }
        }

        private Visibility _farenheitChartVisibility;
        public Visibility FarenheitChartVisibility
        {
            get { return _farenheitChartVisibility; }
            set
            {
                if (value != _farenheitChartVisibility)
                {
                    _farenheitChartVisibility = value;
                    RaisePropertyChanged("FarenheitChartVisibility");
                }
            }
        }

        private DateTime _startRecord;
        public DateTime StartRecord
        {
            get { return _startRecord; }
            set
            {
                if (value != _startRecord)
                    _startRecord = value;
            }
        }

        private ObservableCollection<Point> _celsiusTemperatureChart;
        public ObservableCollection<Point> CelsiusTemperatureChart
        {
            get { return _celsiusTemperatureChart; }
            set
            {
                _celsiusTemperatureChart = value;
                RaisePropertyChanged("CelsiusTemperatureChar");
            }
        }

        private ObservableCollection<Point> _farenheitTemperatureChart;
        public ObservableCollection<Point> FarenheitTemperatureChart
        {
            get { return _farenheitTemperatureChart; }
            set
            {
                _farenheitTemperatureChart = value;
                RaisePropertyChanged("FarenheitTemperatureChar");
            }
        }

        private SerialPort _port;
        public SerialPort Port
        {
            get { return _port; }
            set
            {
                if (value != _port)
                    _port = value;
            }
        }

        private string _sentence;
        public string Sentence
        {
            get { return _sentence; }
            set
            {
                if (value != _sentence)
                {
                    _sentence = value;
                    RaisePropertyChanged("Sentence");
                }
            }
        }

        private Sentences sentencesObject;
        public Sentences SentencesObject
        {
            get { return sentencesObject; }
            set { sentencesObject = value; }
        }


        private string _temperature;
        public string Temperature
        {
            get { return _temperature; }
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                    RaisePropertyChanged("Temperature");
                }
            }
        }

        private bool _temperatureSwitch;
        public bool TemperatureSwitch
        {
            get { return _temperatureSwitch; }
            set
            {
                if (value != _temperatureSwitch)
                    _temperatureSwitch = value;
            }
        }

        public ICommand ButtonOn { get; set; }
        public ICommand ButtonOff { get; set; }
        public ICommand ButtonCelsius { get; set; }
        public ICommand ButtonFarenheit { get; set; }
        public ViewModel()
        {
            SentencesObject = new Sentences();
            Sentence = "Waiting for a temperature...";
            FarenheitChartVisibility = Visibility.Collapsed;
            CelsiusChartVisibility = Visibility.Visible;
            StartRecord = DateTime.Now;
            CelsiusTemperatureChart = new ObservableCollection<Point>();
            FarenheitTemperatureChart = new ObservableCollection<Point>();
            Temperature = "0";
            TemperatureSwitch = true;
            Port = new SerialPort();
            FineImageVisibility = Visibility.Visible;
            ColdImageVisibility = Visibility.Hidden;
            HotImageVisibility = Visibility.Hidden;
            InitializePort();

            ButtonOn = new DelegateCommand(OnCommandButtonOn);
            ButtonOff = new DelegateCommand(OnCommandButtonOff);
            ButtonCelsius = new DelegateCommand(OnCommandButtonCelsius);
            ButtonFarenheit = new DelegateCommand(OnCommandButtonFarenheit);
        }

        public void InitializePort()
        {
            Port.DataReceived += new SerialDataReceivedEventHandler(GetValueFromArduino);
            Port.BaudRate = 9600;
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                if (portnames.Length == 1)
                    Port.PortName = portnames[0];
                else
                {
                    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                    var tList = (from n in portnames
                                 join p in ports on n equals p["DeviceID"].ToString()
                                 select n + " - " + p["Caption"]).ToList();

                    foreach (string s in tList)
                    {
                        if (s.Contains("Arduino"))
                        {
                            Port.PortName = s.Substring(0, 5).Replace(" ", string.Empty);
                        }
                    }
                }
                if (string.IsNullOrEmpty(Port.PortName))
                {
                    //TODO : display error message
                }
            }
        }

        private void GetValueFromArduino(object sender, SerialDataReceivedEventArgs e)
        {
            DateTime timer_method = new DateTime();
            List<string> temp_s = new List<string>();
            string s = Port.ReadExisting();
            float tmp_temperature = 0;

            if (!string.IsNullOrEmpty(s))
            {
                temp_s = Regex.Split(s, "\\r\\n").ToList();
                temp_s = temp_s.Where(x => !string.IsNullOrEmpty(x)).Select(x => x).ToList();
                foreach (string t in temp_s)
                {
                    tmp_temperature = float.Parse(t, CultureInfo.InvariantCulture.NumberFormat);
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => CelsiusTemperatureChart.Add(
                        new Point(Math.Round(DateTime.Now.TimeOfDay.TotalSeconds - StartRecord.TimeOfDay.TotalSeconds),
                                  Math.Round(tmp_temperature, 2)))));
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => FarenheitTemperatureChart.Add(
                        new Point(Math.Round(DateTime.Now.TimeOfDay.TotalSeconds - StartRecord.TimeOfDay.TotalSeconds),
                                  Math.Round((tmp_temperature * 9 / 5) + 32, 2)))));
                }
                if (TemperatureSwitch)
                    Temperature = tmp_temperature.ToString() + " °C";
                else
                    Temperature = Math.Round((tmp_temperature * 9 / 5) + 32, 2).ToString() + " °F";

                timer_method = DateTime.Now;
                timer_method = timer_method.AddMinutes(-(timer_method.Minute % 2)).AddSeconds(-timer_method.Second);
                timer_method = timer_method.AddSeconds(5);
                Sentence = SentencesObject.GetSentence(tmp_temperature, timer_method);
                DisplayFace(tmp_temperature);
            }
        }

        public void DisplayFace(float temperature)
        {
            if (temperature > 30.0f)
            {
                FineImageVisibility = Visibility.Hidden;
                ColdImageVisibility = Visibility.Hidden;
                HotImageVisibility = Visibility.Visible;
            }
            else if (temperature < 5.0f)
            {
                FineImageVisibility = Visibility.Hidden;
                ColdImageVisibility = Visibility.Visible;
                HotImageVisibility = Visibility.Hidden;
            }
            else
            {
                FineImageVisibility = Visibility.Visible;
                ColdImageVisibility = Visibility.Hidden;
                HotImageVisibility = Visibility.Hidden;
            }
        }

        public void OnCommandButtonCelsius(object arg)
        {
            if (Port.IsOpen)
            {
                TemperatureSwitch = true;
                Temperature = CelsiusTemperatureChart.Last().Y.ToString() + " °C";
                FarenheitChartVisibility = Visibility.Collapsed;
                CelsiusChartVisibility = Visibility.Visible;
            }
        }

        public void OnCommandButtonFarenheit(object arg)
        {
            if (Port.IsOpen)
            {
                TemperatureSwitch = false;
                Temperature = FarenheitTemperatureChart.Last().Y.ToString() + " °F";
                CelsiusChartVisibility = Visibility.Collapsed;
                FarenheitChartVisibility = Visibility.Visible;
            }
        }

        public void OnCommandButtonOn(object arg)
        {
            if (!Port.IsOpen)
                Port.Open();
        }

        public void OnCommandButtonOff(object arg)
        {
            if (Port.IsOpen)
                Port.Close();
        }
    }
}
