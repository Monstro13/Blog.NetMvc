using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о новости для админа
    /// </summary>
    public class AdminPostInfo
    {
        public AdminPostInfo(){}

        public AdminPostInfo(Post post)
        {
            NewsId = post.PostId.ToString();
            UserLogin = post.User.Login;
            DateCreation = post.DateCreation.ToString("dd/MM/yyyy HH:mm:ss");
            Rubric = post.Rubric.Title;
            PostTitle = post.Title;
            PostRating = post.Rating.ToString();
            UserRating = post.User.Karma.ToString();
            Status = post.IsDeleted ? "Удалено\r\n" : "";
            Status += post.IsSpam ? "Спам\r\n" : "";
            Status += post.IsConfirm ? "Подтверждено" : "Не подтверждено";
        }

        public String NewsId { get; set; }

        public String UserLogin { get; set; }

        public String DateCreation { get; set; }

        public String Rubric { get; set; }

        public String PostTitle { get; set; }

        public String PostRating { get; set; }

        public String UserRating { get; set; }

        public String Status { get; set; }
    }
}
