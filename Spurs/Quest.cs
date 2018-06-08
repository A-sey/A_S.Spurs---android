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

namespace Spurs
{
    class Quest
    {
        String text;
        String answer = "";
        public Quest(String str)
        {
            // Делим строку на ячейки (должно получиться 2 штуки)
            String[] que = str.Split(new String[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
            // Заносим вопрос
            text = que[0];
            // Пересохрняем ответы
            String temp = que[1];
            // Преобразуем строку
            temp = temp.Replace("</p>", "");
            temp = temp.Replace("\"></img>", "");
            temp = temp.Replace("<img src=\"", "<p>:i:");
            // Пока есть строки
            String[] parts = temp.Split(new String[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String i in parts)
                answer += i + "\n";
        }

        public String GetQuestion() { return text; }
        public String GetAnswer() { return answer; }
    }
}