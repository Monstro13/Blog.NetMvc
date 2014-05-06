using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("Link")]
    public class Link
    {
        [Key]
        public Int32 LinkId { get; set; }

        [MaxLength(30)]
        public String Title { get; set; }

        [Required]
        [MaxLength(150)]
        [DataType(DataType.Url)]
        public String Value { get; set; }

        public virtual ICollection<Post> Posts { get; set; } 
    }
}
