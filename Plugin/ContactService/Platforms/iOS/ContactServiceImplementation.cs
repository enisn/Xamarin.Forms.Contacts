using Contacts;
using Foundation;
using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Plugin.ContactService
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public class ContactServiceImplementation : IContactService
    {
        /// <summary>
        /// Gets contact in a background task
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Contact>> GetContactListAsync(Func<Contact, bool> filter = null) => Task.Run(() => GetContactList(filter));

        /// <summary>
        /// Gets contact in main thread
        /// !!!Not Recommended
        /// </summary>
        public IEnumerable<Contact> GetContactList(Func<Contact, bool> filter = null)
        {
            //try
            //{
            var keysToFetch = new[] { CNContactKey.Identifier, CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.EmailAddresses, CNContactKey.PhoneNumbers, CNContactKey.ImageDataAvailable, CNContactKey.ThumbnailImageData };
            NSError error;
            //var containerId = new CNContactStore().DefaultContainerIdentifier;
            // using the container id of null to get all containers.
            // If you want to get contacts for only a single container type, you can specify that here
            var contactList = new List<CNContact>();
            using (var store = new CNContactStore())
            {
                var allContainers = store.GetContainers(null, out error);
                foreach (var container in allContainers)
                {
                    try
                    {
                        using (var predicate = CNContact.GetPredicateForContactsInContainer(container.Identifier))
                        {
                            var containerResults = store.GetUnifiedContacts(predicate, keysToFetch, out error);
                            contactList.AddRange(containerResults);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("\n\n\n" + ex.ToString() + "\n\n\n");

                        if (ex.GetType() != typeof(NullReferenceException))
                            Debug.WriteLine(ex.ToString());
                        continue;
                    }
                }
            }
            //var contacts = new List<Contact>();

            foreach (var item in contactList)
            {
                if (item.GivenName == null) continue;
                Contact _contact = new Contact();

       
                if (filter != null && !filter(_contact))
                    continue;

                yield return _contact;
            }
         
        }

        Contact CreateContact(CNContact contact)
        {
            return new Contact
            {
                Numbers = GetNumbers(contact),
                Emails = GetEmails(contact),
                Name = $"{contact.GivenName} {contact.FamilyName}",
                //PhotoUri = //NOT IMPLEMENTED YET,
                //PhotoUriThumbnail = //NOT IMPLEMENTED YET,
                
            };
        }

        IEnumerable<string> GetNumbers(CNContact contact)
        {
            foreach (var number in contact.PhoneNumbers)
            {
                yield return number?.Value?.ToString();
            }
        }

        IEnumerable<string> GetEmails(CNContact contact)
        {
            foreach (var email in contact.EmailAddresses)
            {
                yield return email?.Value?.ToString();
            }
        }

    }
}
