using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public Int32 ContactId { get; set; }

        [Required]
        public String Type { get; set; }

        [Required]
        public String Value { get; set; }

        public Int32 GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual GroupContact Group { get; set; }
    }
}
