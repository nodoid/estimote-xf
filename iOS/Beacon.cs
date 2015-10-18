using System;
using estimote.iOS;
using Estimote;
using CoreLocation;
using Foundation;
using System.Linq;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(BeaconDS))]
namespace estimote.iOS
{
    public class BeaconDS:IBeacon
    {
        BeaconManager BeaconManager;
        CLBeaconRegion BeaconRegion;
        CLProximity previousProximity;
        CloudManager CloudManager;

        string message;

        public void InitialiseBeacon()
        {
            Config.Setup(App.Self.APP_ID, App.Self.APP_TOKEN);
        }

        public void StartScanning(int milliSecond = 0)
        {
            BeaconManager = new BeaconManager();
            BeaconManager.ReturnAllRangedBeaconsAtOnce = true;
            BeaconRegion = new CLBeaconRegion(new NSUuid(App.Self.BEACON_ID), "beacons");
            CheckForBluetooth();
            BeaconRegion.NotifyEntryStateOnDisplay = true;
            BeaconRegion.NotifyOnEntry = true;
            BeaconRegion.NotifyOnExit = true;

            AppDelegate.Self.LocationManager.DidStartMonitoringForRegion += (object sender, CLRegionEventArgs e) =>
            AppDelegate.Self.LocationManager.RequestState(e.Region);


            CloudManager = new CloudManager();

            try
            {
                var beacons = CloudManager.FetchEstimoteBeaconsAsync().ContinueWith(t =>
                    {
                        if (t.IsCompleted)
                        {
                            var list = t.Result;
                            foreach (var b in list)
                            {
                                App.Self.Beacons.Add(new BeaconData
                                    {
                                        MacAddress = b.MacAddress.ToString(),
                                        Major = (int)b.Major, MeasuredPower = (int)b.Power, Minor = (int)b.Minor,
                                        Name = b.Name, /*Rssi = e.Region.Rssi,*/ ProximityUUID = b.ToString(),
                                        Region = new RegionData
                                        {
                                            Major = (int)b.Major, Minor = (int)b.Minor, 
                                            Identifier = b.Name, ProximityUUID = b.ProximityUUID.ToString()
                                        }
                                    });
                            }
                        }
                    }).ConfigureAwait(true);  
            }
            catch
            {
                new UIAlertView("Error", "Unable to fetch cloud beacons, ensure you have set Config in AppDelegate", null, "OK").Show();
            }

            AppDelegate.Self.LocationManager.RegionEntered += (object sender, CLRegionEventArgs e) =>
            {
                if (e.Region.Identifier == App.Self.BEACON_ID)
                {
                    Console.WriteLine("beacon region entered");
                }
            };

            AppDelegate.Self.LocationManager.DidDetermineState += (object sender, CLRegionStateDeterminedEventArgs e) =>
            {

                switch (e.State)
                {
                    case CLRegionState.Inside:
                        Console.WriteLine("region state inside");
                        break;
                    case CLRegionState.Outside:
                        Console.WriteLine("region state outside");
                        break;
                    case CLRegionState.Unknown:
                    default:
                        Console.WriteLine("region state unknown");
                        break;
                }
            };

            AppDelegate.Self.LocationManager.DidRangeBeacons += (object sender, CLRegionBeaconsRangedEventArgs e) =>
            {
                if (e.Beacons.Length > 0)
                {

                    CLBeacon beacon = e.Beacons[0];
                    switch (beacon.Proximity)
                    {
                        case CLProximity.Immediate:
                            message = "Immediate";
                            break;
                        case CLProximity.Near:
                            message = "Near";
                            break;
                        case CLProximity.Far:
                            message = "Far";
                            break;
                        case CLProximity.Unknown:
                            message = "Unknown";
                            break;
                    }

                    if (previousProximity != beacon.Proximity)
                    {
                        Console.WriteLine(message);
                    }
                    previousProximity = beacon.Proximity;
                }
            };

            //if (BeaconManager.IsAuthorizedForMonitoring && BeaconManager.IsAuthorizedForRanging)
            //{
            BeaconRegion = new CLBeaconRegion(new NSUuid(App.Self.BEACON_ID), "BeaconSample");
            BeaconManager.RangedBeacons += (object sender, RangedBeaconsEventArgs e) =>
            {
                foreach (var beacon in App.Self.Beacons)
                {
                    var found = e.Beacons.FirstOrDefault(t => (int)t.Major == beacon.Major);
                    var idx = e.Beacons.ToList().IndexOf(found);
                    if (found != null)
                    {
                        if (idx < App.Self.Beacons.Count)
                        {
                            App.Self.Beacons[idx].Rssi = (int)found.Rssi;
                        }
                    }
                }
            };


            BeaconManager.EnteredRegion += (sender, e) =>
            {
                if (App.Self.Beacons.Count != 0)
                {
                    var exists = (from beacon in App.Self.Beacons
                                                 where beacon.Major == (int)e.Region.Major
                                                 select beacon).FirstOrDefault();
                    //var exists = App.Self.Beacons.FirstOrDefault(t => t.Major == e.Beacons.FirstOrDefault(w => w.Major));
                    if (exists == null)
                        App.Self.Beacons.Add(exists);
                    else
                    {
                        var index = App.Self.Beacons.IndexOf(exists);
                        if (index != -1)
                            App.Self.Beacons[index] = exists;
                    }
                }
                else
                {
                    App.Self.Beacons.Add(new BeaconData
                        {
                            /*MacAddress = beacon.MacAddress.ToString(),*/
                            Major = (int)e.Region.Major, MeasuredPower = (int)e.Region.Radius, Minor = (int)e.Region.Minor,
                            /*Name = beacon.Name, Rssi = e.Region.Rssi,*/ ProximityUUID = e.Region.ProximityUuid.ToString(),
                            Region = new RegionData
                            {
                                Major = (int)e.Region.Major, Minor = (int)e.Region.Minor, 
                                Identifier = e.Region.Identifier, ProximityUUID = e.Region.ProximityUuid.ToString()
                            }
                        });
                }
            };

            BeaconManager.ExitedRegion += (sender, e) =>
            {
                if (App.Self.Beacons.Count != 0)
                {
                    var exists = (from b in App.Self.Beacons
                                                 let reg = b.Region
                                                 where reg.Identifier == e.Region.Identifier
                                                 select reg).FirstOrDefault();
                    if (exists != null)
                    {
                        var index = App.Self.Beacons.IndexOf(App.Self.Beacons.FirstOrDefault(t => t.Region.Major == (int)e.Region.Major));
                        App.Self.Beacons.RemoveAt(index);
                    }
                }
            };
            //}
        }

