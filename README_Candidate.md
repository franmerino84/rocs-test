# Ryanair TravelLabs - ROCS Hiring Test - Tycoon Factory - Candidate comments



## Table of Contents

[TOC]

## Overview
This application request was very open, but at same time, conducted to use some specific architecture and coding strategies and methodologies that I suppose are currently used by the company.

I understand this as a test, but also an opportunity to improve and iterate over previous knowledge in the different concepts used throughout the solution.

I've read and worked before using all the required strategies, but in many of them, just in a limited way, because of working over existing projects, or just using some certain of technologies or libraries.

I'm very proud of the result of the work, and also about the learning acquired during the process.

### The dog ate my homework
I know. I'm not excusing myself, but putting in context that it's difficult to find the needed time to face a solution from its beggining, and merging several technical approaches while working 40 hours a week, and caring a 5 months old baby during my free time. 

For more misfortune, I had some medical issues during the weekend, and that slowed me more in the development. But again, I think I made a good work here.

The effects of this limitation in time came principally in the way I had to sacrifice coding in TDD at some point in favour of completing the design and functionality of the 4 use cases cases, and leaving some exception handling, and testing for a late stage of the development.

To cover that, I kept always track some TODO comments for all the code, to ensure I would come back later for it.

Unfortunately, the solution grew up too much, and there's still some tests placeholders to cover. In any case, I've ensured that all main classes (domain, presentation, application, and persistence) have been properly tested, and only helpers and mapper classes are pending.



## Application usage
I've built the application as a Web API. I won't cover at this point installation of the application, so it's thought to be run over Visual Studio at this point.

I've used Visual Studio 2022, with projects that run over .NET 7, so make sure you have installed the latest version to run it. Probably it works over previous IDE, or even the projects can be moved to .NET 6, but I cannot ensure since I haven't tested it.

