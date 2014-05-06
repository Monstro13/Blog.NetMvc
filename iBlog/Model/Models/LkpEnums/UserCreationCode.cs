using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.LkpEnums
{
    /// <summary>
    /// результат операции создания пользователя
    /// </summary>
    public enum UserCreationCode
    {
        Ok,
        EmailExists,
        LoginExists,
        DiffPass,
        Error
    }
}
