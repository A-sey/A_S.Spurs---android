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
    [Activity(Label = "Выбор файла")]
    public class Choose : Activity
    {
        String[] files = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.content_choose);
            var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            path = Path.Combine(path.ToString(), "spurs/");
            files = Directory.GetFiles(path);
            LinearLayout LL = FindViewById<LinearLayout>(Resource.Id.master);
            ViewGroup.LayoutParams LP = new ViewGroup.LayoutParams(-1, -2);
            for(int i=0; i<files.Length; i++)
            {
                if (files[i].EndsWith(".html"))
                {
                    Button but = new Button(this);
                    but.Id = i;
                    but.Click += But_Click;
                    but.Text = Path.GetFileName(files[i]);
                    LL.AddView(but, LP);
                }
            }
        }

        private void But_Click(object sender, EventArgs e)
        {
            int id = ((Button)sender).Id;
            SetFileName(files[id]);
            //throw new NotImplementedException();
        }

        private void SetFileName(String file)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            String filename = Path.Combine(path, "Choosedfile.txt");
            FileStream cfile = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(cfile);
            streamWriter.Write(file);
            streamWriter.Close();
            cfile.Close();
            
            this.SetResult(Result.Ok);
            this.Finish();

        }


    }
}