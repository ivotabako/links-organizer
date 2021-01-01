using LinksOrganizer.Models;
using LinksOrganizer.ViewModels;
using Xamarin.Forms;

namespace LinksOrganizer.Behaviors
{
    public class SearchEntryBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += Bindable_TextChanged;
           
            base.OnAttachedTo(bindable);
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null && sender is Entry view)
            {
                var vm = view.BindingContext as StartPageViewModel;
                vm.SetFavoriteLinksItemsCommand.Execute(e.NewTextValue);
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= Bindable_TextChanged;
           
            base.OnDetachingFrom(bindable);
        }

    }
}