To make it run, just open the solution, select as default project the project "TycoonFactoryScheduler.Presentation.Api" and press F5 (or Ctrl + F5 if you don't want to debug).

A browser window will pop-up with a Swagger front page showing all the options covered by the API. You can use also any other http client like Postman, or similar.

Now I'll describe the implemented use cases.

### Insert a new activity
The method is POST and the endpoint is <applicationUrl>/api/activities

The body parameters allowed are:

* ActivityType: string (only accepts BuildComponent and BuildMachine) - Type of the activity to create
* Description: string (1-100 characters) - Details of the activity
* Start: DateTime (required) - Beginning date and time of the activity
* End: DateTime (after Start) - Final date and time of the activity
* Workers: Array of single chars (size > 0) - Ids of the workers assigned to the activity

The response of a successful request is 201 Created, containing as body:

* Id: int - Id of the activity
* ActivityType: string - Type of the activity to create
* Description: string - Details of the activity
* Start: DateTime - Beginning date and time of the activity
* End: DateTime - Final date and time of the activity
* Workers: Array of single chars - Ids of the workers assigned to the activity

The response of a not successful request, because of not existing workers, is a 404 Not Found with an array of strings describing the errors.

The response of a not successful request, because of scheduling conflicts, is a 409 Conflict with an array of strings describing the errors.

The response of a not successful request, because of not accomplishing the minimum or maximum amount of workers allowed for that activity type, is a 422 Unprocessable Entity with a string describing the error.

The response of a not successful request, because of an unexpected failure is 500 Internal Server Error without any content.

### Delete an activity
Note here that I've considered this operation as idempotent. So it will be return success even if the activity didn't exist previously.

The method is DELETE and the endpoint is <applicationUrl>/api/activities/<id>

The placeholder parameter is:

* Id: int - Id of the activity to update

The response of a successful request, is 204 No Content, without any content

The response of a not successful request, because of an unexpected failure is 500 Internal Server Error without any content.

### Update the dates of an existing activity
The method is PATCH and the endpoint is <applicationUrl>/api/activities/<id>

The placeholder parameter is:

* Id: int - Id of the activity to update

The body parameters allowed are:

* Start: DateTime (Required) - New beginning date and time of the activity
* End: DateTime (after Start) - New final date and time of the activity

The response of a successful request is 200 OK, containing as body:

* Id: int - Id of the activity
* ActivityType: string - Type of the activity to create
* Description: string - Details of the activity
* Start: DateTime - Beginning date and time of the activity
* End: DateTime - Final date and time of the activity
* Workers: Array of single chars - Ids of the workers assigned to the activity

The response of a not successful request, because of not existing activity is a 404 Not Found with a string describing the error.

The response of a not successful request, because of scheduling conflicts is a 409 Conflict with an array of strings describing the errors.

The response of a not successful request, because of an unexpected failure is 500 Internal Server Error without any content.

### Get the top 10 busy workers in next 7 days
In fact, I wrote and endpoint that delivers the descripted query if no parameters are added, but it accepts also to put the start and end date of the analysis, and also the amount of workers to retrieve (as much).

I'm not considering into the count of time, the one spent charging the batteries.

The method is GET and the endpoint is <applicationUrl>/api/workers/get-top-busy

The url parameters allowed are:

* Start: DateTime - Start date and time of the analysis - Defaulted to current date and time, or well 7 days before the specified end date and time
* End: DateTime - End date and time of the analysis - Defaulted to 7 days after the specified start date and time
* Size: int - Number of workers to retrieve - Defaulted to 10

The response of a successful request is 200 OK, containing as body an array of:

* Id: int - Worker id 
* TimeBusy: TimeSpan - Amount of time that the worker was working

The response of a not successful request, because of an unexpected failure is 500 Internal Server Error without any content.

To retrieve information from the seed data, I recommend to set the Start parameter as 2023-03-03.

## Architecture of the solution
As mention before, there were some guidelines in the description of the problem, so I decided to follow them.

To be honest, they were going to be my first option in any case, so it's great that I didn't have to adapt too much.

Therefore, I have chosen a Design-Driven-Development-like approach while structuring the projects. I have several layers with different responsibilities and visibility. 

### Common
This layer contains some classes and utils that are used by other layers including Domain

### Domain
The Domain layer contains all the entities and main business logic of the application.

It consumes the Common layer.

### Abstractions
The Abstractions layer contains some interfaces, abstract classes, and models, mainly related to the Persistence layer.

It consumes the Common layer and the Domain layer.

### Infrastructure
I've used this project as an utility project available for all other layers.

In my case, I've implemented some exception, IOC, logging, mapping, validating, and static adaptations services that helps a lot to simplify and clean up the other layers, so they can be more focus in their real responsibility.

It consumes the Common layer.

### Persistence
This layer ensures the persistence of the data in a database, and to provide it back for the required queries.

I'm using here Entity Framework Core to store the information in Sql Server, and some Unit of Work and Repository patterns. EF Core already implements them some way, but I considered interesting to create some wrappers over it so I can have the Application layer agnostic to the implementation or any Queryable objects management.

It consumes the Common, the Domain and the Abstractions layers.

### Application
This layer wrappes the 4 use cases management being independent to the final output, so different possible presentation layers could use it.

The management of each use case covers interactions with the database, logging, and the domain objects and services with their associated business logic.

I've used here a CQRS approach, and different classes for each use case.

It consumes the Common, the Domain and the Abstractions layers.

### Presentation
The presentation layer is the one in charge of interacting with the end user. It listen for the requests and provide a response in a friendly, standard and understandable, so any final user can take advantage of it.

In this case I decided to use an ASP .NET Core Web API, so it can be used as the backend side of any console, desktop or web application. It can be even used through Swagger (only while debugging), or by any other http client, like Postman.

It consumes the Common, Abstractions, and Application layers. 

### Test
The test layer, can be considered outside the application itself, since it has the responsability to ensure every piece of the software works at expected.

I've mimic the projects and directories structure of the rest of the application inside this layer to get easy access to the corresponding test class of a specific class.

I've also created a Test helpers project to contain some common logic of the tests, so it can be reused.

Most of the tests are Unit, to guarantee the input and output of every method, property and function in all classes, but there are also some Integration tests for the Persistence layer, so I can ensure it works as expected in a real environment.

This layer consumes its corresponding tested layer project.



## Technologies, design patterns, processes, and other concerns

### Target framework - .Net 7
All the projects are built over the framework .NET 7. I considered that even if it's not an LTS release, there's not much difference with .NET 6, and maybe I could take advantage of any improvement.

### Screaming Architecture
Recently I heard of this way to structure the directory and files inside a project and I considered very interesting.

I've seen and worked on multiple projects where the files are organized by its nature. So for example there's a directory Controllers, where there are only controllers. Other directory (or big file) for mappings. Another for view models. Another for exceptions. Another for services, etc.

This way of structuring a project has advantages in the sense of you can have a global idea of what kind of things are being done in any of this directories. The big disadvantage here is that there's not any trace between all the classes required to other single class. That can lead to forget some configuration actions (like dependencies injections, or mapping registers).

The key of Screaming Architecture is that, for all those classes that are only used from the same family of classes, to be stored under the same entity directory, and/or use case directory.

So for example, in the presentation layer we have (under the controllers directory, that I respected because of the standard of this key name) a directory for the Activities, and other for the Workers. Inside them, we have another directory for each use case. In our case a use case it's an endpoint. And inside the use case directory, we have all the controller file, dtos classes and mapping classes.

Just to clarify, I've not created a specific controller per use case, but used the same controller as a partial class.

I'm not very fan of partial classes, but in this case works like a charm, since another typical issue is having very big controller classes that are difficult to read.

### Object Oriented Programming
I've tried to make use of OOP techniques over all the solution. Making use of interfaces, abstract classes, generic types, and inheritance.

Many of my helpers classes are based on OOP, but I will focus in the Domain layer.

Here I've created an Abstract Activity class from which inherits each of the activity types. The behavior in our case it's the same for every one of them, but they have some specifics parameters (modeled as implemented abstract properties) that change the main logic. 

These parameters are the minimum and maximum number of workers allowed (minimum is common, but I implemented it just in case), and the batteries charge duration.

### Entity Framework Core
I've decided to use Entity Framework Core over Sql Server as persistence of the application.

To build up the database model I have follow a Code First approach, so the entities are defined in the Domain Layer, and the database context is defined in the Persistence layer.

I have create some seed data for each of the three resultant tables. I've taken the license of include a few more fields to the entities, so they look more real.

With all that information I've created an automatic Initial migration file that creates the tables, relationship and initial data.

It has been a little tricky to take the inheritance to the database model, but I achieved it with a Table-per-hierarchy and Discriminator approach.

To keep up to date the configured database (it reads from the connection string "TycoonFactoryScheduler"), I've set at the startup of the application to run the migrations.

I've configured the appsettings.development.json to point the connection string to a localdb so it can be created in the runtime machine as a Sql Express instance.

To test the persistence layer, I've created some integration tests that build up and destroy another different localdb database for each of the tests.

I was sorry that for the GetTopBusy function I had to materialize part of the query, since Entity Framework Core doesn't support aggregations over nested collections. I could have written a SQL text query, or a Stored Procedure but I prefered to keep it like this. I commented it in the code, just in case in the future, a new EF Core version supports it, and can be improved.

### Unit of work pattern
I've implemented this unit of work pattern to ensure all storage actions are realized in the same transaction.

Probably it wasn't needed at this stage, since Entity Framework Core somehow ensures that, but also works as a Facade that simplify the dependencies in the constructors.

### Repository pattern
Again, Entity Framework Core already covers this pattern, but, in order to keep the database logic over the persistence layer, in special about all related to queries, it has been interesting to use this pattern.

These instances are accesses through the Unit of Work class.

### MediaTR (Mediator pattern)
I've use the nuget MediaTR to cover two different aspects.

For one side, it implements the Mediator pattern that liberates from the injecting explosion issue on the constructor of the controllers.

It just map where to call when receiving an object of certain type, and return its corresponding reponse type if defined.

I've used this approach for the calls from the Presentation layer to the Application layer.

### CQRS pattern
I've implemented the CQRS pattern in the application layer. So, also supported by the Screaming Architecture, I've isolated the actions that just queries the state, from others that actually modifies it.

This split has two main utilities. One for clarity of the reader, to ensure if executing some code has side effects or not. The other is to be able to manage the performance or the access to the databases making distinction between these two kind of access.

The ideal would have been to take this pattern to other layers, but I'll leave it for the next steps section.

### Automapper and factories
I've used the nuget Automapper to translate classes between different layers. From dtos to application classes parameters, and from application classes parameters to domain entities mainly.

Also some mappings works like a Factory pattern. I'm talking about the Activity entity that depending on the activity type requires different instances of the subclasses. I've make use of the ConstructUsing method that it provides.

### NLog logging
I've used NLog nuget to help in the task of creating logs of the application.

I've created a default NLog.config file that configures the logging by creating a log file per day, and also in the console window. It supports to use other targets like AWS CloudWatch, or Elastic Search, but they weren't required.

I've also disable some Microsoft standard logging because of clarity of the created logs, but it can be enabled easily.

I've put the responsibility of logging in the Application layer.

### Branching policy
I've decide to use GitFlow over Git as branching policy. Basically I've created a develop branch to start there the development. That branch was being updated by merge of concrete feature branches. At the end of the process, I've created a release branch to take the final changes to the main branch.

I could be used also a trunk approach, but for an initial stage I see this more appropriate.

I've left the merged branches open on purpose so you can review the full process, but I would have closed them after the merge.

### SOLID
I've tried to follow SOLID guidelines across the whole solution.

#### Single responsibility principle
Each class, except for the repositories, has only one responsibility. I've tried to separate through use cases all the functionality so they are not so big an have a comprehensive semantic.

#### Open-closed principle
Using inheritance to describe different types of activity. It can be extended in the future without affecting previous implementations. Only the factories need to be changed.

#### Liskov substitution principle
Every override in the inheritance or interface implementation consider the expected behavior of the parent, so this principle is not violated.

#### Interface segregation principle
There are not much interfaces, but I've segregated the IGenericRepository methods on IGenericReadOnlyRepository and IGenericWriteRepository for example.

Other interfaces are not big enough be considered splittables.

#### Dependency inversion principle
All business-related classes and third parties are used through dependency injection in the constructor. I've created some static classes, but only over generic helpers.

### Clean code
I've tried to keep the code as readable as I can. For that, I've followed the following approaches:

* Not very big classes
* Having comprehensive, standardize and logical names for directories, projects, interfaces, classes, properties, fields, methods, functions, parameters and variables
* Avoid comments
* Extract to methods or functions reusable blocks of code and parts of code very long if make sense
* Following same coding style and organization along the whole code

### Test Driven Development
As I mentioned in the section "The dog ate my homework", I just use this approach in the first steps of the feature "feature/top-10-busy-workers", so the commits can be tracked there.

The procedure is simple:

* Write a unit test.
* Code the minimun needed to pass the test.
* Refactor the application to adapt to the new scenario.
* If more value tests can be added, return to first step.

### Testing
I've used NUnit as test driver, and created unit tests for every class. For the database related class I've implemented Integration tests with ephimeral environments.

I've tagged (added traits) the tests as Unit, Integration and Database, so the tests can be ran depending of the type.

I've used Moq nuget to Mock interfaces and classes methods and functions.

I've left lot of TODO placeholders because of lack of time.

### Exception handling
I've created a bunch of custom exceptions for different situations in the use cases. The application layer is catching, logging, and rethrowing them. The presentation layer is catching them again and transforming into the appropriate http responses.

All non-custom exceptions that are catch by the application layer are transformed into an UnexpectedApplicationException, so the presentation layer can return a 500 Internal Server Error.

So the application layer only throws custom exceptions, and argument exceptions if their parameters have problems.

### Reflection
Sometimes is demonized, but I consider Reflection a very useful util that the framework offers. In this solution I've used it to collect classes that implements or extends an interface or class and add them to some specific configuration.

I've used it to collect:

* Dependency injection
* Mapping (for Automapper)
* Mediator handlers (for MediaTR)



## Dilemmas
During the development I have faced several internal discussion about how to proceed with several situations:

### Enums and exceptions types shared along the whole project
I have read different positions about sharing enums and exceptions or creating different versions of the same at different layers.

In my opinion, and that's what I've implemented here, there's no reason to duplicate these types, and it will make more difficult the maintainability of the application.

### Unit testing mocking hell
Another open discussion, even if the dominant position is to implement massive unit test mocking all  injected dependencies, is that maybe make sense to use more integration tests, or partial integration test, to simulate a little more the real behavior of a class.

I haven't follow that approach, but I understand, that all this mocking sometimes conducts the implementation into one unique solution, and probably other codes that also satisfied the same requirement are not valid for our unit tests.

### Event oriented application
It's very extended in DDD oriented applications to use events to communicate different layers and entities. I understand its possibilities, and the power and uncoupling behind, but also how it makes the solution less readable, and with a higher learn curve.

I've decided not to use it in this application for the moment.

There is some coupling on the domain entities. Probably I should have use interfaces in the relationships so I could have just mocked the behavior. As result of this, the tests I've added to the entity classes, are not real unit tests, but somekind of scoped integration tests.

Also I had the problem to create some useless parameterless constructor, to be able to mock it, and this can be improved.


## Next steps
This is a list of next steps I would follow in a real application:
* Complete application testing (TODOs placeholders)
* Delete merged branches (left keeped for the tracking of the reviewers)
* Uncouple domain entities between them
* Add end to end tests
* Manage CancellationToken
* Return in the response some application and parameterized error codes 
* Protect the API through authentication and/or authorization
* Hide domain properties to ensure business methods are called
* Dockerize API
* CICD
* API versioning
* Application versioning
* Get deeper in CQRS approach, and reach it to the Repository layer


---

Thanks for the opportunity. I hope you enjoy this solution!

Francisco Javier Merino Gallardo
