using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о новости
    /// </summary>
    public class PostInfo
    {
        #region constructor

        public PostInfo(Post post)
        {
            PostId = post.PostId;

            DateCreation = post.DateCreation;

            Title = post.Title;

            Message = post.Message;

            Rating = post.Rating;

            UserLogin = post.User.Login;

            UserRating = post.User.Karma;

            Rubric = post.Rubric.Title;

            Kommentars = new List<PostKommentar>();
            post.Kommentars.Where(x=> !x.IsDeleted && !x.IsSpam).ToList().ForEach(x => Kommentars.Add(new PostKommentar(x)));

            HashTags = post.HashTags.ToList();

            Links = post.Links.ToList();

            Files = new List<DataFile>();
            post.Files.ToList().ForEach(x => Files.Add(new DataFile { Path = x.FilePath, Category = x.FileType.FileCategory.Title }));
        }

        #endregion

        #region fields

        public Int32 PostId { get; set; }

        public DateTime DateCreation { get; set; }

        public String Title { get; set; }

        public String Message { get; set; }

        public Int32 Rating { get; set; }

        public String UserLogin { get; set; }

        public Int32 UserRating { get; set; }

        public String Rubric { get; set; }

        public List<PostKommentar> Kommentars { get; set; }

        public List<DataFile> Files { get; set; }

        public List<HashTag> HashTags { get; set; }

        public List<Link> Links { get; set; }

        #endregion
    }
}
