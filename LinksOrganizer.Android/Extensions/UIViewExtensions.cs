using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
using Android.OS;
using LinksOrganizer.Extensions;
using LinksOrganizer.CustomControls;

namespace LinksOrganizer.Extensions
{
    public static class UIViewExtensions
    {
        public static void InitializeFrom(this View nativeControl, RoundedBoxView formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            var background = new GradientDrawable();

            background.SetColor(formsControl.BackgroundColor.ToAndroid());          
            nativeControl.Background = background;
            
            nativeControl.UpdateCornerRadius(formsControl.CornerRadius.TopLeft);
            nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
        }

        public static void UpdateFrom(this View nativeControl, RoundedBoxView formsControl,
          string propertyChanged)
        {
            if (nativeControl == null || formsControl == null)
                return;

            if (propertyChanged == RoundedBoxView.CornerRadiusProperty.PropertyName)
            {
                nativeControl.UpdateCornerRadius(formsControl.CornerRadius.TopLeft);
            }
            if (propertyChanged == VisualElement.BackgroundColorProperty.PropertyName)
            {
                if (nativeControl.Background is GradientDrawable background)
                {
                    background.SetColor(formsControl.BackgroundColor.ToAndroid());
                }
            }

            if (propertyChanged == RoundedBoxView.BorderColorProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }

            if (propertyChanged == RoundedBoxView.BorderThicknessProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }
        }

        public static void UpdateBorder(this View nativeControl, Color color, int thickness)
        {
            if (nativeControl.Background is GradientDrawable backgroundGradient)
            {
                var relativeBorderThickness = thickness * 3;
                backgroundGradient.SetStroke(relativeBorderThickness, color.ToAndroid());
            }
        }

        public static void UpdateCornerRadius(this View nativeControl, double cornerRadius)
        {
            if (nativeControl.Background is GradientDrawable backgroundGradient)
            {
                var relativeCornerRadius = (float)(cornerRadius * 3.7);
                backgroundGradient.SetCornerRadius(relativeCornerRadius);
            }
        }
    }
}
