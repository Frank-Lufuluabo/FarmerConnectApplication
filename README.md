# 🌾 FarmerConnectApplication

FarmerConnectApplication is a web-based platform built with **ASP.NET Core MVC**, **Entity Framework Core**, and **ASP.NET Core Identity** that allows farmers to register, log in, and manage their own products. Each farmer can only view and edit the products they created, ensuring privacy and data separation.

##  Features

-  Farmer registration with automatic user and role creation
-  Secure login with role-based access control
-  Farmers can only view/edit their own products
-  Product CRUD (Create, Read, Update, Delete)
-  Identity system with role-based authorization
-  Razor Pages and MVC integrated
-  Developer-friendly structure for future scaling
-  Login link on the registration page
-  Cancel buttons on all relevant forms

## Setup Instructions

### Prerequisites
- .NET SDK version 8.0 or later
-  SQL Server

### Steps to Setup the Project

1. Clone the the repository or download the project files 

2. Navigate to the `FarmerConnectApplication` directory
```
cd FarmerConnectApplication
```
3. Update the Database Connection
```
Open appsettings.json and update the DefaultConnection string to match SQL Server environment.
```
4. Restore .NET Dependencies
```sh
dotnet restore
```
5. Run Migration
```
add-migration InitialMigration and update-database 
```
6. Run the Application
```
dotnet run
```

## Show your support

Give a ⭐️ if you like this project!
   