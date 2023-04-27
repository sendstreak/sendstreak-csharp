using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SendStreak
{
    /// <summary>
    /// API Client to make requests to SendStreak.
    /// </summary>
    public class SendStreakClient
    {
        private const string SendStreakApiUrl = "https://api.sendstreak.com";

        private HttpClient httpClient;

        private string apiKey;

        /// <param name="apiKey">The API key for authentication.</param>
        /// <param name="requestHeaders">A collection of headers which will be added to every request.</param>
        /// <exception cref="ArgumentException"></exception>
        public SendStreakClient(string apiKey, Dictionary<string, string> requestHeaders)
        {
            if (validateApiKey(apiKey))
            {
                throw new ArgumentException("An API key is required to initialize the SendStreak SDK.");
            }

            this.apiKey = apiKey;

            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(SendStreakApiUrl);
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.apiKey);
            
            foreach (var header in requestHeaders)
            {
                this.httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        /// <inheritdoc cref="SendStreakClient(string, Dictionary{string, string})"/>
        public SendStreakClient(string apiKey) : this(apiKey, new Dictionary<string, string>()) { }

        /// <summary>
        /// Gets or sets the API key used by the client.
        /// </summary>
        public string ApiKey { 
            get => apiKey;
            set
            {
                if (!validateApiKey(value))
                {
                    throw new Exception("A valid API key must be provided.");
                }

                this.apiKey = value;
            }
        }

        /// <summary>
        /// Updates or creates a contact if it does not exist already.
        /// </summary>
        /// <param name="contact">An object containing the contact's data.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task UpdateContactAsync(Contact contact)
        {
            string json = JsonConvert.SerializeObject(contact);
            return this.invokeSendStreakApiAsync("/v1/contacts", json);
        }

        /// <summary>
        /// Sends an event to store for the provided contact.
        /// </summary>
        /// <param name="email">The email address of the contact.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task SendEvent(string email, string eventName)
        {
            string json = JsonConvert.SerializeObject(new
            {
                email = email,
                slug = eventName
            });
            return this.invokeSendStreakApiAsync("/v1/events", json);
        }

        /// <summary>
        /// Sends a single mail asynchronously using the template specified by the template slug.
        /// </summary>
        /// <param name="recipientAddress">The email address of the recipient.</param>
        /// <param name="templateSlug">The slug string of the template to use.</param>
        /// <param name="variables">Variables used by the template.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task SendMailAsync(string recipientAddress, string templateSlug, IDictionary<string, string> variables)
        {
            string json = JsonConvert.SerializeObject(new MailPayload(recipientAddress, templateSlug, variables));
            return this.invokeSendStreakApiAsync("/v1/messages", json);
        }

        /// <inheritdoc cref="SendMailAsync(string, string, IDictionary{string, string})" />
        public Task SendMailAsync(string recipientAddress, string templateSlug)
        {
            return this.SendMailAsync(recipientAddress, templateSlug, null);
        }

        private Task<HttpResponseMessage> invokeSendStreakApiAsync(string path, string jsonPayload)
        {
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            return this.httpClient.PostAsync(path, content);
        }

        private static bool validateApiKey(string apiKey)
        {
            return apiKey == null || apiKey.Length == 0;
        }
    }
}
