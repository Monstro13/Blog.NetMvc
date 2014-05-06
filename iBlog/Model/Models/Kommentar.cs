using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("Kommentar")]
    public class Kommentar
    {
        [Key]
        public Int32 KommentarId { get; set; }

        [Required]
        public Int32 UserId { get; set; }

        [Required]
        public Int32 PostId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreation { get; set; }

        [Required]
        [MaxLength(3000)]
        [DataType(DataType.Text)]
        public String Message { get; set; }

        [Required]
        public Int32 Rating { get; set; }

        [Required]
        public Boolean IsSpam { get; set; }

        [Required]
        public Boolean IsDeleted { get; set; }

        [Required]
        public Boolean IsConfirm { get; set; }

        [Required]
        public Boolean IsChanged { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public virtual ICollection<RatingKommentar> RatingKommentars { get; set; } 
    }
}
