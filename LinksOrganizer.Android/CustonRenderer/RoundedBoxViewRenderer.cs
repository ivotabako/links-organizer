﻿using System.ComponentModel;
using Android.Content;
using LinksOrganizer.CustomControls;
using LinksOrganizer.CustonRenderer;
using LinksOrganizer.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly:
  ExportRenderer(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]

namespace LinksOrganizer.CustonRenderer
{
    public class RoundedBoxViewRenderer : ViewRenderer<RoundedBoxView, View>
    {
        public RoundedBoxViewRenderer(Context context)
            : base(context)
        {

        }

        public static void Init()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<RoundedBoxView> e)
        {
            base.OnElementChanged(e);

            this.InitializeFrom(Element);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            this.UpdateFrom(Element, e.PropertyName);
        }
    }
}
