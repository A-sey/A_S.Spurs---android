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
        List<String> lines = new List<string>();
        String path;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.content_answer);
            String text = Intent.GetStringExtra("text");
            String answ = Intent.GetStringExtra("answ");
            ScrollView SV = FindViewById<ScrollView>(Resource.Id.ans_scroll);
            LinearLayout LL = FindViewById<LinearLayout>(Resource.Id.ans_mast);
            LinearLayout.LayoutParams LP = new LinearLayout.LayoutParams(-1, -2);
            EditText et1 = FindViewById<EditText>(Resource.Id.et1);
            et1.Text = text;
            et1.LongClick += Et_LongClick;
            path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            path = Path.Combine(path.ToString(), "spurs/");
            GetLines(answ);
            String tempAns = "";
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith(":i:"))
                    try
                    {
                        if (tempAns != "")
                        {
                            EnterText(tempAns, LL);
                            tempAns = "";
                        }
                        ImageView IV = new ImageView(this);
                        IV.SetImageURI(Android.Net.Uri.Parse(path + lines[i].Remove(0,3)));
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
                else tempAns += lines[i] + "\n";
            }
            if (tempAns != "")
                EnterText(tempAns, LL);
        }

        private void GetLines(String text)
        {
            String[] parts = text.Split(new String[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String i in parts)
                lines.Add(i);
        }

        private void EnterText(String text, LinearLayout LL)
        {
            TextView TV = new TextView(this);
            TV.Text = text;
            TV.SetTextColor(Android.Graphics.Color.Black);
            TV.SetPadding(10, 0, 10, 0);
            TV.LongClick += TV_LongClick;
            LL.AddView(TV);
        }

        private void TV_LongClick(object sender, View.LongClickEventArgs e)
        {
            String str = ((TextView)sender).Text;
            ClipboardManager myClipboard = (ClipboardManager)GetSystemService(ClipboardService);
            ClipData myClip = ClipData.NewPlainText("text", str);
            myClipboard.PrimaryClip = myClip;
            Toast.MakeText(Application.Context, "Текст скопирован", ToastLength.Short).Show();
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
            var intent = new Intent(this, typeof(Image));
            String uri = lines[((ImageView)sender).Id].Remove(0,3);
            intent.PutExtra("img", path+uri);
            StartActivity(intent);
        }
    }
}