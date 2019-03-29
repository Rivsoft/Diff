Prerequisites
=============

* .NET Core 2.2 SDK **[Download Here](https://www.microsoft.com/net/download/dotnet-core/2.2)**
* RabbitMQ **[Download Here](https://www.rabbitmq.com/download.html)**
  
How to run the Web API application
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to API folder: **`cd Diff\Diff.API`**
* Restore nuget packages: **`dotnet restore`**
* Generate the Database: **`dotnet ef database update`**
* Run the application: **`dotnet run`**
* Application should be available on: **[http://localhost:5000/api/](http://localhost:5000/api/)**

How to run the Diff service
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to API folder: **`cd Diff\Diff.Core.Host`**
* Restore nuget packages: **`dotnet restore`**
* Run the application: **`dotnet run`**

How to run the Web API tests
============================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to API folder: **`cd Diff\Diff.API.Tests`**
* Restore nuget packages: **`dotnet restore`**
* Run tests: **`dotnet test`**

How to run Core library unit tests
==================================

* Clone Git Repo: **`git clone https://github.com/Rivsoft/Diff.git Diff`**
* Change to API Tests folder: **`cd Diff\Diff.Core.Tests`**
* Restore nuget packages: **`dotnet restore`**
* Run tests: **`dotnet test`**
