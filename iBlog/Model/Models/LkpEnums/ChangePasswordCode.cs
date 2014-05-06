using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.LkpEnums
{
    /// <summary>
    /// результат изменения пароля
    /// </summary>
    public enum ChangePasswordCode
    {
        Ok,
        oldIsBad,
        newAndConfirmIsNotEquals,
        Error
    }
}
