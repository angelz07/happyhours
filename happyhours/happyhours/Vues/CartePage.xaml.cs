using happyhours.Class;
using happyhours.Class.DependencyService;
using happyhours.Class.Maps;
using happyhours.Fichiers;
using happyhours.Objets;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace happyhours.Vues
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CartePage : ContentPage
	{
        ToolBars ToolBars = new ToolBars();
        ToolsGps ToolsGps = new ToolsGps();
        Tools Tools = new Tools();
        ToolsPins ToolsPins = new ToolsPins();

        public IGeolocator locator = CrossGeolocator.Current;
        public Map MapApplication;
        private Xamarin.Forms.Maps.Position _position;
        public ObservableCollection<InfosPinsClass.InfosPinStruct> InfosPinList = new ObservableCollection<InfosPinsClass.InfosPinStruct>();


        public CartePage ()
		{
			InitializeComponent();
            ToolBars.MenuCarte(this, Navigation);

            
        }

       

        protected override void OnAppearing()
        {

            base.OnAppearing();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ProblemConnexionInternet, VariablesGlobal.LanguageApp.Ok);
                // your logic...  
            }
            else {
                VariablesGlobal.IdMysqlSelect = "";
                VariablesGlobal.EtablissementSelect = "";

                bool IsLocation = ToolsGps.IsLocationAvailable();
                if (IsLocation == true)
                {
                    VariablesGlobal.IsGpsAuthorized = true;
                    FindPinsAsync();
                }
                else
                {
                    ActionGpsDesactiveAsync();
                }



            }



        }
        private async void ActionGpsDesactiveAsync()
        {
            var answer = await DisplayAlert(VariablesGlobal.LanguageApp.TitreActivationGps, VariablesGlobal.LanguageApp.QuestionActivationGps, VariablesGlobal.LanguageApp.Oui, VariablesGlobal.LanguageApp.Non);
            if (answer)
            {
                DependencyService.Get<ISettingsService>().OpenSettings();
                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                {

                    if (ToolsGps.IsLocationAvailable() == true)
                    {
                        //  Tools.AsyncHelper.RunSync(() => FindPinsAsync());
                        VariablesGlobal.IsGpsAuthorized = true;

                        FindPinsAsync();
                        return false;
                    }
                    return true;
                });
            }
            else
            {

                VariablesGlobal.IsGpsAuthorized = false;
                await DisplayAlert(VariablesGlobal.LanguageApp.TitreActivationGps, VariablesGlobal.LanguageApp.ReponseNegativeActivationGPS, VariablesGlobal.LanguageApp.Ok);
                FindPinsAsync();
            }

        }

        
        public async void FindPinsAsync()
        {
            using (HttpClient http = new HttpClient()) {
                try
                {
                   
                    string AddresseApi = PrefsApp.ApiAddress + "?api=" + PrefsApp.ApiKey + "&action=list-pins-all";
                    Console.WriteLine("**************** " + AddresseApi +"*************************");
                    var response = await http.GetAsync(AddresseApi);
                    if ((int)response.StatusCode == 200)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        try
                        {
                            StatusReq status = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusReq>(data);
                            if (status.status == true)
                            {
   //                            Console.WriteLine("status.message = " + status.message);
                                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(data).First.Next;
                                  Console.WriteLine("type = " + jsonObj.GetType());

                                DoCarte(jsonObj);
                               
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    
                }

            }
        }


        public async void GetPosition()
        {
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                var locatore = CrossGeolocator.Current;
                locatore.DesiredAccuracy = 100;

                position = await locatore.GetLastKnownLocationAsync();

                if (position != null)
                {
                    _position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
                    //got a cahched position, so let's use it.
                    return;
                }

                if (!locatore.IsGeolocationAvailable || !locatore.IsGeolocationEnabled)
                {
                    //not available or enabled
                   await DisplayAlert("Attention GPS Désactivé", "GPS désactivé, merci d'ativé votre GPS sur l'appareil.", "OK");
                    //   await DisplayAlert("Attention GPS Désativé", "GPS désactivé, merci d'ativé votre GPS sur l'appareil.", "OK");
                    return;
                }

                position = await locatore.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                throw ex;
                //Display error as we have timed out or can't get location.
            }
            _position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            if (position == null)
                return;

        }

        public void PinClicked(object sender, EventArgs e, ListViewAddressJson pin)
        {
            // ShowMessage("info", "click", "ok");
            var DetailPin = new DetailPin(pin);
            Navigation.PushAsync(DetailPin);

        }

        private void DoCarte(JToken jsonObj)
        {
            GetPosition();
            var map = new CustomMap
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(_position.Latitude, _position.Latitude), Distance.FromMiles(0.3));

            map.CustomPins = new List<ListViewAddressJson> { };
            Console.WriteLine("jsonObj.First() = " + jsonObj.First.ToString());
            foreach (var item in jsonObj.First) {
                Console.WriteLine("item.ToString() = " + item.ToString());
                InfosPinsClass.InfosPinStruct lignePin = (InfosPinsClass.InfosPinStruct)Newtonsoft.Json.JsonConvert.DeserializeObject(item.ToString(), typeof(InfosPinsClass.InfosPinStruct));
                int i = 0;
                Console.Write("InfosPinList.Count = " + InfosPinList.Count);
                /*if (InfosPinList.Count > 0)
                {
                    for (i = 0; i < InfosPinList.Count; i++)
                    {
                        Console.WriteLine("nb item = " + i);
                    }
                }*/
               

                InfosPinList.Insert(i, new InfosPinsClass.InfosPinStruct()
                {

                    Id = lignePin.Id,
                    TypeEtablissement = lignePin.TypeEtablissement,
                    Longitude = lignePin.Longitude,
                    Latitude = lignePin.Latitude,
                    Label = lignePin.Label,
                    Address = lignePin.Address,
                    Url = lignePin.Url,
                    Promo = lignePin.Promo,
                    HeurePromoDebut = lignePin.HeurePromoDebut,
                    HeurePromoFin = lignePin.HeurePromoFin,
                    Quand = lignePin.Quand,
                    Date_enregistrement = lignePin.Date_enregistrement,
                    Date_modification = lignePin.Date_modification,
                    Icon = lignePin.Icon,
                    UsersValidate = lignePin.UsersValidate,
                    UsersUnValidate = lignePin.UsersUnValidate


                });

                double LongitudeDouble = Convert.ToDouble(lignePin.Longitude, CultureInfo.GetCultureInfo("en-us"));
                double LatitudeDouble = Convert.ToDouble(lignePin.Latitude, CultureInfo.GetCultureInfo("en-us"));


              ///  DisplayAlert("info", "LongitudeDouble:" +  LongitudeDouble + " Latitude:" + LatitudeDouble, "ok");


                var pin = new ListViewAddressJson
                     {
                         Type = PinType.Place,
                     Position = new Xamarin.Forms.Maps.Position(Convert.ToDouble(lignePin.Latitude, CultureInfo.GetCultureInfo("en-us")), Convert.ToDouble(lignePin.Longitude, CultureInfo.GetCultureInfo("en-us"))),
                    IdMysql = lignePin.Id,
                         TypeEtablissement = lignePin.TypeEtablissement,
                         Longitude = lignePin.Longitude,
                         Latitude = lignePin.Latitude,
                         Label = lignePin.Label,
                         Address = lignePin.Address,
                         Url = lignePin.Url,
                         Promo = lignePin.Promo,
                         HeurePromoDebut = lignePin.HeurePromoDebut,
                         HeurePromoFin = lignePin.HeurePromoFin,
                         Quand = lignePin.Quand,
                         Date_enregistrement = lignePin.Date_enregistrement,
                         Date_modification = lignePin.Date_modification,
                         Icon = lignePin.Icon,
                         UsersValidate = lignePin.UsersValidate,
                         UsersUnValidate = lignePin.UsersUnValidate

                     };

                     pin.Clicked += (sender, e) => { PinClicked(sender, e, pin); };

                    map.CustomPins.Add(pin);
               
                   map.Pins.Add(pin);
                   
            }


           // DisplayAlert("info", map.Pins.Count.ToString(), "ok");

           map.MoveToRegion(MapSpan.FromCenterAndRadius(_position, Distance.FromMiles(1)));
            map.MapType = MapType.Street;
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;

        }
    }
}