using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("Attach")]
    public class AttachFile
    {
        [Key]
        public Int32 AttachFileId { get; set; }

        [Required]
        public Int32 FileTypeId { get; set; }

        [MaxLength(30)]
        public String Title { get; set; }

        [Required]
        [MaxLength(200)]
        [DataType(DataType.ImageUrl)]
        public String FilePath { get; set; }

        [ForeignKey("FileTypeId")]
        public virtual FileType FileType { get; set; }

        public virtual ICollection<Post> Posts { get; set; } 
    }
}
