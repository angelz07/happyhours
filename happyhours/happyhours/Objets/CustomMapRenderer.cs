using happyhours.Objets;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace happyhours.Class.Maps
{
    public class CustomMap : Map
    {
        public List<ListViewAddressJson> CustomPins { get; set; }
    }
}
