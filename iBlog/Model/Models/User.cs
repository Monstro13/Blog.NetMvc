using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public Int32 UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public String FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public String SecondName { get; set; }

        [Required]
        [MaxLength(100)]
        public String Login { get; set; }

        [Required]
        [MaxLength(200)]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateRegistration { get; set; }

        [Required]
        public Int32 Karma { get; set; }

        [Required]
        public Boolean Sex { get; set; }

        [Required]
        public Boolean IsActivate { get; set; }

        [Required]
        public Boolean IsAdmin { get; set; }

        [Required]
        public Boolean IsDeleted { get; set; }

        [Required]
        public Boolean IsBlocked { get; set; }

        [Required]
        public Int32 UserCategoryId { get; set; }

        [ForeignKey("UserCategoryId")]
        public virtual UserCategory UserCategory { get; set; }

        public virtual ICollection<Kommentar> Kommentars { get; set; }

        public virtual ICollection<RatingKommentar> RatingKommentars { get; set; } 

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<RatingPost> RatingPosts { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } 
    }
}