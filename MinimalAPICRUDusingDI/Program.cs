using MinimalAPICRUDusingDI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();

// Add Swagger for API documentation
builder.Services.AddSwaggerGen();


// Register EmployeeService in the DI Container
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();


// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline for the development environment.
if (app.Environment.IsDevelopment())
{
    // Use Swagger middleware to generate Swagger Documentation
    app.UseSwagger();
    // Use Swagger UI middleware to interact with the Swagger documentation
    app.UseSwaggerUI();
}

// CRUD operations for Employee model
// The EmployeeService is injected into the endpoints

// Endpoint to retrieve all employees
app.MapGet("/employees", (IEmployeeService employeeService) => employeeService.GetAllEmployees());

// Endpoint to retrieve a single employee by their ID
app.MapGet("/employees/{id}", (int id, IEmployeeService employeeService) =>
{
    var employee = employeeService.GetEmployeeById(id);
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});


// Endpoint to create a new employee
app.MapPost("/employees", (Employee newEmployee, IEmployeeService employeeService) =>
{
    // Validate the new employee using ValidationHelper
    if(!ValidationHelper.TryValidate(newEmployee, out var validateResults))
    {
        // Return 400 Bad Request if validation fails
        return Results.BadRequest(validateResults);
    }

    // Add the new employee using the EmployeeService
    var createdEmployee = employeeService.AddEmployee(newEmployee);

    // Return 201 Created with the new employee's data
    return Results.Created($"/employees/{createdEmployee.Id}", createdEmployee);
});

// Endpoint to update an existing employee with validation
app.MapPut("/employees/{id}", (int id, Employee updatedEmployee, IEmployeeService employeeService) =>
{
    // Validate the updated employee using ValidationHelper
    if(!ValidationHelper.TryValidate(updatedEmployee, out var validateResults))
    {
        return Results.BadRequest(validateResults);   // Return 400 Bad Request if validation fails
    }

    // Update the employee using the EmployeeService
    var employee = employeeService.UpdateEmployee(id, updatedEmployee);

    // Return 200 Ok if found and updated, otherwise 404 Not Found
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});


// Endpoint to delete an employee
app.MapDelete("/employees/{id}", (int id, IEmployeeService employeeService) =>
{
    var result = employeeService.DeleteEmployee(id);
    return result ? Results.NoContent() : Results.NotFound();
});

// Run the application
app.Run();


