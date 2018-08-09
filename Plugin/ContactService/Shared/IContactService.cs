using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.ContactService
{
    public interface IContactService
    {
        /// <summary>
        /// Gets contact in an awaitable background task
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Contact>> GetContactListAsync(Func<Contact,bool> filter = null);
        /// <summary>
        /// Gets contacts in main thread
        /// !!!NOT RECOMMENDED
        /// </summary>
        /// <returns></returns>
        IEnumerable<Contact> GetContactList(Func<Contact, bool> filter = null);
    }
}
