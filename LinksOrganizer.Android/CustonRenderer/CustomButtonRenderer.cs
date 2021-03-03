using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content;
using LinksOrganizer.CustonRenderer;
using LinksOrganizer.CustomControls;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace LinksOrganizer.CustonRenderer
{
    class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Cleanup
            }

            if (e.NewElement != null && !e.NewElement.IsEnabled && e.NewElement is CustomButton btn)
            {
                var color = btn.DisabledTextColor;
                Control.SetTextColor(color.ToAndroid());                
            }
        }
    }
}
