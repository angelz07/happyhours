using System;
using System.Collections.Generic;
using System.Text;

namespace happyhours.Objets
{
    public class ListAdresse
    {

        public class ListViewAddressStruct
        {
            

            public int Id { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Rue { get; set; }
            public string Numero { get; set; }
            public string CodePostal { get; set; }
            public string Pays { get; set; }
            public string Ville { get; set; }
        }

       // public static ListViewAddressStruct ListViewAddressJson;
    }
}
