using Android.App;
using Android.OS;
using Android.Content.PM;
using LinksOrganizer.Utils;
using Xamarin.Forms.Platform.Android;
using LinksOrganizer.Views;
using LinksOrganizer.ViewModels;

namespace LinksOrganizer
{
    [Activity(Label = "LinksOrganizer", Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class MainActivity :  global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Forms.FormsMaterial.Init(this, bundle);
            LoadApplication(new App());
        }

        public async override void OnBackPressed()
        {
            IBackButtonHandler backButtonHandler = null;
            ViewModelBase viewModel = null;
            foreach (var fragment in this.GetFragmentManager().Fragments)
            {
                var page = fragment.GetType().GetProperty("Page")?.GetValue(fragment);

                if (page is StartPageView)
                {
                    viewModel = (ViewModelBase)(page as StartPageView).BindingContext;
                    if(viewModel.NavigationService.PreviousPageViewModel is StartPageViewModel)
                        backButtonHandler = viewModel as IBackButtonHandler;
                }

                if (page is LinkItemView)
                {
                    viewModel = (ViewModelBase)(page as LinkItemView).BindingContext;
                    if (viewModel.NavigationService.PreviousPageViewModel is LinkItemViewModel)
                        backButtonHandler = viewModel as IBackButtonHandler;
                }

                if (page is OptionsView)
                {
                    viewModel = (ViewModelBase)(page as OptionsView).BindingContext;
                    if (viewModel.NavigationService.PreviousPageViewModel is OptionsViewModel)
                        backButtonHandler = viewModel as IBackButtonHandler;
                }

                if (backButtonHandler != null)
                    break;
            }

            var backButtonHandled = await backButtonHandler?.HandleBackButton(viewModel);
            if (!backButtonHandled)
            {
                base.OnBackPressed();
            }
        }
    }
}
