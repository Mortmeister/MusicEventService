# Music Event Service
## Group Members
- Porieya Fararuni — `fararuni-create` (also commits as `porie`)
- Morten Lillehaug — `Mortmeister`
- Vicente Mastruda — `vicenteM67`
---

## Project Description

Music Event Service is a console-based event management platform focused on  musical
events. Users can register an account, browse and book upcoming concerts and
festivals, manage their own events, and leave reviews for events they have attended.

The platform is built in C# as a console application and demonstrates
core object-oriented programming principles including inheritance, polymorphism,
encapsulation, interfaces, LINQ, and exception handling.

Payments are not handled, only the bookings are tracked.
Data lives in memory only, so it resets every time the program is restarted. Therefore, we´ve created a few users, events and bookings on startup so the menus aren't empty.


## How to Build and Run

**Prerequisites:** .NET 10 SDK

````bash
# Clone the repository
git clone git@github.com:Mortmeister/MusicEventService.git
cd MusicService

# Build
dotnet build

# Run
dotnet run --project MusicService
````

**Test accounts (seeded on startup):**

| Username | Password | Role                                      |
|----------|----------|-------------------------------------------|
| test     | test     | Organiser — has created several events    |
| test2    | test2    | Organiser — has created Iron Maiden Live  |
| buyer    | buyer    | Attendee — has bookings and past events, can leave reviews |

Reviews require a past event with a confirmed booking. Log in as `buyer` to test
 the review feature.

## Design Decisions

### Inheritance and Abstract Classes
`Event` is an abstract base class shared by `Concert` and `Festival`. We chose
this because concerts and festivals share a large set of common fields (title,
date, venue, organiser, ticket types) but each carries type-specific data that
the other does not. Making `Event` abstract enforces that you can never create a generic "event"
without a concrete type, which is what you expect from a music event service.

### Polymorphism
`GetSummary()` and `GetPerformers()` are virtual/abstract methods on `Event`,
overridden in each derived class. We used polymorphism here because the browse
and search views need to display any event type in a list without knowing whether
it is a Concert or Festival. 

### Encapsulation
All model properties use `private set` or are get-only, with state changes
controlled through dedicated methods. We did this to protect data integrity. Without it,
any part of the codebase could set `Status = EventStatus.Cancelled` or decrement
`RemainingQuantity` directly, bypassing validation entirely. Encapsulation means
the object is always responsible for keeping itself in a valid state.

### Separation of Concerns
We split the project into three layers: `Models/` for data, `Services/` for
business logic, and `UI/Menus/` for console input and output. This structure allowed us 
to work together well in parallel, if menu code and business logic
were mixed together, we would constantly be stepping on each other's work.
Keeping them separate also meant that when we found bugs in the service layer,
we could fix them without touching any of the menus.

### Single Source of Truth
Bookings, events, and reviews are stored centrally in `DataStorage` rather than
as lists on the `User` class. We considered keeping `BookingHistory` and
`MyEvents` on `User` directly, but this would mean every write operation has to
update two places simultaneously. If one update is missed, the data goes out of
sync silently. Centralising everything in `DataStorage` and using LINQ to filter
by user means there is only one place to write to and one place to read from.

### LINQ
We Used LINQ for searching, filtering, calculating averages and fetching things by user. Examples: 
`EventService.FilterEventByKeyword`,
`FilterEventByCategory`, `FilterEventByType<T>` (using `OfType<T>()`),
`BookingService.GetBookingsForUser`, average rating in the event detail view.

### Exception Handling
Services throw typed exceptions for domain rule violations, and UI menus catch
them and display user-friendly messages. We chose this because it keeps validation in one place
— the service — rather than repeating the same checks in every menu that calls it.
It also means the application never crashes from something foreseeable. A user
trying to book their own event sees a clear message.

## Requirements Specification

### User Stories

**01 — Register an account**

As a visitor, I want to register with a username and password, so that I can
access the platform.
- Username must be unique (case-insensitive)
- Password is never stored or displayed in plaintext
- Empty username or password is rejected

**02 — Log in**

As a registered user, I want to log in with my credentials, so that I can
access my account and its features.
- Correct credentials grant access and show the main menu
- Failed login shows a generic error message (no hint as to which field was wrong)
- Password input is masked

**03 — Create a Concert**

As a logged-in user, I want to create a Concert event, so that others can
discover and book it.
- Required fields: title, description, date (future), venue, category, genre,
  performers, at least one ticket type
- The logged-in user is automatically set as the organiser
- Invalid input is rejected with a clear message

**04 — Create a Festival**

As a logged-in user, I want to create a Festival event, so that I can advertise
multi-act events.
- Required fields: title, description, date (future), venue, category, lineup,
  duration in days, at least one ticket type
- `Festival` is a distinct class from `Concert` with its own specific fields

**05 — Browse upcoming events**

As a logged-in user, I want to see all upcoming events in a list, so that I can
find something to attend.
- Only events with status `Upcoming` are shown
- Each event displays a one-line summary with title, type, date, and availability
- User can select an event to view its full details

**06 — Search and filter events**

As a logged-in user, I want to search and filter events, so that I can find
something specific.
- Filter by category (selected from a numbered menu)
- Filter by event type (Concert or Festival, selected from a numbered menu)
- Keyword search matches title, description, and venue (case-insensitive)

**07 — Book a ticket**

As a logged-in user, I want to book a ticket for an event, so that I have a
confirmed spot.
- User cannot book their own event
- User cannot book a sold-out ticket type
- Booking is confirmed with a reference number, event, ticket type, price, and date
- Available ticket count decreases immediately on booking

**08 — View and cancel my bookings**

As a logged-in user, I want to view and manage my bookings, so that I can keep
track of what I have signed up for.
- Upcoming and past bookings are shown in separate sections
- User can cancel a confirmed upcoming booking
- Cancellation returns the ticket to the available pool

**09 — Manage my events**

As an organiser, I want to edit and cancel my events, so that I can keep them
up to date.
- Only upcoming events can be edited or cancelled
- Only the organiser of an event may modify it
- A confirmation prompt is shown before cancelling
- Forbidden operations show a user-friendly error message

**10 — Leave a review**

As an attendee, I want to leave a review after attending an event, so that I
can share my experience.
- Review is only available for past events with a confirmed booking
- A user may not review their own event
- A user may leave at most one review per booking
- Rating is between 1 and 5, comment is optional
- Average rating is shown on the event detail page

**11 — View reviews received**

As an organiser, I want to see reviews left on my events, so that I can
understand how my events were received.
- Reviews are accessible from "My Reviews" in the main menu
- Both reviews written by the user and reviews received as an organiser are shown

**12 — View event details**

As a logged-in user, I want to view an event's full details before booking,
so that I can make an informed decision.
- Displays title, organiser, type, date, venue, category, description, ticket
  types with prices and availability, and average rating
- Book option is hidden if the user is the organiser or all tickets are sold out

## Project Plan and Task Breakdown

See `https://github.com/users/Mortmeister/projects/11/views/1` for the full task breakdown from the GitHub
Kanban board.

---

## Process Report

See `docs/process-report.md`.

---

## UML Class Diagram

See `docs/UmlDiagram.png`.

---

## AI Prompts Used

See `docs/ai-prompts.md`.