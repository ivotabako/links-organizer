using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content;
using LinksOrganizer.CustonRenderer;
using LinksOrganizer.CustomControls;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace LinksOrganizer.CustonRenderer
{
    class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ( sender is CustomButton btn)
            {
                if (btn.IsEnabled)
                {

                var color = btn.EnabledTextColor;
                Control.SetTextColor(color.ToAndroid());
                }
                else
                {

                var color = btn.DisabledTextColor;
                Control.SetTextColor(color.ToAndroid());
           
                }
            }
        }
    }
}
