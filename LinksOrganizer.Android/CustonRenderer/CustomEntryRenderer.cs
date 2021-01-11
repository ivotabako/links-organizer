using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content;
using LinksOrganizer.CustonRenderer;
using LinksOrganizer.CustomControls;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace LinksOrganizer.CustonRenderer
{
    class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}
