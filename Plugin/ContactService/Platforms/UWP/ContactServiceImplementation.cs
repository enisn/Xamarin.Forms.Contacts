using System;
using System.Collections.Generic;
using System.Text;
using Plugin.ContactService.Shared;
using System.Threading.Tasks;

namespace Plugin.ContactService
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public class ContactServiceImplementation : IContactService
    {
        public IList<Contact> GetContactList()
        {
            throw new NotImplementedException();
        }

        public Task<IList<Contact>> GetContactListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
