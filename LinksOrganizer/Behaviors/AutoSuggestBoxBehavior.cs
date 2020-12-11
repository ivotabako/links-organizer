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
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var vm = this.BindingContext as StartPageViewModel;

                if (sender is AutoSuggestBox box)
                {
                    vm.SetSearchedLinkItemsCommand.Execute(box.Text);
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                var vm = this.BindingContext as StartPageViewModel;
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
