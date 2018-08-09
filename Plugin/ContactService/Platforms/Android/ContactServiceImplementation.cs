using Android.App;
using Android.Content;
using Android.Database;
using Android.Provider;
using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.ContactService
{
    /// <summary>
    ///     Interface for $safeprojectgroupname$
    /// </summary>
    public class ContactServiceImplementation : IContactService
    {
        public IEnumerable<Contact> GetContactList(Func<Contact, bool> filter = null)
        {
            return GetContacts(filter);
            //.ToList();
        }


        public Task<IEnumerable<Contact>> GetContactListAsync(Func<Contact, bool> filter = null) => Task.Run(() => GetContactList(filter));

        private IEnumerable<Contact> GetContacts(Func<Contact, bool> filter = null)
        {
            var uri = ContactsContract.Contacts.ContentUri;
            //var ctx = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            var ctx = Application.Context;
            var cursor = ctx.ApplicationContext.ContentResolver.Query(uri, null, null, null, null);
            if (cursor.Count == 0) yield break;

            while (cursor.MoveToNext())
            {
                var contact = CreateContact(cursor, ctx);

                if (filter != null && !filter(contact))
                    continue;

                if (!string.IsNullOrWhiteSpace(contact.Name))
                    yield return contact;
            }
        }

        private static Contact CreateContact(ICursor cursor, Context ctx)
        {
            var contactId = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.Id);
            //            var hasNumbers = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber) == "1";

            var numbers = GetNumbers(ctx, contactId);
                //.ToList();
            var emails = GetEmails(ctx, contactId);
                //.ToList();

            var contact = new Contact
            {
                Name = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.DisplayName),
                PhotoUri = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.PhotoUri),
                PhotoUriThumbnail = GetString(cursor, ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri),
                Emails = emails,
                Numbers = numbers,
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
    }
}
