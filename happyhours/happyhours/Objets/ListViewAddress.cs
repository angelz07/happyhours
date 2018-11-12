using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace happyhours.Objets
{
    public class ObservableGroupCollection<K, T> : ObservableCollection<T>
    {
        private readonly K _key;

        public ObservableGroupCollection(IGrouping<K, T> group)
            : base(group)
        {
            _key = group.Key;
        }

        public K Key
        {
            get { return _key; }
        }
    }
}
