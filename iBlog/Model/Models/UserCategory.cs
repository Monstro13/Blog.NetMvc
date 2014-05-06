using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("UserCategory")]
    public class UserCategory
    {
        [Key]
        public Int32 UserCategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public String Title { get; set; }

        public String Code { get; set; }
        
        [MaxLength(150)]
        public String Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
