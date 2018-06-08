using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using System.Threading;
using Android.Content;
using Android.Runtime;

namespace Spurs
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
        public String DBName = "";
        List<Quest> list = new List<Quest>();
        List<Quest> find = new List<Quest>();
        bool SEARCHINANSWER = false;
        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            EditText et = FindViewById<EditText>(Resource.Id.editText);
            et.AfterTextChanged += TextChanged;

            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
            }
            else
            {
                ReadFile();
                FindingText();
                FillList();
            }
        }


        public void ReadFile()
        {
            //var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //var filename = Path.Combine(path.ToString(), "spurs/" + DBName);
            ///
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            String sysFile = Path.Combine(path, "Choosedfile.txt");
            FileStream cfile = new FileStream(sysFile, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader streamReader = new StreamReader(cfile);
            String filename = "";
            while (!streamReader.EndOfStream)
                filename += (char)streamReader.Read();
            streamReader.Close();
            cfile.Close();
            ///
            if (File.Exists(filename))
            {
                try
                {
                    FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    streamReader = new StreamReader(file, Encoding.Unicode);
                    FillDB(streamReader.ReadToEnd());
                    streamReader.Close();
                    file.Close();
                }
                catch
                {
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    alert.SetTitle("Ошибка открытия файла");
                    alert.SetMessage("Невозможно получить доступ к файлу");
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(path,"spurs"));
                }
                catch { }
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Ошибка открытия файла");
                alert.SetMessage("Файл не существует");
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_file)
            {
                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
                {
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 1);
                }
                else
                {
                    var intent = new Android.Content.Intent(this, typeof(Choose));
                    StartActivityForResult(intent, 2);
                }
                return true;
            }
            if (id == Resource.Id.action_search)
            {
                SEARCHINANSWER = !SEARCHINANSWER;
                item.SetChecked(SEARCHINANSWER);
                FindingText();
                FillList();
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            //base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 2)
            {
                ReadFile();
                FindingText();
                FillList();
            }
        }

        // Заполнение списка вопросов
        private void FillDB(String DB)
        {
            // Удаляем крайние строки
            DB = DB.Replace("<table border=\"1\"><tbody>", "");
            DB = DB.Replace("</tbody></table>", "");
            // Убираем все переносы на новую строку
            DB = DB.Replace("\n", "");
            // Разбиваем базу данных по отдельным вопросам
            String[] temp = DB.Split(new String[] { "<tr>", "</tr>" }, StringSplitOptions.RemoveEmptyEntries);
            list.Clear();
            try
            {
                foreach (String i in temp)
                {
                    list.Add(new Quest(i));
                }
            }
            catch
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Ошибка чтения файла");
                alert.SetMessage("Вероятно, база данных повреждена");
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

        private void TextChanged(object sender, EventArgs eventArgs)
        {
            FindingText();
            FillList();
        }

        private void FindingText()
        {
            find.Clear();
            EditText et = FindViewById<EditText>(Resource.Id.editText);
            String[] parts = et.Text.Split(new String[] { " " },StringSplitOptions.RemoveEmptyEntries);
            foreach(Quest i in list)
            {
                bool skip = false;
                String searchIn = i.GetQuestion();
                if (SEARCHINANSWER)
                    searchIn += " " + i.GetAnswer();
                foreach (String j in parts)
                    if (!searchIn.ToLower().Contains(j.ToLower()))
                        skip = true;
                if (!skip) find.Add(i);
            }
        }

        private void FillList()
        {
            LinearLayout LL = FindViewById<LinearLayout>(Resource.Id.master);
            LL.RemoveAllViews();
            LinearLayout.LayoutParams LP = new LinearLayout.LayoutParams(-1, -2);
            EditText et = FindViewById<EditText>(Resource.Id.editText);
            ScrollView SV = FindViewById<ScrollView>(Resource.Id.scroll);
          
            int count = 0;
            foreach (Quest i in find)
            {
                Button but = new Button(this);
                but.Text = i.GetQuestion();
                but.LayoutParameters = LP;
                but.Id = count;
                but.Click += But_Click;
                count++;
                LL.AddView(but, LP);
            }
        }

        private void But_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(Answers));
            int c = ((Button)sender).Id;
            intent.PutExtra("text", find[c].GetQuestion());
            intent.PutExtra("answ", find[c].GetAnswer());
            StartActivity(intent);
            //throw new NotImplementedException();
        }
    }
}

