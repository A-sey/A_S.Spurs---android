using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Spurs
{
    [Activity(Label = "@string/app_name")]
    public class Answers : Activity
    {
        List<String> images = new List<string>();
        String path;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.content_answer);
            String text = Intent.GetStringExtra("text");
            String answ = Intent.GetStringExtra("answ");
            String imag = Intent.GetStringExtra("imag");
            //String ImagePath = Intent.GetStringExtra("path");
            ScrollView SV = FindViewById<ScrollView>(Resource.Id.ans_scroll);
            LinearLayout LL = FindViewById<LinearLayout>(Resource.Id.ans_mast);
            LinearLayout.LayoutParams LP = new LinearLayout.LayoutParams(-1, -2);
            EditText et1 = FindViewById<EditText>(Resource.Id.et1);
            et1.Text = text;
            et1.LongClick += Et_LongClick;
            EditText et2 = FindViewById<EditText>(Resource.Id.et2);
            et2.Text = answ.Substring(0, answ.Length - 1);
            et2.LongClick += Et_LongClick;
            path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            path = Path.Combine(path.ToString(), "spurs/"/*, ImagePath*/);
            GetImgs(imag);
            for (int i = 0; i < images.Count; i++)
            {
                try
                {
                    ImageView IV = new ImageView(this);
                    IV.SetImageURI(Android.Net.Uri.Parse(path + images[i]));
                    IV.SetScaleType(ImageView.ScaleType.FitStart);
                    IV.LayoutParameters = new ViewGroup.LayoutParams(-1, 1000);
                    IV.Id = i;
                    IV.LongClick += IV_LongClick;
                    LL.AddView(IV);
                }
                catch
                {
                    Toast.MakeText(Application.Context, "Ошибка открытия изображения", ToastLength.Long).Show();
                }
            }
        }

        private void GetImgs(String imag)
        {
            String[] parts = imag.Split(new String[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String i in parts)
                images.Add(i);
        }

        private void Et_LongClick(object sender, View.LongClickEventArgs e)
        {
            String str = ((EditText)sender).Text;
            ClipboardManager myClipboard = (ClipboardManager)GetSystemService(ClipboardService);
            ClipData myClip = ClipData.NewPlainText("text", str);
            myClipboard.PrimaryClip = myClip;
            Toast.MakeText(Application.Context, "Текст скопирован", ToastLength.Short).Show();
        }

        private void IV_LongClick(object sender, View.LongClickEventArgs e)
        {
            var intent = new Android.Content.Intent(this, typeof(Image));
            String uri = images[((ImageView)sender).Id];
            intent.PutExtra("img", path+uri);
            StartActivity(intent);
        }
    }
}