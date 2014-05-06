using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// диалог пользователей 
    /// </summary>
    public class Correspondence
    {
        public List<PostKommentar> Messages { get; set; }

        public String PostTitle { get; set; }

        public Int32 PostId { get; set; }
    }
}
