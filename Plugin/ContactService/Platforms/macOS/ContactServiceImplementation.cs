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
