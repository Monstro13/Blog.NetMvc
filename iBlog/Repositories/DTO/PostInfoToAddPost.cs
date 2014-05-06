using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    /// <summary>
    /// модель для добавления новости
    /// </summary>
    public class PostInfoToAddPost
    {
        public List<String> FilePaths { get; set; }
 
        public Int32 RubricId { get; set; }

        public String TitlePost { get; set; }

        public String Text { get; set; }

        public List<String> HashTags { get; set; }

        public List<String> Links { get; set; }

        public Boolean ForSelfCategory { get; set; }
    }
}
