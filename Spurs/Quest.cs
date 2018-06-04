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
        String image = "";
        public Quest(String str)
        {
            // Делим строку на ячейки (должно получиться 2 штуки)
            String[] que = str.Split(new String[] { "<td>", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
            // Заносим вопрос
            text = que[0];
            // Пересохрняем ответы
            String temp = que[1];
            // Пока есть строки
            while (temp.IndexOf("<p>") != -1)
            {
                // Добавляем в список первый подвернувшийся ответ
                answer += temp.Substring(temp.IndexOf("<p>") + 3, temp.IndexOf("</p>") - temp.IndexOf("<p>") - 3) + "\n";
                // Удаляем весь текст с начала по конец этого ответа
                temp = temp.Remove(0, temp.IndexOf("</p>") + 4);
            }
            // Восстанавливаем строку с ответами
            temp = que[1];
            // Пока есть ссылки на картинки
            while (temp.IndexOf("src=\"") != -1)
            {
                // Находим начало адреса картинки
                int start = temp.IndexOf("src=\"") + 5;
                // Добавляем адрес картинки в список
                image += temp.Substring(start, temp.IndexOf("\"", start) - start) + "\n";
                // Удаляем текст с начала по конец адреса картинки
                temp = temp.Remove(0, temp.IndexOf("\"", start) + 1);
            }
        }

        public String GetQuestion() { return text; }
        public String GetAnswer() { return answer; }
        public String GetImage() { return image; }
    }
}