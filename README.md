# Ryanair TravelLabs - ROCS Hiring Test - Tycoon Factory

## Scenario Description
**Tycoon Co.** is a factory in which the production is realized by automated android workers. You need to create an application 
that maintains and organizes the schedule of these workers' activities inside the factory. 

Each android worker is identified by a letter of the alphabet (A, B, etc).

Workers' activities have a start and end time. There are two different types of activities:
 * Build component: Which is always performed by one worker.
 * Build machine: That can be performed by one or several workers that join together in a team.

These android workers can do activities during full day, all days of the year. But each time a worker finishes an activity 
needs to rest some time to recharge its batteries. Independently of the duration of the activity, the charge period will be 
2 hours after finishing to build a component and 4 hours after building a machine.

## Solution Scope
 * The application should allow to schedule activities in any time (future or past). When an activity is created it must be possible to indicate
 the start and end times, the type of activity and the worker (or list of workers) that is performing that activity.
 * It should be possible to modify the times of one existing activity
 * And also, it should be possible to delete an existing activity.
 * The application should indicate when a worker's activity conflicts with other activities of the same worker, because their times overlap.
 These conflicts will help the users of the application to adjust the times of the activities so the issue can be solved.
 * The application should show a list of the top 10 androids that are working more time in the next 7 days.

### Initial Values and Seed Data
You can define some initial data like the information of the android workers, and even some initial activities already assigned.

### Data Persistence
It is not mandatory to implement a full persistence; the application can work with data in memory (so changes are discarded when application is closed).

### User Interface
You are free to implement the external interface of your application as you feel more confident. You can implement Web API, just a
console application that reads keyboard inputs, or any other approach (does not need to be even functional). For us, it is more important the
backend side.

## What we are looking for?
* Coding in English.
* Clean Code.
* SOLID Principles.
* Separation of concerns.
* Perform some unit tests
* Use any frameworks and libraries that you consider convenient.

## Bonus / Nice to Have
* Organize the code in different projects if required is a plus.
* Use of Domain-Driven Design concepts only if you have experience with it is a plus.
* Following TDD is welcome only if you have experience with it (you can demonstrate doing smaller commits to reflect the steps of the work)	
* Persistence layer is not mandatory, but it will be a plus the use of any ORM (specially Entity Framework).
* High test coverage is a plus.

## Submission
- Create an account at **GitLab** (it's free)
- Fork the repository `rocs-test`.
- Please make your repository private and add @ryanairLabs account as 'developer'
- We want to see the evolution of your code, so meaningful commits are welcome.
- Please note that the application will be executed on other machines, so don't keep local references.

Please include a `README_Candidate.md` file to be used for any other consideration or explanation that the candidate wants to highlight about the design/implementation process. Last but not least, include the URL of your forked repository in your email notifying Ryanair your code is ready to be reviewed.

---

Thanks for your time, we look forward to hearing from you!

Ryanair Team