        public void StopScanning()
        {
            if (BeaconManager != null)
            {
                BeaconManager.StartMonitoringForRegion(BeaconRegion);
                BeaconManager.StopRangingBeaconsInRegion(BeaconRegion);
                BeaconManager.Dispose();
            }
        }

        void CheckForBluetooth()
        {
            AppDelegate.Self.LocationManager.RequestState(BeaconRegion);
            BeaconManager.RequestWhenInUseAuthorization();

            var status = BeaconManager.AuthorizationStatus;
            if (status == CLAuthorizationStatus.AuthorizedAlways)
            {
                AppDelegate.Self.LocationManager.StartMonitoring(BeaconRegion);
                AppDelegate.Self.LocationManager.StartRangingBeacons(BeaconRegion);
            }
            else if (status == CLAuthorizationStatus.Denied)
            {
                new UIAlertView("Location Access Denied", "You have denied access to location services. Change this in app settings.", null, "OK").Show();
            }
            else if (status == CLAuthorizationStatus.Restricted)
            {
                new UIAlertView("Location Not Available", "You have no access to location services.", null, "OK").Show();
            }

            BeaconManager.AuthorizationStatusChanged += (object sender, AuthorizationStatusChangedEventArgs e) =>
            {
                if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
                    return;
                else
                    App.Self.BeaconChangedClass.BroadcastIt("You need to allow for bluetooth permissions for this to work");
            };
        }
    }
}

