# iPractice Technical Assignment
## 1.Introduction

This assignment has been built for self praticing as well as for an ongoing hiring process. I will be happy to receive feedback.

Project has been build using Visual Studio 2022 with .net 6 on Windows 11 environment.

In order to run and debug it on VsCode Launch configuration created can be executed from "Run and Debug" tab. (Api will be accessable through: https://localhost:44301/swagger)

## 2.Additonal Constraints

Even though it is not specified on the requirements document, there are some constraints created for the sake of simplicity.
- Appointments and Availabilities can not start or end besides every quarter of an hour. Allowed :00, :15, :30 and :45
- Updating an appointment is only possible when updated timeslots are still containing all the scheduled appointments.

## 3.Domain Model

![alt text](https://github.com/huseyinbirtan/iPracticeApi/blob/main/Diagrams/DomainModel.png)

In the diagram, Clients and Psychologists are specified as SyncAggregates. The reason is creation or management of those entites should be out of the scope of that domain. For the same reason, they are going to be managed by the integration handlers those are going to be listening domain events for those entities.

After introducing Availability and Appointment objects, some of the value objects become more appearent those are AvailabilityTimeSlot and regular TimeSlot which are being used for appointments. Allthough AvailablityTimeSlot is inherited by TimeSlot, there is one additional behaviour compared to its parent which is "Contains". To figure out whether given appointment is in the Availibility.

Since we got to check information accross the availibilities, it become practical to introduce Schedule as the aggregate root. Here we can find a convenient availability as per given appointment timeslot as well as manage availablities to make sure that there is no overlapping availibities are being created. Although there is a 1 to Many relation between Psychologists and Schedules, at the moment there is only one Schedule can be created per Psychologist. Moving forward, Schedules might reserve Year and WeekOfYear properties for the sake of performance in data retrieval.

## 4.Architecture

![alt text](https://github.com/huseyinbirtan/iPracticeApi/blob/main/Diagrams/ArchitectureOverview.png)

Architecture is demonstrated on the diagram above.

### External Message Queue

This queue will be used to feed the Client and Psychologist data from the external domains. In case there are changes for those entites, changes needs to be reflected in Scheduling Domain Database. "Integration Handlers" will be the consumer of queue messages.

### Integration Handlers

That Handlers are not tested in the scope of Scheduling domain. Just build for the demonstration purposes using Mediatr which is not convenient for communication accross the domains. Different alternatives should be used.

### Domain Events

There are not many domain events has been introduced in the scope of this domain. Those events again needs more complicated frameworks to be used to publish the messages. Can be used to push messages to domains like Billing or Notification for email sending purposes.

## 5.Solution Structure

![alt text](https://github.com/huseyinbirtan/iPracticeApi/blob/main/Diagrams/SolutionStructure.png)

### iPractice.Scheduling.Domain

This project is the core of the solution which is being referenced from Api and DataAccess layer. 
It contains:
- Schedule Aggregate Domain Models
- Abstraction of Repositories
- Exceptions which are being thrown from the domain objects
- SyncAggregates (Client and Psychologist)
- ValueObjects
- Events being published from domain operations

### iPractice.Scheduling.Api

That is the project where application logic, Integration Handlers as well as the controllers/endpoints are being hosted. That is the only external point of interaction within our domain.

For http requests, validations are being done using FluentValidation in the controller level. Once the commands are generated they are being published using Mediatr for entities to be created or updated by the ScheduleCommandHandler.

For Querying data, there is a class defined ReadModel which is being consumed by the Controllers directy.

Exception handling is centralized using ExceptionHandlingMiddleware.

### iPractice.DataAccess

Could be better renamed iPractice.Scheduling.Infrastructure. This project simply consist of the configuration of the entities (aggregates) as well as the concrete definition of the Repository Classes. Configuration is to define the relationships between aggregates and sync-aggregates.

### iPractice.SharedKernel

SharedKernel project contains a few abstractions which might be used accross the domains.

## 6.Unit Tests

Due to time concerns, I wanted to write some unit tests for the Domain Objects, I needed to make sure they are functioning as expected. Tests should be extendended at least for the Handlers.

## 7.Moving Forward

During the development of that project, I have had made a lot of practical choices in order to timebox my effort.

a) There are some performance concerns which might be addressed by:
- Scheduled are created as one per psychologists. Instead of current approach, having schedules per week of the year will make retrieval of the data much performant.
- Pagination for the requests will also bring some efficiency

b) There are some parts which needs to be addressed for a full blown multi-domain application. At the moment the implementation has been done using Mediatr. However, when it comes the communicating accross the domains, frameworks like Wolverine and Masstransit will become more handy.

c) One final point might be having a better logging mechanism for the sake of better monitoring of the Domain.

d) Specification Pattern might be implemented to avoid repository abstractions and definitions to grow too large

e) !!Last Minute Notes!! 
- Forgot to do the check about the client-psychologist assignment while creating appointment
- Swagger should be updated
- Getting List of Availabilities with their Ids will help to update the availability afterwards. Sorry for that.

## 8.Thanks

Thanks for that good challange. Looking forward to hearing from you!