using happyhours.Class;
using happyhours.Fichiers;
using happyhours.Objets;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static happyhours.Objets.InfosPinsClass;
using static happyhours.Objets.ListAdresse;
using static happyhours.Objets.ReponseServeurAddPin;

namespace happyhours.Vues
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AjouterElement : ContentPage
	{
        string MessageError = "";
        public List<ListViewAddressStruct> ListAddress;
        Tools Tools = new Tools();

        public AjouterElement ()
		{
			InitializeComponent ();
           
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
                TestIfExist();
            }

           


        }

        private void EnvoyerBoutton_Clicked(object sender, EventArgs e)
        {
            MessageError = "";
               bool IsCompleted = IsFormCompleted();

                if (IsCompleted == true)
                {
                    SendNewEtablissementAsync();
                }
                else {
                    DisplayAlert("infos", MessageError.ToString(), "ok");
                }
               
           
        }

        private async void SendNewEtablissementAsync()
        {

            
            try
            {
                string NewLongitude = LongitudeXAML.Text.Replace(",", ".");
                string NewLatitude = LatitudeXAML.Text.Replace(",", ".");
                string NewLabel = NomXAML.Text;

                string NewTypeEtablissement = TypeXAML.Items[TypeXAML.SelectedIndex];

                string NewRue = RueXAML.Text;
                string NewNumero = NumeroXAML.Text;
                string NewCodePostal = CodePostalXAML.Text;
                string NewVille = VilleXAML.Text;
                string NewPays = PaysXAML.Text;

                string NewAddress = NewRue + " " + NewNumero + ", " + NewCodePostal + " " + NewVille + " " + NewPays;


                string NewSiteInternet = "";
                if (UrlXAML.Text == "" || UrlXAML.Text == null)
                {
                    NewSiteInternet = "";
                }
                else {
                    NewSiteInternet = UrlXAML.Text;
                }

                string NewPromo = PromoXAML.Text;


               
                string NewHeureDebut = Tools.ConvertTime1Chiffre(HeureDebutXAML.Time.Hours) + ":" + Tools.ConvertTime1Chiffre(HeureDebutXAML.Time.Minutes);
                string NewHeureFin = Tools.ConvertTime1Chiffre(HeureFinXAML.Time.Hours) + ":" + Tools.ConvertTime1Chiffre(HeureFinXAML.Time.Minutes);


                string NewJours = "";
                if (AutreJourXAML.Text == "" || AutreJourXAML.Text == null)
                {
                    if (VariablesGlobal.Lundi == true) {
                        NewJours = NewJours + "Lundi -";
                    }
                    if (VariablesGlobal.Mardi == true)
                    {
                        NewJours = NewJours + "Mardi -";
                    }
                    if (VariablesGlobal.Mercredi == true)
                    {
                        NewJours = NewJours + "Mercredi -";
                    }
                    if (VariablesGlobal.Jeudi == true)
                    {
                        NewJours = NewJours + "Jeudi -";
                    }
                    if (VariablesGlobal.Vendredi == true)
                    {
                        NewJours = NewJours + "Vendredi -";
                    }
                    if (VariablesGlobal.Samedi == true)
                    {
                        NewJours = NewJours + "Samedi -";
                    }
                    if (VariablesGlobal.Dimanche == true)
                    {
                        NewJours = NewJours + "Dimanche -";
                    }
                }
                else {
                    NewJours = AutreJourXAML.Text;
                }


                DateTime Today = DateTime.Now;

                Console.WriteLine("Today is " + Today.ToString("dd/MM/yyyy"));

                


                InfosPinStruct InfosNewPin = new InfosPinStruct
                {
                    Id = "",
                    TypeEtablissement = NewTypeEtablissement,
                    Longitude = NewLongitude,
                    Latitude = NewLatitude,
                    Label = NewLabel,
                    Address = NewAddress,
                    Url = NewSiteInternet,
                    Promo = NewPromo,
                    HeurePromoDebut = NewHeureDebut,
                    HeurePromoFin = NewHeureFin,
                    Quand = NewJours,

                    Date_enregistrement = Today.ToString("dd/MM/yyyy"),
                    Date_modification = Today.ToString("dd/MM/yyyy"),
                    Icon = NewTypeEtablissement + ".png",
                    UsersValidate = "0",
                    UsersUnValidate = "0"
                };


                string json = JsonConvert.SerializeObject(InfosNewPin);
                string UrlApi = PrefsApp.ApiAddressPin ;

                Boolean answer = await DisplayAlert(VariablesGlobal.LanguageApp.ConfirmTitre, VariablesGlobal.LanguageApp.QuestionConfirmAddPin, VariablesGlobal.LanguageApp.Oui, VariablesGlobal.LanguageApp.Non);
                if (answer == true) {
                    string isRecordedOnline = PostBasicAsync(json, UrlApi);
                    ReponseServeurAddPinStruct result = JsonConvert.DeserializeObject<ReponseServeurAddPinStruct>(isRecordedOnline);
                    if (result.Status != "OK")
                    {
                        if (result.Retour != null && result.Retour != "")
                        {
                            await DisplayAlert(VariablesGlobal.LanguageApp.Infos, result.Retour, VariablesGlobal.LanguageApp.Ok);
                        }
                        else
                        {
                            await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ErreurGeneral, VariablesGlobal.LanguageApp.Ok);
                        }

                    }
                    else
                    {
                        await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.MessagePinAddOK, VariablesGlobal.LanguageApp.Ok);
                        await Navigation.PushAsync(new CartePage());
                    }

                }





            }
            catch (Exception e)
            {

                await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ErreurGeneral + ":" + e.Message, VariablesGlobal.LanguageApp.Ok);
            }
                
          
            /*     Connexion db ... */




        }



        private string PostBasicAsync(string contentData, string Url)
        {
            string Retour = "";
            string ApiKey = PrefsApp.ApiKey;
         

            string DataSend = "{\"apikey\": \"" + ApiKey + "\", \"data\": " + contentData + "}";
            //, \"iduser\": \"" + IdUser + "\"
            Console.Write("DataSend : " + DataSend);

            var content = new StringContent(JsonConvert.SerializeObject(DataSend));


            var request = (HttpWebRequest)WebRequest.Create(Url);
       //     request.ProtocolVersion = HttpVersion.Version11;
            //request.ContentType = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Accept = "application/json";
            request.KeepAlive = true;

            Console.Write("Url : " + Url);

     

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(DataSend);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.Write("result : " + result);
                   // DisplayAlert("infos", result , "ok");
                    Retour = result;
                    return Retour;

                }
            }
            catch (WebException ex)
            {

             //   DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ErreurGeneral + ":" + ex.Message, VariablesGlobal.LanguageApp.Ok);
                return ex.Message;
            }
           


        }




        private bool IsFormCompleted()
        {
            bool Retour = true;

            bool IsTextComplete = IsTextBoxCompleted();
            bool IsTimeComplete = IsTimeCompleted();
            bool IsDaysComplete = IsDaysCompleted();

            bool IsTypeCompete = true;

            if (TypeXAML.SelectedIndex < 0 )
            {
                IsTypeCompete = false;
                MessageError = MessageError + "- Type manquant \n";
            }


            if (IsTextComplete == true && IsTimeComplete == true && IsDaysComplete == true && IsTypeCompete == true)
            {
                Retour = true;

            }
            else {
                Retour = false;
            }

            return Retour;
        }

        private bool IsDaysCompleted()
        {
            bool Retour = true;

            bool IsAutre = false;
            if (AutreJourXAML.Text == "" || AutreJourXAML.Text == null)
            {
                IsAutre = false;
            }
            else {
                IsAutre = true;
            }

            bool IsSwitch = false;
            if (VariablesGlobal.Lundi == true)
            {
                IsSwitch = true;
            }
            

            if (VariablesGlobal.Mardi == true)
            {
                IsSwitch = true;
            }
           

            if (VariablesGlobal.Mercredi == true)
            {
                IsSwitch = true;
            }
          

            if (VariablesGlobal.Jeudi == true)
            {
                IsSwitch = true;
            }
         

            if (VariablesGlobal.Vendredi == true)
            {
                IsSwitch = true;
            }
          

            if (VariablesGlobal.Samedi == true)
            {
                IsSwitch = true;
            }
           

            if (VariablesGlobal.Dimanche == true)
            {
                IsSwitch = true;
            }
          



           // DisplayAlert("infos", "IsAutre: " + IsAutre.ToString() + "  IsSwitch:" + IsSwitch.ToString(), "ok");
            if (IsAutre == false && IsSwitch == false)
            {
                MessageError = MessageError + "- " + VariablesGlobal.LanguageApp.MessageErreurIsAutreAndIsJoursFalse + "\n";

                Retour = false;
            }


            if (IsAutre == true && IsSwitch == true)
            {
                MessageError = MessageError + "- " + VariablesGlobal.LanguageApp.MessageErreurIsAutreAndIsJoursTrue + "\n";

                Retour = false;
            }

            return Retour;
        }

        private bool IsTimeCompleted()
        {
            bool Retour = true;

            if (VariablesGlobal.HeureDebut == null || VariablesGlobal.HeureDebut == TimeSpan.Zero)
            {
                Retour = false;
                MessageError = MessageError + "- Heure début manquante \n";
            }

            if (VariablesGlobal.HeureFin == null || VariablesGlobal.HeureFin == TimeSpan.Zero)
            {
                Retour = false;
                MessageError = MessageError + "- Heure fin manquante \n";
            }

            return Retour;
        }

        private bool IsTextBoxCompleted()
        {
            bool Retour = true;
            if (LongitudeXAML.Text == "" || LongitudeXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Longitude manquante \n";
            }


            if (LatitudeXAML.Text == "" || LatitudeXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Latitude manquante \n";
            }


            
            if (NomXAML.Text == "" || NomXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Etablissement manquant \n";
            }

            if (RueXAML.Text == "" || RueXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Rue manquante \n";
            }

            if (NumeroXAML.Text == "" || NumeroXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Numéro manquant \n";
            }

            if (CodePostalXAML.Text == "" || CodePostalXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Code Postal manquant \n";
            }

            if (VilleXAML.Text == "" || VilleXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Ville manquante \n";
            }

            if (PaysXAML.Text == "" || PaysXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Pays manquant \n";
            }

       /*     if (UrlXAML.Text == "" || UrlXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Site web manquant\n";
            }
            */
            if (PromoXAML.Text == "" || PromoXAML.Text == null)
            {
                Retour = false;
                MessageError = MessageError + "- Promo manquante \n";
            }

            return Retour;
        }

        private void MyPositionBoutton_Clicked(object sender, EventArgs e)
        {
            GotoListAddressAsync();
            
          //  Navigation.PushAsync(new ListAddress());
        }

        private async void GotoListAddressAsync()
        {
            Boolean answer = await DisplayAlert(VariablesGlobal.LanguageApp.ConfirmTitre, VariablesGlobal.LanguageApp.QuestionConfirmRechercheAddresse, VariablesGlobal.LanguageApp.Oui, VariablesGlobal.LanguageApp.Non);
            if (answer == true) {
                var page = new ListAddress();

                await Navigation.PushAsync(page);
            }
        }

        private void NomXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Etablissement = e.NewTextValue;
        }

        private void RueXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Rue = e.NewTextValue;
        }

        private void NumeroXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Numero = e.NewTextValue;
        }

        private void CodePostalXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.CodePostal = e.NewTextValue;
        }

        private void VilleXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Ville = e.NewTextValue;
        }

        private void PaysXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Pays = e.NewTextValue;
        }

        private void UrlXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Url = e.NewTextValue;
        }

        private void PromoXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Promotion = e.NewTextValue;
        }

        private void AutreJourXAML_TextChanged(object sender, TextChangedEventArgs e)
        {
            VariablesGlobal.Autre = e.NewTextValue;
        }

        private void HeureFinXAML_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("heure fin : " + HeureFinXAML.Time.Hours);
            VariablesGlobal.HeureFin = HeureFinXAML.Time;
        }

        private void HeureDebutXAML_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            VariablesGlobal.HeureDebut = HeureDebutXAML.Time;
          
        }



        private void TypeXAML_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeXAML.SelectedIndex != -1)
            {
               VariablesGlobal.Type = TypeXAML.SelectedIndex;
            }
        }

        private void LundiXAML_Toggled(object sender, ToggledEventArgs e)
        {
            bool isTToggel = e.Value;
            VariablesGlobal.Lundi = isTToggel;
        }

        private void MardiXAML_Toggled(object sender, ToggledEventArgs e)
        {
            bool isTToggel = e.Value;
            VariablesGlobal.Mardi = isTToggel;
        }

        private void MercrediXAML_Toggled(object sender, ToggledEventArgs e)
        {
            bool isTToggel = e.Value;
            VariablesGlobal.Mercredi = isTToggel;
        }

        private void JeudiXAML_Toggled(object sender, ToggledEventArgs e)
        {
            bool isTToggel = e.Value;
            VariablesGlobal.Jeudi = isTToggel;
        }

        private void VendrediXAML_Toggled(object sender, ToggledEventArgs e)
        {
            bool isTToggel = e.Value;
            VariablesGlobal.Vendredi = isTToggel;
        }

        private void SamediXAML_Toggled(object sender, ToggledEventArgs e)
        {
            bool isTToggel = e.Value;
            VariablesGlobal.Samedi = isTToggel;
        }

        private void DimancheXAML_Toggled(object sender, ToggledEventArgs e)
        {
            bool isTToggel = e.Value;
            VariablesGlobal.Dimanche = isTToggel;
        }


       private void TestIfExist() {
            

            if (VariablesGlobal.Etablissement != "" && VariablesGlobal.Etablissement != null)
            {
                NomXAML.Text = VariablesGlobal.Etablissement;
            }

            if (VariablesGlobal.Type != -1 && VariablesGlobal.Type > -1)
            {
                TypeXAML.SelectedIndex = VariablesGlobal.Type;

                /*  for (int i = 0; i < VariablesGlobal.TypeEtablissement.Length; i++)
                  {
                      string[] words = VariablesGlobal.TypeEtablissement[i].Split('-');
                      //   DisplayAlert("infos", "words[0].ToString() = " + words[0].ToString(), "ok");

                      if (VariablesGlobal.Type.ToString() == words[0])
                      {
                          DisplayAlert("infos", "words[0].ToString() = " + words[0].ToString() + " VariablesGlobal.Type.ToString()  = " + VariablesGlobal.Type.ToString(), "ok");

                      }

                  }*/

            }

            if (VariablesGlobal.Longitude != "" && VariablesGlobal.Longitude != null)
            {
                LongitudeXAML.Text = VariablesGlobal.Longitude;
            }

            if (VariablesGlobal.Latitude != "" && VariablesGlobal.Latitude != null)
            {
                LatitudeXAML.Text = VariablesGlobal.Latitude;
            }

            if (VariablesGlobal.Rue != "" && VariablesGlobal.Rue != null)
            {
                RueXAML.Text = VariablesGlobal.Rue;
            }

            if (VariablesGlobal.Numero != "" && VariablesGlobal.Numero != null)
            {
                NumeroXAML.Text = VariablesGlobal.Numero;
            }

            if (VariablesGlobal.CodePostal != "" && VariablesGlobal.CodePostal != null)
            {
                CodePostalXAML.Text = VariablesGlobal.CodePostal;
            }

            if (VariablesGlobal.Ville != "" && VariablesGlobal.Ville != null)
            {
                VilleXAML.Text = VariablesGlobal.Ville;
            }

            if (VariablesGlobal.Pays != "" && VariablesGlobal.CodePostal != null)
            {
                PaysXAML.Text = VariablesGlobal.Pays;
            }

            if (VariablesGlobal.Ville != "" && VariablesGlobal.Ville != null)
            {
                VilleXAML.Text = VariablesGlobal.Ville;
            }

            if (VariablesGlobal.Url != "" && VariablesGlobal.Url != null)
            {
                UrlXAML.Text = VariablesGlobal.Url;
            }

            if (VariablesGlobal.Promotion != "" && VariablesGlobal.Promotion != null)
            {
                PromoXAML.Text = VariablesGlobal.Promotion;
            }

            if (VariablesGlobal.HeureDebut != null)
            {
                HeureDebutXAML.Time = VariablesGlobal.HeureDebut;
            }

            if (VariablesGlobal.HeureFin != null)
            {
                HeureFinXAML.Time = VariablesGlobal.HeureFin;
            }

            if (VariablesGlobal.Lundi != false)
            {
                LundiXAML.IsEnabled = VariablesGlobal.Lundi;
            }

            if (VariablesGlobal.Mardi != false)
            {
                MardiXAML.IsEnabled = VariablesGlobal.Mardi;
            }

            if (VariablesGlobal.Mercredi != false)
            {
                MercrediXAML.IsEnabled = VariablesGlobal.Mercredi;
            }

            if (VariablesGlobal.Jeudi != false)
            {
                JeudiXAML.IsEnabled = VariablesGlobal.Jeudi;
            }

            if (VariablesGlobal.Vendredi != false)
            {
                VendrediXAML.IsEnabled = VariablesGlobal.Vendredi;
            }

            if (VariablesGlobal.Samedi != false)
            {
                SamediXAML.IsEnabled = VariablesGlobal.Samedi;
            }

            if (VariablesGlobal.Dimanche != false)
            {
                DimancheXAML.IsEnabled = VariablesGlobal.Dimanche;
            }

            if (VariablesGlobal.Autre != "" && VariablesGlobal.Autre != null)
            {
                AutreJourXAML.Text = VariablesGlobal.Autre;
            }
           
        }
    }
}