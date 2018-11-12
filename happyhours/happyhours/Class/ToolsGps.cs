using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace happyhours.Class
{
    public class ToolsGps
    {
      

        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }


    
        public async void GetPosition()
        {
Plugin.Geolocator.Abstractions.Position position = null;
            try
            {

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                if (position != null) {
                    
                }

                //  if (position != null)
                //  {
                //     _position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                //      //got a cahched position, so let's use it.
                //      return;
                //  }

                    //  if (!locatore.IsGeolocationAvailable || !locatore.IsGeolocationEnabled)
                    //  {
                    //not available or enabled
                    //      await DisplayAlert("Attention GPS Désactivé", "GPS désactivé, merci d'ativé votre GPS sur l'appareil.", "OK");
                    //   await DisplayAlert("Attention GPS Désativé", "GPS désactivé, merci d'ativé votre GPS sur l'appareil.", "OK");
                    //    return;
                    //   }

                    //                position = await locatore.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                throw ex;
                //Display error as we have timed out or can't get location.
            }
         //   _position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
         //   if (position == null)
           //     return;

        }

    }
}
