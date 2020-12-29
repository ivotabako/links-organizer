using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LinksOrganizer.Tests.Utils
{
    public class NotifyPropertyChangedHarness
    {
        private readonly List<string> changes;

        public List<string> Changes => changes;

        public NotifyPropertyChangedHarness(INotifyPropertyChanged viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel", "Argument cannot be null.");
            }

            changes = new List<string>();

            viewModel.PropertyChanged += new PropertyChangedEventHandler(ViewModel_PropertyChanged);
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            changes.Add(e.PropertyName);
        }
    }
}
