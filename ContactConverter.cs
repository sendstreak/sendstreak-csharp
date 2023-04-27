using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SendStreak
{
    public class ContactConverter : JsonConverter<Contact>
    {
        public override Contact ReadJson(JsonReader reader, Type objectType, Contact existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            string email = (string)jsonObject["email"];

            Dictionary<string, object> additionalData = new Dictionary<string, object>();
            foreach (var pair in jsonObject.ToObject<Dictionary<string, object>>())
            {
                if (pair.Key != "email")
                {
                    additionalData.Add(pair.Key, pair.Value);
                }
            }

            return new Contact(email, additionalData);
        }

        public override void WriteJson(JsonWriter writer, Contact value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("email");
            writer.WriteValue(value.Email);

            foreach (var pair in value.AdditionalData)
            {
                writer.WritePropertyName(pair.Key);
                writer.WriteValue(pair.Value);
            }

            writer.WriteEndObject();
        }
    }
}
