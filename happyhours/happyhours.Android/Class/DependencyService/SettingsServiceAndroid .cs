using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using happyhours.Class.DependencyService;
using happyhours.Droid.Class.DependencyService;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(SettingsServiceAndroid))]
namespace happyhours.Droid.Class.DependencyService
{
    public class SettingsServiceAndroid : ISettingsService
    {
       // private Context mContext;
       // private Activity mActivity;

        public void OpenSettings()
        {
            
            Context mContext =  VariablesAndroidGlobal.ContextApplication;

            Intent intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.SetData(Android.Net.Uri.Parse("package:" + mContext.PackageName));
          //  intent.AddFlags(ActivityFlags.ClearTop);
            intent.SetFlags(ActivityFlags.ReorderToFront);
            intent.SetFlags(ActivityFlags.ClearTask);
            // intent.AddFlags(ActivityFlags.NewTask);
            //intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.ClearTask | ActivityFlags.NewTask);

            //  intent.SetData(Android.Net.Uri.Parse(baseUrl + @"name_of_apk"));
            //  Android.App.Application.Context.StartActivity(intent);
            //new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings)
            //   Context context = Android.App.Application.Context;
            // myIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            mContext.StartActivity(intent);
        }
    }
}