using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о пользователе для админа
    /// </summary>
    public class InfoUserForAdmin
    {
        public InfoUserForAdmin(){}

        public InfoUserForAdmin(User user)
        {
            UsersId = user.UserId.ToString();
            Fname = user.FirstName;
            Sname = user.SecondName;
            Category = user.UserCategory.Title;
            Login = user.Login;
            Email = user.Email;
            DateRegistration = user.DateRegistration.ToString("dd/MM/yyyy HH:mm:ss");
            Rating = user.Karma.ToString();
            PostsCount = user.Posts.Count().ToString();
            KommentsCount = user.Kommentars.Count().ToString();
            Sex = user.Sex ? "Мужской" : "Женский";
            IsAdmin = user.IsAdmin;
            IsBlocked = user.IsBlocked;
            Status = user.IsActivate ? "Активный\r\n" : "Не активный\r\n";
            Status += user.IsDeleted ? "Удален\r\n" : "";
            Status += user.IsBlocked ? "Заблокирован\r\n" : "";
            Status += user.IsAdmin ? "Администратор" : "";
        }

        public String UsersId { get; set; }

        public String Category { get; set; }

        public String Fname { get; set; }

        public String Sname { get; set; }

        public String Login { get; set; }

        public String Email { get; set; }
        
        public String DateRegistration { get; set; }

        public String Rating { get; set; }

        public String PostsCount { get; set; }

        public String KommentsCount { get; set; }

        public String Sex { get; set; }

        public String Status { get; set; }

        public Boolean IsAdmin { get; set; }

        public Boolean IsBlocked { get; set; }
    }
}
