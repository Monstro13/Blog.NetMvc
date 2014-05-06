using System.Data.Entity;
using Model.Context;

namespace Model.Wrapper
{
    public class BlogInitWrapper
    {
        public static void Init()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogContext, Configuration.Configuration>());            
        }
    }
}
