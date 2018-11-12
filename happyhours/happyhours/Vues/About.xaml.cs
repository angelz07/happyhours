using happyhours.Class;
using happyhours.Fichiers;
using happyhours.Objets;
using happyhours.Vues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace happyhours.Vues
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class About : ContentPage
	{
		public About ()
		{
			InitializeComponent ();

        }

      

        private async void SendEmailAsync()
        {
            string AddressMail = PrefsApp.MailSupport;
            string SujetMail = "Demande de Support HappyHours";

            string BodyMail = "";

            // mailto: ryan.hatfield @test.com? subject = free candy & body = you can trust me
            string Url = "mailto:" + AddressMail + "?subject=" + SujetMail + "&body=" + BodyMail;
            Console.WriteLine("url:" + Url);

            Boolean answer = await DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ConfirmEnvoieMailSupport, VariablesGlobal.LanguageApp.Oui, VariablesGlobal.LanguageApp.Non);
            if (answer == true)
            {
                Device.OpenUri(new Uri(Url));
            }
        }

        private void Support_Clicked(object sender, EventArgs e)
        {
            SendEmailAsync();
        }
    }
}