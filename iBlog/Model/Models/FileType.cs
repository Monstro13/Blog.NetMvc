using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("FileType")]
    public class FileType
    {
        [Key]
        public Int32 FileTypeId { get; set; }

        [Required]
        [MaxLength(10)]
        public String Format { get; set; }

        [Required]
        public Int32 TypeId { get; set; }

        [ForeignKey("TypeId")]
        public virtual FileCategory FileCategory { get; set; }

        public virtual ICollection<AttachFile> Files { get; set; } 
    }
}
