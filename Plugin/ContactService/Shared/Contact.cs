using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.ContactService.Shared
{
    public class Contact
    {
        public string Name { get; set; }
        public string PhotoUri { get; set; }
        public string PhotoUriThumbnail { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }

        public List<string> Numbers { get; set; }
        public List<string> Emails { get; set; }

        public Contact()
        {
            Numbers = new List<string>();
            Emails = new List<string>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
