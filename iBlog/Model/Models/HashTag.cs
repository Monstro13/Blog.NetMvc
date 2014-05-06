using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("HashTag")]
    public class HashTag
    {
        [Key]
        public Int32 HashTagId { get; set; }

        [Required]
        [MaxLength(50)]
        public String Title { get; set; }

        [MaxLength(150)]
        public String Description { get; set; }

        [Required]
        public Int64 Weigth { get; set; }

        public virtual ICollection<Post> Posts { get; set; } 
    }
}
