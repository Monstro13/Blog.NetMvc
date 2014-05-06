using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("Rubric")]
    public class Rubric
    {
        [Key]
        public Int32 RubricId { get; set; }

        [Required]
        public String Title { get; set; }

        [MaxLength(150)]
        public String Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; } 
    }
}
