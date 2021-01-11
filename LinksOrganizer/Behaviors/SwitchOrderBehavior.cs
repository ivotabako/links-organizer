using LinksOrganizer.Utils;
using LinksOrganizer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LinksOrganizer.Behaviors
{
    public class SwitchOrderBehavior : Behavior<Switch>
    {
        protected override void OnAttachedTo(Switch bindable)
        {
            bindable.PropertyChanged += Bindable_PropertyChanged;
           
            base.OnAttachedTo(bindable);
        }

        private async void Bindable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsToggled" && sender is Switch @switch && @switch.BindingContext is OptionsViewModel vm && vm != null)
            {
                await vm.InitializeAsync((@switch.IsToggled, ChangeEvents.OrderChanged));
            }
        }

        protected override void OnDetachingFrom(Switch bindable)
        {
            bindable.PropertyChanged -= Bindable_PropertyChanged;
           
            base.OnDetachingFrom(bindable);
        }

    }
}
