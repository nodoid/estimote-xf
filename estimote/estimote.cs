using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace estimote
{
    public class App : Application
    {
        public string APP_ID { get; private set; } = "estimote-eyl";

        public string APP_TOKEN { get; private set; } = "1926a6c48604313b6c1ea30eb18085e0";

        public string BEACON_ID { get; private set; } = "b9407f30-f5f8-466e-aff9-25556b57fe6d";

        public ObservableCollection<BeaconData> Beacons { get; set; }

        public static App Self { get; private set; }

        public static Size ScreenSize { get; set; }

        public BeaconChangedEvent BeaconChangedClass { get; set; }

        public App()
        {
            App.Self = this;
            BeaconChangedClass = new BeaconChangedEvent();
            Beacons = new ObservableCollection<BeaconData>();

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

