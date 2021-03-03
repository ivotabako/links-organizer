using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LinksOrganizer.CustomControls
{
    public class CustomButton : Button
    {
        public static readonly BindableProperty DisabledTextColorProperty = BindableProperty.Create(nameof(DisabledTextColor), typeof(Color), typeof(Button), Color.Default);

        public Color DisabledTextColor
        {
            get { return (Color)GetValue(DisabledTextColorProperty); }
            set { SetValue(DisabledTextColorProperty, value); }
        }
    }
}
