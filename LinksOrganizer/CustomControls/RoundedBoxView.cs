using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LinksOrganizer.CustomControls
{
	public class RoundedBoxView : BoxView
	{      
        /// <summary>
        /// Thickness property of border
        /// </summary>

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(
                    nameof(BorderThickness),
                    typeof(int),
                    typeof(RoundedBoxView),
                    default(int));

        /// <summary>
        /// Border thickness of circle image
        /// </summary>

        public int BorderThickness
		{
			get { return (int)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}

        /// <summary>
        /// Color property of border
        /// </summary>
       
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(
                    nameof(BorderColor),
                    typeof(Color),
                    typeof(RoundedBoxView),
                    Color.White);

        /// <summary>
        /// Border Color of circle image
        /// </summary>

        public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}
	}
}
