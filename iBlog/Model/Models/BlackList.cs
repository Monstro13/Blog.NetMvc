using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("BlackList")]
    public class BlackList
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public Int32 SubjectUserId { get; set; }

        [Required]
        public Int32 ObjectUserId { get; set; }

        [ForeignKey("SubjectUserId")]
        public virtual User SubjectUser { get; set; }

        [ForeignKey("ObjectUserId")]
        public virtual User ObjectUser { get; set; }
    }
}
