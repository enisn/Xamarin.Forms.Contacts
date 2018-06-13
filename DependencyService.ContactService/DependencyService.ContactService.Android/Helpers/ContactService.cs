using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.Content;
using Android.Database;
using Android.Provider;
using DependencyService.ContactService.Droid.Helpers;
using DependencyService.ContactService.Helpers;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(ContactService))]
namespace DependencyService.ContactService.Droid.Helpers
{
    public class ContactService : IContactService
    {
        public IList<Contact> GetContactList() { return GetContactListA().ToList(); }

        private IEnumerable<Contact> GetContactListA()
        {
            var uri = ContactsContract.Contacts.ContentUri;
            //var ctx = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            var ctx = Application.Context;
            var cursor = ctx.ApplicationContext.ContentResolver.Query(uri, null, null, null, null);
            if (cursor.Count == 0)
            {
                yield break;
            }

            while (cursor.MoveToNext())
            {
                var contact = CreateContact(cursor, ctx);

                if (!string.IsNullOrWhiteSpace(contact.Name))
                    yield return contact;
            }
        }

        private static Contact CreateContact(ICursor cursor, Context ctx)
        {
            var contactId = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.Id);
//            var hasNumbers = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber) == "1";

            var numbers = GetNumbers(ctx, contactId).ToList();
            var emails = GetEmails(ctx, contactId).ToList();

            var contact = new Contact
            {
                Name = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.DisplayName),
                PhotoUri = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.PhotoUri),
                PhotoUriThumbnail = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri),
                Emails = emails,
                Email = emails.LastOrDefault(),
                Numbers = numbers,
                Number = numbers.LastOrDefault()
            };

            return contact;
        }

        private static IEnumerable<string> GetNumbers(Context ctx, string contactId)
        {
            var key = ContactsContract.CommonDataKinds.Phone.Number;

            var cursor = ctx.ApplicationContext.ContentResolver.Query(
                ContactsContract.CommonDataKinds.Phone.ContentUri,
                null,
                ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = ?",
                new[] { contactId },
                null
            );

            return ReadCursorItems(cursor, key);
        }
        private static IEnumerable<string> GetEmails(Context ctx, string contactId)
        {
            var key = ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data;

            var cursor = ctx.ApplicationContext.ContentResolver.Query(
                ContactsContract.CommonDataKinds.Email.ContentUri,
                null,
                ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + " = ?",
                new[] { contactId },
                null);

            return ReadCursorItems(cursor, key);
        }

        private static IEnumerable<string> ReadCursorItems(ICursor cursor, string key)
        {
            while (cursor.MoveToNext())
            {
                var value = GetString(cursor, key);
                yield return value;
            }
            cursor.Close();
        }

        private static string GetString(ICursor cursor, string key)
        {
            return cursor.GetString(cursor.GetColumnIndex(key));
        }

        public Task<IList<Contact>> GetContactListAsync()
        {
            return Task.Run(() => GetContactList());
        }
    }
}