Prerequisites
=============

* .NET Core 2.2 SDK **[Download Here](https://www.microsoft.com/net/download/dotnet-core/2.2)**
* RabbitMQ **[Download Here](https://www.rabbitmq.com/download.html)**
  
How to run the Web API application
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to folder: **`cd Diff\Diff.API`**
* Restore nuget packages: **`dotnet restore`**
* Generate the Database: **`dotnet ef database update`**
* Make sure both Ingestion and Analysis services are running
* Run the application: **`dotnet run`**
* Application should be available on: **[http://localhost:5000/v1/diff/](http://localhost:5000/v1/diff/)**

How to run the Diff Ingestion service
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to folder: **`cd Diff\Diff.Core.IngestionService`**
* Restore nuget packages: **`dotnet restore`**
* Run the application: **`dotnet run`**

How to run the Diff Analysis service
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to folder: **`cd Diff\Diff.Core.AnalysisService`**
* Restore nuget packages: **`dotnet restore`**
* Run the application: **`dotnet run`**

How to run the Web API integration tests
============================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to folder: **`cd Diff\Diff.API.Integration.Tests`**
* Restore nuget packages: **`dotnet restore`**
* Make sure both Ingestion and Analysis services are running
* Run tests: **`dotnet test`**

How to run the Web API unit tests
============================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to folder: **`cd Diff\Diff.API.Tests`**
* Restore nuget packages: **`dotnet restore`**
* Run tests: **`dotnet test`**

How to run Core library unit tests
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to folder: **`cd Diff\Diff.Core.Tests`**
* Restore nuget packages: **`dotnet restore`**
* Run tests: **`dotnet test`**

How to run Data library unit tests
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to folder: **`cd Diff\Diff.Data.Tests`**
* Restore nuget packages: **`dotnet restore`**
* Run tests: **`dotnet test`**

Improvements
============

* Adopt an orchestrator (like Kubernetes or Service Fabric to name a few) to allow for automatic Ingestion and Analysis services scalling.
* Containerize the solutions to allow for better execution and/or deployment of integration tests and application modules. 
  Adding docker compose for example for Integration tests or Web API would allow for a more automated and streamlined run execution.
* Add Load testing
* Add Swagger documentation for the Web API
* Add centralized log functionality using for example ELK stack
