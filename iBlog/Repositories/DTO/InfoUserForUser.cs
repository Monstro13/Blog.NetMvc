using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// Информация о пользователе для других пользователей 
    /// </summary>
    public class InfoUserForUser
    {

        public InfoUserForUser(User user)
        {
            UserId = user.UserId.ToString();

            Name = String.Format("{0} {1}", user.FirstName, user.SecondName);
            Sex = user.Sex ? "Мужчина" : "Женщина";
            Rating = user.Karma.ToString();

            var diff = DateTime.Now - user.DateRegistration;
            var years = diff.Days/365;
            var monats =  (diff.Days - years*365)/30;
            var days = diff.Days - monats*30 - years*365;

            TimeOnSite = String.Format("Лет: {0} | Месяцев: {1} | Дней: {2}", years, monats, days);

            CountOfPost = user.Posts.Count.ToString();
            CountOfKomment = user.Kommentars.Count.ToString();
        }

        public String UserId { get; set; }

        public String Name { get; set; }

        public String Sex { get; set; }

        public String Rating { get; set; }

        public String TimeOnSite { get; set; }

        public String CountOfPost { get; set; }

        public String CountOfKomment { get; set; }
    }
}
