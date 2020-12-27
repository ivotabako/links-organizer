using LinksOrganizer.Models;
using LinksOrganizer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LinksOrganizer.Behaviors
{
    public class SwitchBehavior : Behavior<Switch>
    {
        protected override void OnAttachedTo(Switch bindable)
        {
            bindable.PropertyChanged += Bindable_PropertyChanged;
           
            base.OnAttachedTo(bindable);
        }

        private async void Bindable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsToggled" && sender is Switch @switch)
            {
                var vm = @switch.BindingContext as StartPageViewModel;
                await vm.InitializeAsync(@switch.IsToggled);
            }
        }

        protected override void OnDetachingFrom(Switch bindable)
        {
            bindable.PropertyChanged -= Bindable_PropertyChanged;
           
            base.OnDetachingFrom(bindable);
        }

    }
}
