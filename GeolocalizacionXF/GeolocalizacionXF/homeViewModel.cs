using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Essentials;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GeolocalizacionXF
{
    class homeViewModel : INotifyPropertyChanged
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.UI;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Attributes
        private ObservableCollection<Mylocation> listaLocations;
        private double latitud;
        private double? altitud;
        private double presion;
        private double longitud;
        #endregion


        #region Properties
        public ObservableCollection<Mylocation> ListaLocations
        {
            get { return listaLocations; }
            set
            {
                listaLocations = value;
                OnPropertyChanged("ListaLocations");
            }
        }
        public double Presion
        {
            get { return presion; }
            set
            {
                presion = value;
                OnPropertyChanged("Presion");
            }
        }
        public double Latitud
        {
            get { return latitud; }
            set
            {
                latitud = value;
                OnPropertyChanged("Latitud");
            }
        }
        public double Longitud
        {
            get { return longitud; }
            set
            {
                longitud = value;
                OnPropertyChanged("Longitud");
            }
        }
        public double? Altitud
        {
            get { return altitud; }
            set
            {
                altitud = value;
                OnPropertyChanged("Altitud");
            }
        }
        #endregion

        public homeViewModel()
        {
            Barometer.Start(speed);
            this.ListaLocations = new ObservableCollection<Mylocation>();
            Altitud = 0;
        }

        void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {

            var data = e.Reading;
            // Process Pressure
            Console.WriteLine($"Reading: Pressure: {data.PressureInHectopascals} hectopascals");
            var presion = data.PressureInHectopascals;
            Presion = presion;

        }
        async Task getLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {

                    Latitud = location.Latitude;
                    Altitud = location.Altitude;
                    Longitud = location.Longitude;
                    List<Mylocation> lista = new List<Mylocation>();
                    
                    var locationnew = new Mylocation(

                        )
                    {
                        Alatitud = Latitud,
                        Alongitud = Longitud,
                        Aaltitud = Altitud,
                        Apresion = Presion,
                    };
                    this.ListaLocations.Add(locationnew);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public void ToggleBarometer()
        {
            try
            {
                if (Barometer.IsMonitoring)
                    Barometer.Stop();
                else
                    Barometer.Start(speed);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                //error
            }
        }

        public System.Windows.Input.ICommand ObtenerPresionCommand
        {
            get
            {
                return new RelayCommand(ObtenerPresion);
            }
        }

        private async void ObtenerPresion()
        {
            Latitud = 0;
            Altitud = 0;
            Longitud = 0;
            Presion = 0;
            await getLocation();
            Barometer.ReadingChanged += Barometer_ReadingChanged;
        }



    }
}
public class Mylocation
{
    public double Alatitud { get; set; }
    public double Alongitud { get; set; }
    public double? Aaltitud { get; set; }
    public double Apresion { get; set; }
}

