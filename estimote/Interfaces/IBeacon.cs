namespace estimote
{
    public interface IBeacon
    {
        void StartScanning(int milliSeconds = 0);

        void StopScanning();

        void InitialiseBeacon();
    }
}

