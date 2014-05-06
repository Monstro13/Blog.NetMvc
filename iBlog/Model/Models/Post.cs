using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public Int32 PostId { get; set; }

        [Required]
        public Int32 UserId { get; set; }

        [Required]
        public Int32 UserCategoryId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreation { get; set; }

        [Required]
        public Int32 RubricId { get; set; }

        [MaxLength(100)]
        public String Title { get; set; }

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

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RubricId")]
        public virtual Rubric Rubric { get; set; }

        public virtual ICollection<Kommentar> Kommentars { get; set; }

        public virtual ICollection<AttachFile> Files { get; set; }

        public virtual ICollection<HashTag> HashTags { get; set; }

        public virtual ICollection<Link> Links { get; set; }

        public virtual ICollection<RatingPost> RatingPosts { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } 
    }
}
