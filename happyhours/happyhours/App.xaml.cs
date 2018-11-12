using happyhours.Class;
using happyhours.Fichiers;
using happyhours.Fichiers.Langues;
using happyhours.Objets;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace happyhours
{
    public partial class App : Application
    {
        Tools Tools = new Tools();
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            string LangueSelect = Tools.FindGoodLanguage();
            VariablesGlobal.LanguageApp = (LanguageAppStruct)Newtonsoft.Json.JsonConvert.DeserializeObject(LangueSelect.ToString(), typeof(LanguageAppStruct));



            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
