using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о конкретном диалоге
    /// </summary>
    public class InfoDialog
    {
        public InfoDialog(Answer answer, int getUserId)
        {
            CurrentUserId = getUserId;
            CompanionId = answer.WhoUserId == getUserId ? answer.WhomUserId : answer.WhoUserId;
            IsWatched = answer.IsWatched;
            NewsId = answer.PostId;
        }


        public Int32 CurrentUserId { get; set; }

        public String LoginCompanion { get; set; }

        public Int32 CompanionId { get; set; }

        public Int32 NewsId { get; set; }

        public String NewsTitle { get; set; }

        public Boolean IsWatched { get; set; }
    }
}
