using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.LkpEnums
{
    /// <summary>
    /// результат регистрации
    /// </summary>
    public class ResultRegister
    {
        /// <summary>
        /// идентификатор пользователя
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// его эмайл
        /// </summary>
        public String UserEmail { get; set; }

        /// <summary>
        /// код завершения операции
        /// </summary>
        public UserCreationCode Code { get; set; }
    }
}
