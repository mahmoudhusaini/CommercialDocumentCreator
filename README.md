# My .NET Project – Commercial Document Creator App

A simple ASP.NET Web API application for managing **customer quotations, invoices, and receipts**.  
This project is built with **ASP.NET 8.0 (C#)** for the backend and **pure HTML, CSS, and JavaScript** for the frontend.

> **Note**: This project is optimized for **desktop screens** only and is **not fully mobile responsive**, as the main focus is on showcasing backend and JavaScript functionality.

This is a work-in-progress project, with **new features planned** for future updates.  
Some images and static values are included and can be modified as per your needs.

---

## How to Run the Project

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/commercial-documents-app.git
cd commercial-documents-app
or
Open the solution directly in Visual Studio 2022

### **2. Setup the Database**

Make sure you have SQL Server installed and configured.

Configure your database connection string in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=CommercialDocsDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

Open the Package Manager Console and run the following commands:
Add-Migration InitialCreate
Update-Database

## Tech Stack
- .NET 8.0 – Backend API
- Entity Framework Core – ORM for database operations
- SQL Server
- HTML / CSS / JavaScript – Frontend (no frameworks or libraries used)
