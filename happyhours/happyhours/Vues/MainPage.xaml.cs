using happyhours.Class;
using happyhours.Class.DependencyService;
using happyhours.Fichiers;
using happyhours.Objets;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace happyhours
{
    public partial class MainPage : ContentPage
    {
        ToolBars ToolBars = new ToolBars();
        ToolsGps ToolsGps = new ToolsGps();

        public MainPage()
        {
            InitializeComponent();
            ToolBars.MenuMainPage(this, Navigation);


           

        }

       

        protected override void OnAppearing()
        {
           
            base.OnAppearing();

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert(VariablesGlobal.LanguageApp.Infos, VariablesGlobal.LanguageApp.ProblemConnexionInternet, VariablesGlobal.LanguageApp.Ok);
                // your logic...  
            }
            


        }

        


        public void ShowMessage(string Titre, string Messsage, string Boutton)
        {
            Device.BeginInvokeOnMainThread(async () => { await DisplayAlert(Titre, Messsage, Boutton); });
        }
    }
}
