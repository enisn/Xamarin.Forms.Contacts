using Contacts;
using Foundation;
using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.ContactService
{
    /// <summary>
    /// Interface for $safeprojectgroupname$
    /// </summary>
    public class ContactServiceImplementation : IContactService
    {
        public Task<IList<Contact>> GetContactListAsync()
        {
            return Task.Run<IList<Contact>>(() =>
            {
                return GetContactList();

            });

        }

        public IList<Contact> GetContactList()
        {
            try
            {
                var keysToFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.EmailAddresses };
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
                var contacts = new List<Contact>();

                foreach (var item in contactList)
                {
                    if (item.GivenName == null) continue;
                    Contact _contact = new Contact();
                    _contact.Name = item.GivenName + " " + item.FamilyName;

                    if (item.PhoneNumbers != null)
                    {
                        foreach (var number in item.PhoneNumbers)
                        {
                            _contact.Number = number?.Value?.ToString();
                            _contact.Numbers.Add(number?.Value?.ToString());
                        }
                    }

                    if (item.EmailAddresses != null)
                    {
                        foreach (var email in item.EmailAddresses)
                        {
                            _contact.Email = email?.Value?.ToString();
                            _contact.Emails.Add(email?.Value?.ToString());
                        }
                    }
                    contacts.Add(_contact);

                }
                return contacts;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\n\n\n" + ex.ToString() + "\n\n\n");
                return new List<Contact>();
            }
        }

    }
}
