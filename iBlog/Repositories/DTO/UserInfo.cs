using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    /// <summary>
    /// базовая информация при регистрации пользователя
    /// </summary>
    public class UserInfo
    {
        public String FirstName { get; set; }

        public String SecondName { get; set; }

        public String Email { get; set; }

        public String Login { get; set; }

        public String Password { get; set; }

        public String confirmPassword { get; set; }

        public String Sex { get; set; }
    }
}
