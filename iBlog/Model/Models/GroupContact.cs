using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("MetaInformation")]
    public class GroupContact
    {
        [Key]
        public Int32 GroupContactId { get; set; }

        [Required]
        [MaxLength(20)]
        public String Title { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } 
    }
}
