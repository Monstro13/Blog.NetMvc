using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("FileCategory")]
    public class FileCategory
    {
        [Key]
        public Int32 FileCategoryId { get; set; }

        [Required]
        [MaxLength(20)]
        public String Title { get; set; }

        public virtual ICollection<FileType> FileTypes { get; set; } 
    }
}
