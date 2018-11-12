using happyhours.Fichiers;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static happyhours.Objets.ListAdresse;

namespace happyhours.Vues
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListAddress : ContentPage
    {
		public ListAddress ()
		{
			InitializeComponent ();

            ShowPopUp("address", false);
            ShowPopUp("waiting", true);

            GetPosition();
            /*  StackLayout BaseStack = new StackLayout();
              BaseStack.Orientation = StackOrientation.Vertical;
              BaseStack.HorizontalOptions = LayoutOptions.FillAndExpand;
              BaseStack.VerticalOptions = LayoutOptions.FillAndExpand;

              ActivityIndicator activityIndicator = new ActivityIndicator();
              activityIndicator.HorizontalOptions = LayoutOptions.FillAndExpand;
              activityIndicator.VerticalOptions = LayoutOptions.FillAndExpand;
              activityIndicator.IsRunning = true;

              BaseStack.Children.Add(activityIndicator);
  */
            //  ListAddressXAML.Children.Add(BaseStack);
            /*
             <StackLayout  x:Name="WaitingXAML" AbsoluteLayout.LayoutBounds="0.5, 0.5, -1, -1" AbsoluteLayout.LayoutFlags="PositionProportional" IsVisible="False" Opacity="0.8" BackgroundColor="Gray" >
                        <Image x:Name="ImgWaiting" Source="http://www.satsignal.eu/wxsat/Meteosat7-full-scan.jpg"></Image>
                        <ActivityIndicator BindingContext="{x:Reference ImgWaiting}" IsRunning="true"></ActivityIndicator>

                    </StackLayout>
             */
        }

        public async void GetPosition()
        {
            Position PositionUser = null;
            try
            {

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                PositionUser = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                if (PositionUser != null)
                {
                    //  await DisplayAlert(VariablesGlobal.LanguageApp.Infos, "Longitude : " + PositionUser.Longitude + "-- Latitude : " + PositionUser.Latitude + "-- Timestamp : " + PositionUser.Timestamp + "-- Speed : " + PositionUser.Speed + "-- Heading : " + PositionUser.Heading + "-- AltitudeAccuracy : " + PositionUser.AltitudeAccuracy + "-- Altitude : " + PositionUser.Altitude + "-- Accuracy : " + PositionUser.Accuracy, VariablesGlobal.LanguageApp.Ok);

                    ConvertLongLatToAddressAsync(PositionUser, locator);
                }
                else
                {
                    await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ReponseErreurLocalisationGps, VariablesGlobal.LanguageApp.Ok);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de Géolocalisation : " + ex.Message.ToString());
                await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ReponseErreurLocalisationGps, VariablesGlobal.LanguageApp.Ok);
            }


        }


        private async void ConvertLongLatToAddressAsync(Position positionUser, IGeolocator locator)
        {

            var addresses = await locator.GetAddressesForPositionAsync(positionUser);


            // Array ListAdd;
            if (addresses.Count() > 0)
            {
                // ObservableCollection<ListViewAddressStruct> ListViewAddressStruct = new ObservableCollection<ListViewAddressStruct>();
                List<ListViewAddressStruct> ListViewAddressStruct = new List<ListViewAddressStruct>();
                //  List<ListViewAddressStruct> ListAdd = new List<ListViewAddressStruct>();
                //  int[] ListAdd = new int[addresses.Count()];
                // var ListAdd2 = new List<string>();

                // Console.WriteLine("addresses.Count()=" + addresses.Count());
                int I = 0;
                foreach (var address in addresses)
                {
                    I = I + 1;

                    if (address == null)
                    {
                        await DisplayAlert("Alert", "No address found. Please enter your address details manually", "OK");
                        // applicationSpinner.IsVisible = false;
                    }
                    else
                    {
                        ListViewAddressStruct item = new ListViewAddressStruct();

                        //  Console.WriteLine(" address.Thoroughfare:" + address.Thoroughfare + " address.Locality:" + address.Locality + " address.SubThoroughfare:" + address.SubThoroughfare + " address.AdminArea:" + address.AdminArea + " address.CountryCode:" + address.CountryCode + " address.CountryName:" + address.CountryName + " address.FeatureName:" + address.FeatureName + " address.Locality:" + address.Locality + " address.Latitude:" + address.Latitude + " address.Longitude:" + address.Longitude + " address.PostalCode:" + address.PostalCode + " address.SubAdminArea:" + address.SubAdminArea + " address.SubLocality:" + address.SubLocality + " address.SubThoroughfare:" + address.SubThoroughfare + " address.Thoroughfare:" + address.Thoroughfare );
                        //  RueXAML.Text = address.Thoroughfare;

                        //  NumeroXAML.Text = address.Locality;
                        //  CodePostalXAML.Text = address.SubThoroughfare;
                        item.Id = I;
                        item.CodePostal = address.PostalCode;
                        item.Latitude = address.Latitude.ToString();
                        item.Longitude = address.Longitude.ToString();
                        item.Numero = address.SubThoroughfare;
                        item.Pays = address.CountryName;
                        item.Rue = address.Thoroughfare;
                        item.Ville = address.Locality;


                        ListViewAddressStruct.Add(item);
                    }

                }



                ListAddXAML.ItemsSource = ListViewAddressStruct;


                ListAddXAML.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
                    AddresseSelect(sender, e);
                };

                ShowPopUp("waiting", false);
                ShowPopUp("address", true);
            }
            else
            {
                await DisplayAlert("Alert", "No address found. Please enter your address details manually", "OK");
                // applicationSpinner.IsVisible = false;
            }

        }

        private void AddresseSelect(object sender, SelectedItemChangedEventArgs e)
        {
            ListViewAddressStruct item = (ListViewAddressStruct)e.SelectedItem;

            // RueXAML.Text = item.Rue;
            // NumeroXAML.Text = item.Numero;
            // CodePostalXAML.Text = item.CodePostal;
            // VilleXAML.Text = item.Ville;

            VariablesGlobal.Rue = item.Rue;
            VariablesGlobal.Numero = item.Numero;
            VariablesGlobal.CodePostal = item.CodePostal;
            VariablesGlobal.Ville = item.Ville;
            VariablesGlobal.Latitude = item.Latitude;
            VariablesGlobal.Longitude = item.Longitude;
            VariablesGlobal.Pays = item.Pays;
            





            ShowPopUp("waiting", false);
            ShowPopUp("address", false);
            Navigation.PopAsync();
        }


        private async void OnClose(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public bool ShowPopUp(string arg, bool Status)
        {
            bool Retour = false;
            if (arg == "address")
            {
                StackListAddressXAML.IsVisible = Status;
               
            }
            else
            {
                WaitingXAML.IsVisible = Status;
               // WaitingXAML.IsVisible = Status;
            }
            return Retour;
        }



    }
}