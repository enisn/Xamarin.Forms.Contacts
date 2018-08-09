using System.Collections.Generic;
using System.Linq;

namespace Plugin.ContactService.Shared
{
    /// <summary>
    /// Crossplatform contact model.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Display Name of Contact
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// If photo assigned, photo uri of this contact
        /// </summary>
        public string PhotoUri { get; set; }
        /// <summary>
        /// If photo assigned photo thumbnail uri of this contact
        /// </summary>
        public string PhotoUriThumbnail { get; set; }
        /// <summary>
        /// Phone number of this contact
        /// </summary>
        public string Number { get => Numbers.LastOrDefault(); }
        /// <summary>
        /// Email address of this contact
        /// </summary>
        public string Email { get => Emails.LastOrDefault(); }
        /// <summary>
        /// If contact have multiple phone numbers 
        /// </summary>
        public IEnumerable<string> Numbers { get; set; }
        /// <summary>
        /// IF contact have multiple email addresses
        /// </summary>
        public IEnumerable<string> Emails { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Contact()
        {
            //Numbers = new List<string>();
            //Emails = new List<string>();
        }
        /// <summary>
        /// Displays contact name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
