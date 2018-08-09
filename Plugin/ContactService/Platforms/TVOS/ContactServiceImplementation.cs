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
        public IEnumerable<Contact> GetContactList(Func<Contact, bool> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contact>> GetContactListAsync(Func<Contact, bool> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
