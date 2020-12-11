using dotMorten.Xamarin.Forms;
using LinksOrganizer.Models;
using LinksOrganizer.ViewModels;
using Xamarin.Forms;

namespace LinksOrganizer.Behaviors
{
    public class AutoSuggestBoxBehavior : Behavior<AutoSuggestBox>
    {
        protected override void OnAttachedTo(AutoSuggestBox bindable)
        {
            bindable.TextChanged += AutoSuggestBox_TextChanged;
            bindable.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
            base.OnAttachedTo(bindable);
        }

        private void AutoSuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender is AutoSuggestBox box)
            {             
                var vm = box.BindingContext as StartPageViewModel;
                vm.SetSearchedLinkItemsCommand.Execute(box.Text);              
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && sender is AutoSuggestBox box)
            {
                var vm = box.BindingContext as StartPageViewModel;
                vm.LoadLinkItemCommand.Execute(args.ChosenSuggestion as LinkItem);
            }
        }

        protected override void OnDetachingFrom(AutoSuggestBox bindable)
        {
            bindable.TextChanged -= AutoSuggestBox_TextChanged;
            bindable.QuerySubmitted -= AutoSuggestBox_QuerySubmitted;
            base.OnDetachingFrom(bindable);
        }

    }
}
