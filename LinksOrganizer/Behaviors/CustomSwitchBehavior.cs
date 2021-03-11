using LinksOrganizer.CustomControls;
using LinksOrganizer.Utils;
using LinksOrganizer.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LinksOrganizer.Behaviors
{
    public class CustomSwitchBehavior : Behavior<CustomSwitch>
    {
        protected override void OnAttachedTo(CustomSwitch bindable)
        {
            bindable.PropertyChanged += Bindable_PropertyChanged;
           
            base.OnAttachedTo(bindable);
        }

        private async void Bindable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsToggled" && sender is CustomSwitch @switch && @switch.BindingContext is OptionsViewModel vm && vm != null)
            {
                await vm.InitializeAsync((@switch.IsToggled, GetChangeEvents(@switch.Name)));
            }
        }

        private ChangeEvents GetChangeEvents(string name)
        {
            return name switch
            {
                "UseClipboardChanged" => ChangeEvents.UseClipboardChanged,
                "ThemeChanged" => ChangeEvents.ThemeChanged,
                "OrderChanged" => ChangeEvents.OrderChanged,
                "SecureLinksChanged" => ChangeEvents.SecureLinksChanged,
                _ => ChangeEvents.None,
            };
        }

        protected override void OnDetachingFrom(CustomSwitch bindable)
        {
            bindable.PropertyChanged -= Bindable_PropertyChanged;
           
            base.OnDetachingFrom(bindable);
        }

    }
}
