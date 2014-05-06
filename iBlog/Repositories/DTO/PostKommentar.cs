using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о комментарии к посту
    /// </summary>
    public class PostKommentar
    {
        public PostKommentar(Kommentar komment)
        {
            KommentId = komment.KommentarId;
            UserLogin = komment.User.Login;
            Message = komment.Message;
            DateCreation = komment.DateCreation;
            Rating = komment.Rating;
        }

        public Int32 KommentId { get; set; }

        public String UserLogin { get; set; }

        public String Message { get; set; }

        public DateTime DateCreation { get; set; }

        public Int32 Rating { get; set; }
    }
}
