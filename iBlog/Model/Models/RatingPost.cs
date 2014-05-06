using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models.LkpEnums;

namespace Model.Models
{
    [Table("RatingPost")]
    public class RatingPost
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public Int32 UserId { get; set; }

        [Required]
        public Int32 PostId { get; set; }

        [Required]
        public RatingCode Value { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }
}
