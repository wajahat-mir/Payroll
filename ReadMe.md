# Payroll API

Simple Payroll API that allows to Get/Post Reports on Employee payrolls.
You can see the Db Structure DB Creation scripts under Payroll.API/Resources/DbCreationScript.sql

## How was testing completed?

* Quick setup of Swagger for Dev Testing
* Added some Unit tests, more certainly can be added 

## Changes if this API was destined for Production?

* Add Employees in Bulk via a Table Type and a Merge Statement (the function has been written but, I am facing issues in setting up a Table Type on a localdb)
* Add a Production appsettings.json
* Use Data Models in Repo instead of leveraging Bll Models
* Add Authentication middleware
* Improve Error Handling and error messaging
* Implement Unit Of Work so, that all queries are completed in one transaction
* Implement a Builder Pattern for Mock Repositories for Unit Testing
* Allow only HTTPS
* Apply a CORS policy

## Compromises made?

* Employees Pay Rate cannot change instead a new employeeId is provided in such a scenario


