using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Spurs
{
    [Activity(Label = "@string/app_name")]
    public class Image : Activity
    {
        Point size = new Point(0,0);
        Point start = new Point(0,0);
        HorizontalScrollView HV;
        ScrollView SV;
        ImageView IVold = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.content_image);
            size.Set(1100, 1100);
            start.Set(0, 0);
            DrawImage();

            ZoomControls ZC = FindViewById<ZoomControls>(Resource.Id.zoom);
            ZC.ZoomInClick += ZC_ZoomInClick;
            ZC.ZoomOutClick += ZC_ZoomOutClick;
        }

        private void ZC_ZoomOutClick(object sender, EventArgs e)
        {
            int step = -200;
            ZoomChange(step);
            //throw new NotImplementedException();
        }

        private void ZC_ZoomInClick(object sender, EventArgs e)
        {
            int step = 200;
            ZoomChange(step);
            //throw new NotImplementedException();
        }

        private void ZoomChange(int step)
        {
            if (size.X + step > 0 && size.Y + step > 0 && size.X + step < 10000 && size.Y + step < 10000)
            {
                size.Set(size.X + step, size.Y + step);
                start.X = (int)(start.X * size.X / (size.X - step));
                start.Y = (int)(start.Y * size.Y / (size.Y - step));
                DrawImage();
            }
        }

        private void DrawImage()
        {            
            if (IVold!=null)
                IVold.SetImageURI(null);
            HV = FindViewById<HorizontalScrollView>(Resource.Id.hscroll);
            SV = FindViewById<ScrollView>(Resource.Id.scroll);
            SV.RemoveAllViews();
            FrameLayout FL = new FrameLayout(this);
            String uri = Intent.GetStringExtra("img");
            ImageView IV = new ImageView(this);
            IV.SetImageURI(Android.Net.Uri.Parse(uri));
            IV.SetScaleType(ImageView.ScaleType.FitCenter);
            ViewGroup.LayoutParams LP = new ViewGroup.LayoutParams(size.X, size.Y);
            FL.AddView(IV, LP);
            SV.AddView(FL, LP);
            IVold = IV;
        }
    }
}