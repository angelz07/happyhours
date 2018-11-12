using happyhours.Objets;
using System;
using System.Collections.Generic;
using System.Text;

namespace happyhours.Fichiers
{
    public class VariablesGlobal
    {
        public static bool IsGpsAuthorized { get; set; }
        public static LanguageAppStruct LanguageApp { get; set; }
        

        //public static bool et; set; }

        public static string Etablissement = "";
        public static int Type = -1;
        public static string Rue = "";
        public static string Numero = "";
        public static string CodePostal = "";
        public static string Ville = "";
        public static string Pays = "";
        public static string Url = "";
        public static string Promotion = "";
        public static TimeSpan HeureDebut = TimeSpan.Zero;
        public static TimeSpan HeureFin = TimeSpan.Zero;
        public static bool Lundi = false;
        public static bool Mardi = false;
        public static bool Mercredi = false;
        public static bool Jeudi = false;
        public static bool Vendredi = false;
        public static bool Samedi = false;
        public static bool Dimanche = false;
        public static string Autre = "";
        public static string Latitude = "";
        public static string Longitude = "";

        public static string[] TypeEtablissement = {
             "0-Bierre",
             "1-Cocktail",
             "2-Alcool",
             "3-Divers"
        };

        public static string IdMysqlSelect = "";
        public static string EtablissementSelect = "";
    }
}
