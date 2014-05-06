using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о комментарии
    /// </summary>
    public class InfoKomment
    {
        public InfoKomment(){}

        public InfoKomment(Kommentar komment)
        {
            UserLogin = komment.User.Login;
            DateCreation = komment.DateCreation.ToString();
            KommentarId = komment.KommentarId;
            Message = komment.Message;
        }

        public String UserLogin { get; set; }

        public String DateCreation { get; set; }

        public Int32 KommentarId { get; set; }

        public String Message { get; set; }
    }
}
