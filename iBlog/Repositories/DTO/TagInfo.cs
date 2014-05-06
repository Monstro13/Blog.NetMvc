using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// инфо о теге
    /// </summary>
    public class TagInfo
    {
        public TagInfo(HashTag tag)
        {
            this.Title = tag.Title;
            this.Weigth = tag.Weigth;
        }

        public String Title { get; set; }

        public Int64 Weigth { get; set; }
    }
}
