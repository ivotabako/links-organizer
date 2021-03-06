using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            set 
            {
                
                SetValue(DisabledTextColorProperty, value);
            }
        }

        public static readonly BindableProperty EnabledTextColorProperty = BindableProperty.Create(nameof(EnabledTextColor), typeof(Color), typeof(Button), Color.Default);

        public Color EnabledTextColor
        {
            get { return (Color)GetValue(EnabledTextColorProperty); }
            set
            {

                SetValue(EnabledTextColorProperty, value);
            }
        }

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (string.Equals(propertyName, "IsEnabled", StringComparison.CurrentCultureIgnoreCase))
            {
                if (this.IsEnabled)
                {
                    this.TextColor = EnabledTextColor;
                }
                if (!this.IsEnabled)
                {
                    this.TextColor = DisabledTextColor;
                }
            }
        }

        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
        }
    }
}
