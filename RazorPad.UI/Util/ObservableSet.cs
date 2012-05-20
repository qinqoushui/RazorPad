using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RazorPad.UI.Util
{
    public class ObservableSet<T> : ObservableCollection<T>
    {
        public ObservableSet(IEnumerable<T> collection)
            : base(collection)
        {
        }

        protected override void InsertItem(int index, T item)
        {
            if(Contains(item))
                return;

            base.InsertItem(index, item);
        }
    }
}
