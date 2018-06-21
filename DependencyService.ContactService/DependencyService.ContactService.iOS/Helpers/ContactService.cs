using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Contacts;
using DependencyService.ContactService.Helpers;
using DependencyService.ContactService.iOS.Helpers;
using Foundation;
using Xamarin.Forms;


[assembly: Dependency(typeof(ContactService))]
namespace DependencyService.ContactService.iOS.Helpers
{

    public class ContactService : IContactService
    {

        public Task<IList<Contact>> GetContactListAsync()
        {
            return Task.Run(() => GetContactList());
        }

        public IList<Contact> GetContactList()
        {
            try
            {
                var keysToFetch = new[]
                {
                    CNContactKey.GivenName,
                    CNContactKey.FamilyName,
                    CNContactKey.EmailAddresses
                };
                var contactList = ReadRawContactList(keysToFetch);

                return GetContacts(contactList).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\n\n\n{ex}\n\n\n");
                return new List<Contact>();
            }
        }

        private static IEnumerable<CNContact> ReadRawContactList(IEnumerable<NSString> keysToFetch)
        {
            //var containerId = new CNContactStore().DefaultContainerIdentifier;
            // using the container id of null to get all containers.
            // If you want to get contacts for only a single container type, you can specify that here
            using (var store = new CNContactStore())
            {
                var allContainers = store.GetContainers(null, out _);
                return ContactListFromAllContainers(keysToFetch, allContainers, store);
            }

        }

        private static IEnumerable<CNContact> ContactListFromAllContainers(
            IEnumerable<NSString> keysToFetch,
            IEnumerable<CNContainer> allContainers,
            CNContactStore store)
        {
            var nsStrings = keysToFetch.ToList();
            var contactList = new List<CNContact>();
            foreach (var container in allContainers)
            {
                var contacts = ReadFromContainer(nsStrings, container, store);
                contactList.AddRange(contacts);
            }

            return contactList;
        }

        private static IEnumerable<CNContact> ReadFromContainer(
            IEnumerable<NSString> keysToFetch,
            CNContainer container,
            CNContactStore store)
        {
            try
            {
                using (var predicate = CNContact.GetPredicateForContactsInContainer(container.Identifier))
                {
                    return store.GetUnifiedContacts(predicate, keysToFetch.ToArray(), out _);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\n\n\n{ex}\n\n\n");
            }

            return null;
        }

        private static IEnumerable<Contact> GetContacts(IEnumerable<CNContact> contactList)
        {
            return contactList.Select(GetContact)
                              .Where(contact => !string.IsNullOrWhiteSpace(contact.Name));
        }

        private static Contact GetContact(CNContact item)
        {
            var numbers1 = GetNumbers(item.PhoneNumbers).ToList();
            var emails = GetNumbers(item.EmailAddresses).ToList();

            var contact = new Contact
            {
                Name = $"{item.GivenName} {item.FamilyName}",
                Numbers = numbers1,
                Number = numbers1.LastOrDefault(),
                Emails = emails,
                Email = emails.LastOrDefault()
            };

            return contact;
        }

        private static IEnumerable<string> GetNumbers(CNLabeledValue<NSString>[] items)
        {
            if (items == null) yield break;

            foreach (var item in items)
            {
                var value = item?.Value?.ToString();
                yield return value;
            }
        }

        private static IEnumerable<string> GetNumbers(CNLabeledValue<CNPhoneNumber>[] items)
        {
            if (items == null) yield break;

            foreach (var number in items)
            {
                var value = number?.Value?.ToString();
                yield return value;
            }
        }
    }
}