# Todo Application
![Build Status](https://img.shields.io/badge/build-passing-brightgreen) ![License](https://img.shields.io/badge/license-MIT-blue) [![Docker Image CI](https://github.com/ahammedunnivp/TodoApp/actions/workflows/docker-image.yml/badge.svg?branch=main)](https://github.com/ahammedunnivp/TodoApp/actions/workflows/docker-image.yml)

a simple yet powerful task management system.

[Overview](#overview) | [Features](#features) | [Project Structure](#project-structure) |  [Installation](#installation) | [Usage](#usage) | [API Endpoints](#api-endpoints) | [Testing](#testing) | [Design](#design-decisions)




## Overview
The ToDo application simplifies task management, providing users with a seamless experience to organize their activities. Leveraging the power of .NET Core Web API, the application's backend ensures robust functionality, while the intuitive Blazor UI offers a user-friendly interface.


## Features
- **Effortless Task Management**: View, add, edit, categorize, and delete tasks effortlessly.
- **Task Status Tracking**: Mark tasks as Done to track completion status.
- **Categorisation**: Categorise tasks to better organise and prioritise.
- **Advanced Filtering**: Utilise powerful filtering options to narrow down tasks based on various criteria.
- **Sorting**: Arrange tasks based on difficulty, created order, or any other relevant parameter.
- **Pagination**: Navigate through tasks with ease using pagination features.

## Project Structure
The project is following Clean Architecture/Onion Architecture, and there are 3 micro services independent of each other.
- TodoService
	- Unni.Todo.Domain - C# Library containing Entities specific to the Todo micro-service and further extended to include domain services.
	- Unni.Todo.Application - C# Library containing DTOs, AutoMapper Profiles, Interfaces & Services implementation.
	- Unni.Todo.Infrastructure - C# Library containing specific implementations of DbContext, Repositories & UnitOfWork using EF Core.
	- Unni.Todo.WebAPI - .NET Core Web API project with relevant controllers and middle-wares.
- AdminService
	- Unni.Admin.Domain - C# Library containing Entities specific to the Admin micro-service and further extended to include domain services.
	- Unni.Admin.Application - C# Library containing DTOs, AutoMapper Profiles, Interfaces & Services implementation.
	- Unni.Admin.Infrastructure - C# Library containing specific implementations of DbContext, Repositories & UnitOfWork using EF Core.
	- Unni.Admin.WebAPI - NET Core Web API project with relevant controllers and middle-wares.
- TodoUI
	- Unni.Todo.UI - A blazor UI project handling the user interaction.
## Prerequisites
Before you start, ensure you have the following prerequisites installed on your machine:

1. **.NET SDK:**
   - Download and install the [.NET SDK](https://dotnet.microsoft.com/download).

2. **Integrated Development Environment (IDE):**
   - Choose one of the following:
     - [Visual Studio](https://visualstudio.microsoft.com/): Comprehensive IDE for .NET development.
     - [Visual Studio Code](https://code.visualstudio.com/): Lightweight and extensible code editor.

3. **.NET Runtime:**
   - Included with the .NET SDK installation.
4. **EF Core Command Line Tools:** We are using Entity Framework as the ORM framework, so to initialise Databases we can use command line tools.
    - Ensure that the `dotnet ef` command line tool is available. Install it using the following command:

      ```bash
      dotnet tool install --global dotnet-ef
      ```

## Installation
1. **Clone this repository**: [TodoApp](https://github.com/ahammedunnivp/TodoApp)
2. [**Open the Solution file**](src/Unni.ToDo.sln) in the IDE of your choice.
3. **Restore all the packages**: Restore all the packages using the package manager, from Visual Studio you can right click on the solution and do the operation.
4. **Build the Solution**: Build the solution, it will build all the 4 projects included.
5. **Initialise Database**: This app is using a SQLite to store all the data, which is keeping all the data in disk. We are using Entity Framework as an ORM framework, so the database can be initialised using the following commands. 
	1. Todo DB - Open the terminal from the Unni.Todo.Infrastructure project.
	
	```bash
	   dotnet ef database update -c ToDoDBContext
	```

	2. Admin DB - Open the terminal from the Unni.Admin.Infrastructure project.

	```bash
	   dotnet ef database update -c AdminToDoContext
	```

6. **Configure Startup projects**(optional): In Visual Studio, under Solution settings user can configure which projects to start on  Run.
7. Run the application: Configure to run **Unni.Todo.WebAPI**, **Unni.Admin.WebAPI** & **Unni.ToDo.UI**
	For Visual Studio we can use in-built tools or we can use the following commands
```bash
cd src

dotnet build Unni.Todo.WebAPI
dotnet run --project Unni.Todo.WebAPI

dotnet build Unni.Admin.WebAPI
dotnet run --project Unni.Admin.WebAPI

dotnet build Unni.Todo.UI
dotnet run --project Unni.ToDo.UI

```

## Docker
### Docker Hub URLs

1. **Todo API**: [ahammedunni/todo-api](https://hub.docker.com/r/ahammedunni/todo-api)
2. **Admin API**: [ahammedunni/admin-api](https://hub.docker.com/r/ahammedunni/admin-api)
3. **Todo UI**: [ahammedunni/todo-ui](https://hub.docker.com/r/ahammedunni/todo-ui)

### Getting Started

To run the application locally using Docker, follow these steps:

1. **Create Docker Network:**
	```bash
	   docker network create todonetwork
	```

2. Run Todo API
	```bash
	   docker run -p 5005:8080 --name todoapi --network todonetwork -d ahammedunni/todo-api
	```
 3. Run Admin API
	```bash
	   docker run -p 4004:8080 --name adminapi --network todonetwork -d ahammedunnivp/admin-api
	```    
4. Run Todo UI
	```bash
	   docker run -p 3003:8080 -e "AdminServiceUrl=http://adminapi:8080" -e "TodoServiceUrl=http://todoapi:8080" --name todo-ui --network todonetwork -d ahammedunnivp/todo-ui
	```    
## Usage

The Todo application provides a user-friendly interface for managing tasks. Here's a quick guide on how to use the application:

1. **Open the Application:**
   - Ensure that the `ToDoApp.API` and `ToDoApp.UI` projects are built and running. Follow the steps mentioned in the "Getting Started" section or use the docker method.

2. **Access the ToDo UI:**
   - Open your web browser and navigate to [https://localhost:7071](https://localhost:7071) to access the Todo application. (Port may vary based on your configuration)

3. **View ToDos:**
   - The main page displays a list of todos.

4. **Add a New ToDo:**
   - Click the "Add Todo" button on top of the table to add items
   - Fill in the details, such as title, description, category, etc.
   -  Click "Save" to add the todo.
5. **Add a ToDo Category** (Optional): If you want to Category to the dropdown, and as the AdminUI is not ready at this moment, through an API you can add as many categories as you  like.
```bash
curl -X 'POST' \
  'https://localhost:7282/api/Category' \
  -H 'accept: /' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "Coding",
  "description": "Pending items"
}'
```

6. **Edit a Todo:**
   - To edit a todo, click the "Edit" button next to the todo you want to modify.
   - Update the todo details when the raw becomes editable.
   - Click "Save" to apply the changes.

7. **Mark Todo as Done:**
   - Click the "Edit" button next to the todo you want to modify.
   - Click the checkbox next to a todo to mark it as done. The todo will be considered "Done".

8. **Filter and Sort:**
   - Pagination: The pagination options can be viewed and modified at the bottom right of the table view of the Todos. Paginations include Page Size and Page Number.
   - Filter: As a start a basic filter of displaying only the pending items or all the todos is implemented in UI, which can be found on top left side of the table. 
   - Sorting: Options for sorting can be found bottom left of the table, where you can specify the Sort field and Sort Order.

9. **Delete a Todo:**
   - To delete a todo, click the "Delete" button next to the todo.

10. **API Endpoints (Optional):**
    - For advanced users, interact with the ToDo application programmatically using the provided API endpoints.
    - Also as the AdminUI is not ready, for the time being you can use Admin APIs

## API Endpoints

The ToDo application exposes a set of API endpoints,  here's a summary of the available API endpoints:

### Todo Service

#### Get ToDo by ID

- **Endpoint:** `GET /api/todo/{id}`
- **Description:** Retrieve a specific todo by its ID.
- **Response:** JSON object representing the ToDo.

#### Search Todos

- **Endpoint:** `POST /api/todo/search`
- **Description:** Retrieve a list of todos adhering to the search filter.
- **Response:** JSON object containing todo objects as well as the pagination details.

#### Create New ToDo

- **Endpoint:** `POST /api/todo`
- **Description:** Create a new todo.
- **Request Body:** JSON object with todo details.
- **Response:** JSON object representing the created todo.

#### Update ToDo

- **Endpoint:** `PUT /api/todo/{id}`
- **Description:** Update an existing todo by its ID.
- **Request Body:** JSON object with updated todo details.
- **Response:** JSON object representing the updated todo.

#### Delete ToDo

- **Endpoint:** `DELETE /api/todo/{id}`
- **Description:** Delete a todo by its ID.
- **Response:** 204 No Content on successful deletion.

### Admin Service

#### Get All Categories

- **Endpoint:** `GET /api/category
- **Description:** Retrieve a list of all todo categories.
- **Response:** JSON array containing category objects.

#### Get Category by ID

- **Endpoint:** `GET /api/category/{id}`
- **Description:** Retrieve a specific category by its ID.
- **Response:** JSON object representing the category.

#### Create New Category

- **Endpoint:** `POST /api/category`
- **Description:** Create a new category.
- **Request Body:** JSON object with category details.
- **Response:** JSON object representing the created category.

#### Update Category

- **Endpoint:** `PUT /api/category/{id}`
- **Description:** Update an existing category by its ID.
- **Request Body:** JSON object with updated category details.
- **Response:** JSON object representing the updated category.

#### Delete Category

- **Endpoint:** `DELETE /api/categories/{id}`
- **Description:** Delete a category by its ID.
- **Response:** 204 No Content on successful deletion.


For detailed information on request and response formats, please refer to the API documentation or use Swagger UI for interactive exploration.

## Testing

The Todo application is thoroughly tested to ensure the reliability and correctness of its functionality. The testing strategy includes a combination of unit tests for individual components and in-process integration/API endpoint testing.
### Functional/API Endpoint Testing

In-process functional/API endpoint testing is performed to validate the interactions between different components and ensure the correctness of API responses. There are multiple xUnit projects handling the testing.

- **Unni.Todo.FunctionalTests**
  - In-process tests for API endpoints, including CRUD operations and filtering/sorting.
  - Utilised the `WebApplicationFactory` and `HttpClient` for in-process testing.
  - Ensured proper setup and teardown of test data with in-memory database.

	- **API Response Validation:**
	  - Tests to validate the correctness of API responses, including status codes, headers, and response bodies.
  - Ensured the API follows RESTful principles.
- Unni.Admin.FunctionalTests
	- In-process tests for API endpoints checking the CRUD operations.
	- Utilised the `WebApplicationFactory` and `HttpClient` for in-process testing.
### Unit Tests

#### Controllers
The controllers responsible for handling HTTP requests are unit-tested to ensure they correctly interact with the underlying services.
- **TodoController Tests:**
  - Tests for CRUD operations, input validation, and response handling.
  - Mocked service objects and dependency injection to isolate controller logic.

#### Services
The business logic encapsulated in services is extensively unit-tested to verify its correctness and adherence to requirements.
- **TodoService Tests:**
  - Unit tests for task creation, updating, marking as done, and deletion.
  - Mocked repository and logger objects for isolated testing.

#### Repositories
Data access and persistence functionality provided by repositories are unit-tested to ensure proper interaction with the underlying database.
- **TodoRepository Tests:**
  - Unit tests for database interactions related to tasks.
  - Utilised in-memory database for isolated testing.


The entire test suite consists of approximately 46 tests, covering individual components, interactions between components, and API endpoints. This comprehensive testing approach contributes to the robustness and reliability of the Todo application.


## Running Tests

1. **Open the Solution:**
   - Launch your preferred integrated development environment (IDE) that supports .NET development.
   - Open the Unni.Todo solution.

2. **Build the Solution:**
   - Ensure that the solution is built to resolve all dependencies.
   - This step ensures that the latest code changes are compiled and ready for testing.

3. **Run Unit Tests:**
   - Navigate to the unit test project in the solution.
   - Use the testing features of your IDE to run the unit tests, like Test Explorer in Visual Studio.
   - Alternatively, you can run the tests from the command line using the following command:
     ```bash
     dotnet test
     ```
   - Review the test results to ensure that all tests pass successfully.
## Design Decisions

### Controller - Service - Repository - UnitOfWork 

- **Controller:**
    - Handles incoming HTTP requests and manages the flow of data between the client and the service layer.
    - Responsible for validating inputs, invoking appropriate service methods, and handling responses.
- **Service:**
    - Contains the business logic and application-specific functionality.
    - Orchestrates the interaction between the controller and the repository layer.
- **Repository:**
    - Manages data access and storage.
    - Performs CRUD (Create, Read, Update, Delete) operations on the underlying data storage.
- **UnitOfWork:**
    - Coordinates and manages transactions across multiple repository operations.
    - Ensures data consistency and integrity.

This design pattern promotes separation of concerns, maintainability, and testability of the application.

### Domain - Application - Infrastructure - WebAPI
- Domain
	- This layer represents the core business logic and contains entities.
	- It does not have any dependencies on other layers. It's the innermost layer and contains the pure business logic.
- Application
	- This layer contains application services that orchestrate interactions between the domain layer and the infrastructure layer.
	- It depends on the domain layer but not depend on specific details of the infrastructure.
- Infrastructure
	- This layer is responsible for implementing details that are external to the application, such as databases and frameworks.
	- It depends on the domain layer but not vice versa.
- Web API 
	- This layer exposes the application's functionality to the outside world through APIs.
	- It depends on the application layer to handle business logic, but it does not contains any business logic itself.
### Common C# Library
It serves as a shared resource for these components, ensuring consistency and promoting code reuse across both the API and UI layers. Including DTOs and Interfaces in the common library facilitates seamless communication and interaction between different parts of the application.

### Caching
As a performance optimisation, ToDo App incorporates in-memory caching. Currently, the application caches ToDo Categories with a timer set to expire every 60 seconds. This ensures that the categories are cached for a reasonable duration, reducing the need to fetch the data from the underlying data source on every request. The caching mechanism enhances the overall responsiveness and efficiency of the application.
### Message Broker Integration

The initial design considerations for the App proactively consider potential integrations with a message broker, such as RabbitMQ. In anticipation of future enhancements, particular attention has been given to the separation of CreateToDo requests from their associated DTO or Model. This deliberate design choice serves as a strategic foundation, ensuring a smooth and straightforward integration with a message broker for elevated event-driven communication capabilities and enhanced scalability in the future.
## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
