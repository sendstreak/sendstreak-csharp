using Newtonsoft.Json;
using System.Collections.Generic;

namespace SendStreak
{
    internal struct MailPayload
    {
        [JsonProperty(PropertyName = "rcpt")]
        public string RecipientAddress { get; set; }

        [JsonProperty(PropertyName = "templateSlug")]
        public string TemplateSlug { get; set; }

        [JsonProperty(PropertyName = "variables", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, string> Variables { get; set; }

        public MailPayload(string recipientAddress, string templateSlug, IDictionary<string, string> variables)
        {
            this.RecipientAddress = recipientAddress;
            this.TemplateSlug = templateSlug;
            this.Variables = variables;
        }
    }
}
