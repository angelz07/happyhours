using System;
using System.Collections.Generic;
using System.Text;

namespace happyhours.Objets
{
    public class ReponseServeurAddPin
    {
        public struct ReponseServeurAddPinStruct
        {
            public string Status { get; set; }
            public string Type { get; set; }
            public string Retour { get; set; }

        }
    }
}
