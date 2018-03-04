using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.ContactService
{
    public partial class MainPage : ContentPage
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        public MainPage()
        {
            InitializeComponent();
            GetContacs();
        }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        async Task GetContacs()
        {
            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            lstContacts.BindingContext = contacts;
        }
    }
}
