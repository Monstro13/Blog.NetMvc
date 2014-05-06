using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("Answer")]
    public class Answer
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public Int32 KommentarId { get; set; }

        [Required]
        public Int32 PostId { get; set; }

        [Required]
        public Int32 WhomUserId { get; set; }

        [Required]
        public Int32 WhoUserId { get; set; }

        [Required]
        [MaxLength(3000)]
        public String Source { get; set; }

        [Required]
        [MaxLength(3000)]
        public String Text { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public Boolean IsWatched { get; set; }

        [Required]
        public Boolean IsDeleted { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        [ForeignKey("WhomUserId")]
        public virtual User WhomUser { get; set; }

        [ForeignKey("WhoUserId")]
        public virtual User WhoUser { get; set; }
    }
}
