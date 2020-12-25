using LinksOrganizer.Models;
using LinksOrganizer.ViewModels;
using Xamarin.Forms;

namespace LinksOrganizer.Behaviors
{
    public class ListViewBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            bindable.ItemTapped += Bindable_ItemTapped;
           
            base.OnAttachedTo(bindable);
        }

        private void Bindable_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && sender is ListView view)
            {
                var vm = view.BindingContext as StartPageViewModel;
                vm.LoadLinkItemCommand.Execute(e.Item as LinkItem);
            }
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            bindable.ItemTapped -= Bindable_ItemTapped;
           
            base.OnDetachingFrom(bindable);
        }

    }
}
