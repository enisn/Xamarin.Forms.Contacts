using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.ContactService.Shared;

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
