using Xamarin.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;

namespace estimote
{
    public class Beacons : ContentPage
    {
        public Beacons()
        {
            if (Device.OS == TargetPlatform.iOS)
                Padding = new Thickness(0, 20);

            var btnStart = new Button
            {
                WidthRequest = App.ScreenSize.Width / 2,
                Text = "Start scanning"
            };
            btnStart.Clicked += delegate
            {
                DependencyService.Get<IBeacon>().StartScanning(5);
            };

            var btnStop = new Button
            {
                WidthRequest = App.ScreenSize.Width / 2,
                Text = "Stop scanning",
            };
            btnStop.Clicked += delegate
            {
                DependencyService.Get<IBeacon>().StopScanning();
            };

            var btnStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    btnStart, btnStop
                }
            };

            var listView = new ListView
            {
                WidthRequest = App.ScreenSize.Width * .9,
                HasUnevenRows = true,
                ItemsSource = App.Self.Beacons,
                ItemTemplate = new DataTemplate(typeof(BeaconListViewCell)),
                IsPullToRefreshEnabled = true
            };

            App.Self.Beacons.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            {
                listView.ItemsSource = App.Self.Beacons;
            };
            
            Content = new StackLayout
            { 
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    btnStack, listView
                }
            };
        }

        public class BeaconListViewCell : ViewCell
        {
            public BeaconListViewCell()
            {
                var lblUUID = new Label()
                {
                    Text = "lyric",
                    Font = Font.SystemFontOfSize(NamedSize.Default),
                    TextColor = Color.White
                };
                lblUUID.SetBinding(Label.TextProperty, new Binding("ProximityUUID", stringFormat: "UUID = {0}"));

                var lblMajor = new Label()
                {
                    Text = "date",
                    Font = Font.SystemFontOfSize(NamedSize.Small),
                    TextColor = Color.Green,
                    WidthRequest = App.ScreenSize.Width / 2
                };
                lblMajor.SetBinding(Label.TextProperty, new Binding("Major", stringFormat: "Major val = {0}"));

                var lblMinor = new Label()
                {
                    Text = "date",
                    Font = Font.SystemFontOfSize(NamedSize.Small),
                    TextColor = Color.Green,
                    WidthRequest = App.ScreenSize.Width / 2
                };
                lblMinor.SetBinding(Label.TextProperty, new Binding("Minor", stringFormat: "Minor val = {0}"));

                var lblMac = new Label()
                {
                    Text = "date",
                    Font = Font.SystemFontOfSize(NamedSize.Small),
                    TextColor = Color.Lime,
                    WidthRequest = App.ScreenSize.Width / 2
                };
                lblMac.SetBinding(Label.TextProperty, new Binding("MacAddress", stringFormat: "Mac = {0}"));

                var lblRSSI = new Label()
                {
                    Text = "date",
                    TextColor = Color.Lime,
                    WidthRequest = App.ScreenSize.Width / 2,
                    Font = Font.SystemFontOfSize(NamedSize.Small)
                };
                lblRSSI.SetBinding(Label.TextProperty, new Binding("Rssi", stringFormat: "Signal = {0}"));

                var stackDataTop = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        lblMajor, lblMinor
                    }
                };
                var stackDataBottom = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        lblMac, lblRSSI
                    }
                };

                View = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Padding = new Thickness(12, 8),
                    Children = { lblUUID, stackDataTop, stackDataBottom }
                };
            }
        }
    }
}


