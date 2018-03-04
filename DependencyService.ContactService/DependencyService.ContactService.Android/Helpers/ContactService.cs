using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using DependencyService.ContactService.Droid.Helpers;
using DependencyService.ContactService.Helpers;

[assembly: Dependency(typeof(ContactService))]
namespace DependencyService.ContactService.Droid.Helpers
{
    public class ContactService : IContactService
    {
        public IList<Contact> GetContactList()
        {
            var uri = ContactsContract.Contacts.ContentUri;
            //var ctx = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            var ctx = Application.Context;
            var cursor = ctx.ApplicationContext.ContentResolver.Query(uri, null, null, null, null);

            var contactList = new List<Contact>();


            if (cursor.MoveToFirst())
            {
                while (cursor.MoveToNext())
                {
                    if (cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName)) == null)
                        continue;

                    var _contact = new Contact();

                    _contact.Name = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                    _contact.PhotoUri = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.PhotoUri));
                    _contact.PhotoUriThumbnail = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri));

                    String contact_id = cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.Id));

                    #region Email
                    var emailCursor = ctx.ApplicationContext.ContentResolver.Query(
                             ContactsContract.CommonDataKinds.Email.ContentUri, null,
                             ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId + " = ?",
                             new String[] { contact_id }, null);

                    while (emailCursor.MoveToNext())
                    {
                        _contact.Email = emailCursor.GetString(emailCursor.GetColumnIndex(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data));
                        _contact.Emails.Add(_contact.Email);

                    }
                    emailCursor.Close();
                    #endregion



                    if (cursor.GetString(cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber)) == "1")
                    {
                        #region Phone
                        var phoneCursor = ctx.ApplicationContext.ContentResolver.Query(
                            ContactsContract.CommonDataKinds.Phone.ContentUri, null,
                            ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId + " = ?",
                            new string[] { contact_id }, null
                            );
                        while (phoneCursor.MoveToNext())
                        {
                            _contact.Number = phoneCursor.GetString(phoneCursor.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                            _contact.Numbers.Add(_contact.Number);
                        }
                        phoneCursor.Close();
                        #endregion
                    }

                    contactList.Add(_contact);
                }
            }
            else
            {
                //ActivityCompat.RequestPermissions((Activity)Forms.Context, new string[] { permission }, 1);
            }

            return contactList;
        }

        public Task<IList<Contact>> GetContactListAsync()
        {
            return Task.Run(() => GetContactList());
        }
    }
}