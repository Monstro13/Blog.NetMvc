using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о категории
    /// </summary>
    public class CategoryInfo
    {
        public CategoryInfo(){}

        public CategoryInfo(UserCategory category)
        {
            Id = category.UserCategoryId;
            Title = category.Title;
            Code = category.Code;
        }

        public Int32 Id { get; set; }

        public String Title { get; set; }

        public String Code { get; set; }
    }
}
