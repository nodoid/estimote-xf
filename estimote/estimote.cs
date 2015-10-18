using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace estimote
{
    public class App : Application,INotifyPropertyChanged
    {
        public string APP_ID { get; private set; } = "YOUR_APP_ID";

        public string APP_TOKEN { get; private set; } = "YOUR_TOKEN_ID";

        public string BEACON_ID { get; private set; } = "YOUR_BEACON_ID";

        public ObservableCollection<BeaconData> Beacons { get; set; }

        public static App Self { get; private set; }

        public static Size ScreenSize { get; set; }

        public BeaconChangedEvent BeaconChangedClass { get; set; }

        private bool isRunning;

        public bool IsRunning
        {
            get
            {
                return isRunning; 
            } 
            set
            { 
                if (isRunning != value)
                {
                    isRunning = value; 
                    OnPropertyChanged("IsRunning");
                }
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public App()
        {
            App.Self = this;
            BeaconChangedClass = new BeaconChangedEvent();
            Beacons = new ObservableCollection<BeaconData>();

            IsRunning = false;

            DependencyService.Get<IBeacon>().InitialiseBeacon();

            // The root page of your application
            MainPage = new estimote.Beacons();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

