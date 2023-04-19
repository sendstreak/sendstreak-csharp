# SendStreak C# SDK

[SendStreak](https://www.sendstreak.com) is a simple interface that lets you integrate quickly with email services such as Mailchimp, Sendgrid or even AWS SES or Gmail to decouple your audience, email history and templates from your email provider.

## Installation
```sh
$ dotnet add <project_name> package sendstreak-csharp
```

## Usage
```csharp
using SendStreak;

SendStreakClient client = new SendStreakClient("YOUR_API_KEY");

Contact contact = new Contact(
    "johndoe@example.com",
    new Dictionary<string, object>()
    {
        { "firstName", "John" },
        { "lastName", "Doe" },
        { "onboarded", false }
    }
);

// Push your contacts to SendStreak with as many attributes as you want
Task task = client.UpdateContactAsync(contact);
task.Wait();

// Send them emails using predefined templates
task = client.SendMailAsync(
    "johndoe@example.com",
    "customer-welcome-email",
    new Dictionary<string, string>()
    {
        { "username", "johndoe" }
    }
);
task.Wait();
```

## We accept contributions here

If you're a C# developer using SendStreak and want to contribute to this SDK, we're more than happy to have your pull request here - and your name on the hall of fame forever!
