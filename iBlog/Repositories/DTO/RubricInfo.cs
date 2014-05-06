using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о рубрике
    /// </summary>
    public class RubricInfo
    {
        public RubricInfo(Rubric r)
        {
            Rubric = r.Title;
            Id = r.RubricId;
        }


        public String Rubric { get; set; }

        public Int32 Id { get; set; }
    }
}
