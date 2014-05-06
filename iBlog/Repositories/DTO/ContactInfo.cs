using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Repositories.DTO
{
    /// <summary>
    /// информация о контакте
    /// </summary>
    public class ContactInfo
    {
        public ContactInfo(){}

        public ContactInfo(Contact contact)
        {
            ContactId = contact.ContactId;

            Type = contact.Type;

            Value = contact.Value;
        }

        public Int32 ContactId { get; set; }

        public String Type { get; set; }

        public String Value { get; set; }
    }
}
