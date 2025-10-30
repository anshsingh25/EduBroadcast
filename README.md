# EduBroadcast

## Overview
EduBroadcast is an ASP.NET Core 9.0 MVC application that provides fast tools for everyday classroom communication. The application has been successfully imported and configured to run in the local environment.

**Purpose**: A school communication platform offering message broadcasting, absence logging, and parent-teacher conference scheduling.

**Current State**: ✅ Fully functional and running on port 8000

## Recent Changes
- **October 30, 2025**: 
  - Imported from GitHub and configured for local environment
  - Renamed project from SchoolCommApp to EduBroadcast
  - Updated all namespaces, branding, and references
  - Configured to run on port 8000 with proper host settings
  - Installed .NET 9.0 SDK
  - Set up deployment configuration for autoscale

## Project Architecture

### Technology Stack
- **Framework**: ASP.NET Core 9.0 MVC
- **Language**: C#
- **Data Storage**: JSON files (no database required)
- **Styling**: Bootstrap 5.3.3
- **Icons**: Bootstrap Icons 1.11.3

### Project Structure
```
├── Controllers/          # MVC Controllers
│   ├── AbsenceController.cs
│   ├── HomeController.cs
│   ├── MessagesController.cs
│   └── SchedulerController.cs
├── Models/               # Domain models
│   └── Domain.cs
├── Views/                # Razor views
│   ├── Absence/
│   ├── Home/
│   ├── Messages/
│   ├── Scheduler/
│   └── Shared/
├── Services/             # Business logic
│   └── Messaging.cs
├── Data/                 # Data access layer
│   └── JsonRepository.cs
├── App_Data/             # JSON data storage
│   ├── classes.json
│   ├── templates.json
│   ├── absences.json
│   ├── slots.json
│   ├── bookings.json
│   └── outbox.log
├── wwwroot/              # Static files
│   └── css/
└── Program.cs            # Application entry point
```

### Key Features
1. **Quick Message Broadcasting**
   - Create and manage message templates
   - Broadcast to entire classes or individual students
   - Send via SMS and/or Email (currently file-logged stubs)
   - Access at: `/Messages/Templates` and `/Messages/Broadcast`

2. **While You Were Out (Absence Log)**
   - Create daily class updates for absent students
   - Public class pages for viewing updates
   - Track what was covered, handouts, and homework
   - Access at: `/Absence` and `/Absence/Class/{classId}`

3. **Conference Scheduler**
   - Admin can create time slots for parent-teacher conferences
   - Parents can book available slots
   - Prevents double-booking
   - Access at: `/Scheduler` (public) and `/Scheduler/Admin`

### Configuration Details
- **Port**: 8000 (configured for local development)
- **Host**: localhost (for local development)
- **Environment**: Development
- **Data Persistence**: JSON files in `App_Data/` directory

### Deployment
- **Type**: Autoscale (stateless web application)
- **Command**: `dotnet run`
- Ready to publish via standard deployment systems

## Development Notes

### Running Locally
The application runs automatically via the configured workflow. To manually run:
```bash
dotnet run
```

### Data Storage
All data is stored as JSON files in the `App_Data/` directory:
- Demo data is automatically seeded on first run
- Simple file-based persistence (not concurrent-safe)
- Suitable for small deployments or demos

### Future Enhancements
To integrate real SMS/Email functionality:
1. Implement `ISmsSender` and `IEmailSender` interfaces in `Services/Messaging.cs`
2. Use Twilio for SMS or SendGrid for Email
3. Configure API keys via environment variables or secrets manager
4. Wire implementations in controllers

### Known Considerations
- File-based JSON storage is not concurrent-safe
- For production use with multiple users, consider migrating to a database
- Current messaging uses file-logged stubs - integrate real services for production
- LSP warnings about nullable references exist but don't affect functionality
