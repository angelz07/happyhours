using happyhours.Fichiers;
using happyhours.Fichiers.Langues;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace happyhours.Class
{
    public class Tools
    {
        public string FindGoodLanguage() {
            string Retour = "";
            if (PrefsApp.langue == "Francais")
            {
                Retour = Language.Francais;
            }
            else if (PrefsApp.langue == "English")
            {
                Retour = Language.English;
            }

            return Retour;
        }

        public int ConvertStringToInt(string intString)
        {
            
            int i = 0;
            if (!Int32.TryParse(intString, out i))
            {
                i = -1;
            }
            return i;
        }

        public TimeSpan ConvertTimeStringToTime(string data)
        {
            var dateTimeToConvert = DateTime.ParseExact(data, "H:mm", null, System.Globalization.DateTimeStyles.None);

            TimeSpan timeConvert = new TimeSpan(dateTimeToConvert.Hour, dateTimeToConvert.Minute, 00);

            return timeConvert;
        }

        public string ConvertTime1Chiffre(int data)
        {
            string retour = "";
            if (data < 10)
            {
                retour = "0" + data;
            }
            else
            {
                retour = data.ToString();
            }

            return retour;
        }

    }
}
