using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace happyhours.Objets
{
    public class ListViewAddressJson : Pin
    {
        public string IdMysql { get; set; }
        public string TypeEtablissement { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
     //   public string Label { get; set; }
     //   public string Address { get; set; }
        public string Url { get; set; }
        public string Promo { get; set; }
        public string HeurePromoDebut { get; set; }
        public string HeurePromoFin { get; set; }
        public string Quand { get; set; }
        // public string user_xamarin { get; set; }
        //  public string religion { get; set; }
        public string Date_enregistrement { get; set; }
        public string Date_modification { get; set; }
        public string Icon { get; set; }
        public string UsersValidate { get; set; }
        public string UsersUnValidate { get; set; }
    }
}
