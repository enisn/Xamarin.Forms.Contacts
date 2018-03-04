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
        Task<IList<Contact>> GetContactListAsync();
        /// <summary>
        /// Gets contacts in main thread
        /// !!!NOT RECOMMENDED
        /// </summary>
        /// <returns></returns>
        IList<Contact> GetContactList();
    }
}
