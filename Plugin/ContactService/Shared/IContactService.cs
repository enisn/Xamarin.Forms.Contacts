using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.ContactService
{
    public interface IContactService
    {
        Task<IList<Contact>> GetContactListAsync();
        IList<Contact> GetContactList();
    }
}
