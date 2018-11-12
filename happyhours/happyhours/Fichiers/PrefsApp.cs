using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace happyhours.Fichiers
{
    public class PrefsApp
    {
        public static string fileUserPref = "preferences.json";
        public static IFormatProvider cultureApp = new CultureInfo("fr-FR", true);

        public static string ApiAddress = "http://web2.telecom4all.be/happy_hours/www/api.php";
        public static string ApiAddressPin = "http://web2.telecom4all.be/happy_hours/www/pin.php";
        public static string ApiKey = "f2f833-cf0e32-c1df0b-78ea82-c3c38f";
        //http://memories.ovh/api-happy-hours/api.php?api=84bdba-d214e3-38c29e-701e1e-1634ed&action=list-pins-all
        public static string langue = "Francais";
        public static string MailSupport = "infos@telecom4all.be";
    }
}
