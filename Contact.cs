using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SendStreak
{
    /// <summary>
    /// Represents a contact in SendStreak.
    /// </summary>
    [JsonConverter(typeof(ContactConverter))]
    public class Contact
    {
        private const string EmailRegex = @".+@.+\..+";

        private string email;

        private IDictionary<string, object> additionalData;

        /// <summary>
        /// Gets or sets the email address of the contact.
        /// </summary>
        public string Email { get => email; set => email = value; }

        /// <summary>
        /// Gets additional (arbitrary) data about the contact.
        /// </summary>
        public IDictionary<string, object> AdditionalData { get => additionalData; }

        /// <param name="email">The email address of the contact.</param>
        /// <param name="additionalData">Optional arbitrary data for the contact.</param>
        /// <exception cref="Exception"></exception>
        public Contact(string email, IDictionary<string, object> additionalData)
        {
            if (!validateEmail(email))
            {
                throw new Exception($"An invalid email address has been provided: '{email}' is not a valid address.");
            }

            this.email = email;
            this.additionalData = additionalData;
        }

        /// <inheritdoc cref="Contact(string, IDictionary{string, object})"/>
        public Contact(string email) : this(email, new Dictionary<string, object>()) { }

        private static bool validateEmail(string email)
        {
            if (email.Length > 0)
            {
                Match m = Regex.Match(email, EmailRegex);
                return m.Success;
            }

            return false;
        }
    }
}