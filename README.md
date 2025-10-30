## SchoolCommApp (ASP.NET Core)

An ASP.NET Core MVC app providing:

- Quick Message broadcast (SMS/Email) using file-logged stubs
- Digital "While You Were Out" absence log with public class pages
- Parent-Teacher Conference Scheduler with booking and conflict prevention

Data is stored as JSON files in `App_Data/` so it runs without a database.

### Prerequisites
- .NET 8 SDK installed

### Run
```bash
cd "SchoolCommApp"
dotnet run
```
Then open `https://localhost:5147` (or the URL shown in the console).

### Features
- Quick Message
  - Manage templates: `/Messages/Templates`
  - Broadcast to a class or selected students: `/Messages/Broadcast`
  - Outgoing messages are appended to `App_Data/outbox.log`
- While You Were Out
  - Add entries for a class: `/Absence`
  - Public class page: `/Absence/Class/{classId}`
- Conference Scheduler
  - Admin slots: `/Scheduler/Admin`
  - Booking portal: `/Scheduler`

### Configuration
Replace the stub senders with Twilio/SendGrid integrations by implementing `ISmsSender` and `IEmailSender` in `Services/Messaging.cs` and wiring them in the controllers where used.

### Notes
- The app seeds demo classes, students, and templates on first run.
- JSON persistence is simplistic and not concurrent-safe; fine for small deployments or demos.


