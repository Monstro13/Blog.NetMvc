using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о игнор-листе
    /// </summary>
    public class IgnorInfo
    {
        public IgnorInfo()
        {
        }

        public IgnorInfo(BlackList blackList)
        {
            CurrentUserId = blackList.SubjectUserId;

            IgnorId = blackList.ObjectUserId;
        }

        /// <summary>
        /// кто добавляет
        /// </summary>
        public String Login { get; set; }

        /// <summary>
        /// его ид
        /// </summary>
        public Int32 CurrentUserId { get; set; }

        /// <summary>
        /// кого добавляет
        /// </summary>
        public Int32 IgnorId { get; set; }
    }
}
