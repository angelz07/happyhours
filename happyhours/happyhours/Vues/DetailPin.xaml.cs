using happyhours.Class;
using happyhours.Fichiers;
using happyhours.Objets;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace happyhours.Vues
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPin : ContentPage
	{
        Tools Tools = new Tools();

		public DetailPin (ListViewAddressJson Pin)
		{
			InitializeComponent ();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ProblemConnexionInternet, VariablesGlobal.LanguageApp.Ok);
                // your logic...  
            }
            else
            {
                VariablesGlobal.IdMysqlSelect = Pin.IdMysql;
                VariablesGlobal.EtablissementSelect = Pin.Label;

                var tapGestureRecognizerLink = new TapGestureRecognizer();
                tapGestureRecognizerLink.Tapped += (s, e) => {
                    Device.OpenUri(new Uri(Pin.Url));
                };


                var tapGestureRecognizerOK = new TapGestureRecognizer();
                tapGestureRecognizerOK.Tapped += (s, e) => {
                    // Device.OpenUri(new Uri(Pin.Url));
                    SendNoteAsync("OK", Pin);

                };

                var tapGestureRecognizerNOTOK = new TapGestureRecognizer();
                tapGestureRecognizerNOTOK.Tapped += (s, e) => {
                    // Device.OpenUri(new Uri(Pin.Url));

                    SendNoteAsync("NOTOK", Pin);
                };





                NomEtablissementXAML.Text = Pin.Label;
                IconXAML.Source = ImageSource.FromFile(Pin.Icon);

                // icon_IdXaml.Source = ImageSource.FromUri(new Uri(Configuration.ServerImages + "images/" + Pin.Icon));
                SiteWebXAML.Source = Pin.Url;

                //  TypeEtablissementXAML.Text = Pin.TypeEtablissement;
                //   LongitudeXAML.Text = Pin.Longitude;
                //   LatitudeXAML.Text = Pin.Latitude;
                //   LabelXAML.Text = Pin.Label;
                AddressXAML.Text = Pin.Address;


                UrlXAML.Text = Pin.Url;
                UrlXAML.GestureRecognizers.Add(tapGestureRecognizerLink);


                PromoXAML.Text = Pin.Promo;

                HeuresXAML.Text = Pin.HeurePromoDebut + " jusqu'à " + Pin.HeurePromoFin;
                //   HeurePromoDebutXAML.Text = Pin.HeurePromoDebut;
                //   HeurePromoFinXAML.Text = Pin.HeurePromoFin;

                string Jours = "";
                if (Pin.Quand.IndexOf("|") != -1)
                {
                    string[] words = Pin.Quand.Split('|');
                    int ICount = 0;
                    foreach (string word in words)
                    {
                        ICount = ICount + 1;
                    }

                    int ITemp = 0;
                    foreach (string word in words)
                    {
                        ITemp = ITemp + 1;
                        if (ITemp < ICount)
                        {
                            Jours = Jours + word + " - ";
                        }
                        else
                        {
                            Jours = Jours + word;
                        }



                        // Console.WriteLine(word);
                    }
                }
                else
                {
                    Jours = Pin.Quand;
                }

                QuandXAML.Text = Jours;

                Date_enregistrementXAML.Text = Pin.Date_enregistrement;
                Date_modificationXAML.Text = Pin.Date_modification;
                // IconLabelXAML.Text = Pin.Icon;


                UsersValidateXAML.Text = Pin.UsersValidate;
                ImageOk.GestureRecognizers.Add(tapGestureRecognizerOK);

                UsersUnValidateXAML.Text = Pin.UsersUnValidate;
                ImageNotok.GestureRecognizers.Add(tapGestureRecognizerNOTOK);
            }


           

        }

        private async void SendNoteAsync(string Arg, ListViewAddressJson pin)
        {
            // DisplayAlert("Infos", Arg, "OK");
            if (Arg == "OK")
            {
                Boolean answer = await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.QuestionVoteOK, VariablesGlobal.LanguageApp.Oui, VariablesGlobal.LanguageApp.Non);
                if (answer == true)
                {
                    // DisplayAlert("Infos", "Super", "OK");
                    int value =  Tools.ConvertStringToInt(pin.UsersValidate) + 1;

                    DateTime Today = DateTime.Now;

                    string Date_modification = Today.ToString("dd/MM/yyyy");
                    string AddresseApi = PrefsApp.ApiAddress + "?api=" + PrefsApp.ApiKey + "&action=updateOK" + "&id=" + pin.IdMysql + "&value=" + value + "&Date_modification=" + Date_modification;
                    Console.WriteLine("AddresseApi=" + AddresseApi);
                    SendUpdate(AddresseApi, Date_modification);
                }
            }
            else {
                Boolean answer = await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.QuestionVoteNOTOK, VariablesGlobal.LanguageApp.Oui, VariablesGlobal.LanguageApp.Non);
                if (answer == true)
                {
                    //await DisplayAlert("Infos", "Bof", "OK");
                    int value = Tools.ConvertStringToInt(pin.UsersUnValidate) + 1;
                    DateTime Today = DateTime.Now;

                    string Date_modification = Today.ToString("dd/MM/yyyy");

                    string AddresseApi = PrefsApp.ApiAddress + "?api=" + PrefsApp.ApiKey + "&action=updateNOTOK" + "&id=" + pin.IdMysql + "&value=" + value + "&Date_modification=" + Date_modification;
                    Console.WriteLine("AddresseApi=" + AddresseApi);
                    SendUpdate(AddresseApi, Date_modification);
                }
            }
        }

        public async void SendUpdate(string AddresseApi, string Date_modification)
        {
            using (HttpClient http = new HttpClient())
            {
                try
                {

                   // string AddresseApi = PrefsApp.ApiAddress + "?api=" + PrefsApp.ApiKey + "&action=list-pins-all";
                   // Console.WriteLine("**************** " + AddresseApi + "*************************");
                    var response = await http.GetAsync(AddresseApi);
                    if ((int)response.StatusCode == 200)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        try
                        {
                           // Console.WriteLine("**************** " + data + "*************************");
                            StatusReq status = Newtonsoft.Json.JsonConvert.DeserializeObject<StatusReq>(data);
                            if (status.status == true)
                            {
                                //                            Console.WriteLine("status.message = " + status.message);
                                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(data).First.Next;
                               // Console.WriteLine("type = " + jsonObj.GetType());

                                PostActionNote(jsonObj, Date_modification);

                            }
                            else { }
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

        private void PostActionNote(JToken jsonObj, string Date_modification)
        {
          //  Console.WriteLine("jsonObj.First() = " + jsonObj.First.ToString());
            ReponseServeurNotes.ReponseServeurUpdateNoteStruct ReponseUpdateNote = (ReponseServeurNotes.ReponseServeurUpdateNoteStruct)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonObj.First.ToString(), typeof(ReponseServeurNotes.ReponseServeurUpdateNoteStruct));
            if (ReponseUpdateNote.Type == "OK")
            {
                UsersValidateXAML.Text = ReponseUpdateNote.Value;
                
            }
            else {
                UsersUnValidateXAML.Text = ReponseUpdateNote.Value;
            }

            Date_modificationXAML.Text = Date_modification;

            DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.MerciVote, VariablesGlobal.LanguageApp.Ok);
        }

       

        private void BouttonDemandeModification_Clicked(object sender, EventArgs e)
        {

            //    VariablesGlobal.IdMysqlSelect = Pin.IdMysql;
            //     VariablesGlobal.EtablissementSelect = Pin.Label;
            string AddressMail = PrefsApp.MailSupport;
            string SujetMail = "Demande de modification pour l'établissement: " + VariablesGlobal.EtablissementSelect + " avec l'id:" + VariablesGlobal.IdMysqlSelect;

            string BodyMail = "Id: " + VariablesGlobal.IdMysqlSelect + "\n";
            BodyMail = BodyMail + "Etablissement: " + VariablesGlobal.EtablissementSelect + "\n";
            BodyMail = BodyMail + "\n";
            BodyMail = BodyMail + "--- Tapez votre demande ici s'il vous plait ---";
                       // mailto: ryan.hatfield @test.com? subject = free candy & body = you can trust me
            string Url = "mailto:"+ AddressMail +"?subject=" + SujetMail + "&body=" + BodyMail;
            Console.WriteLine("url:" + Url);
            Device.OpenUri(new Uri(Url));

        }
    }
}