using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("[controller]")]
[ApiController]

public class EmployesController : ControllerBase

{
    private readonly ApplicationDbContext applicationDbContext;
    public EmployesController(ApplicationDbContext dbContext)
    {
        this.applicationDbContext = dbContext;
    }
    [HttpGet("/hello")]
    public IActionResult GetAllEmployee(){
        var result = applicationDbContext.Employes.ToList();
        return Ok(result);
    }
    [HttpPost]
    public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto){
        var employeeentity = new Employee(){
            Salary = addEmployeeDto.Salary,
            Phone = addEmployeeDto.Phone,
            Email = addEmployeeDto.Email,
            Name = addEmployeeDto.Name
        };
        applicationDbContext.Employes.Add(employeeentity);
        applicationDbContext.SaveChanges();
        return Ok(employeeentity);
    }
    [HttpGet]
    [Route("{Id:Guid}")]
    public IActionResult GetEmployeById(Guid Id)
    {
      var result = applicationDbContext.Employes.Find(Id);
      return Ok(result);
    }
    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult UpdateEmployee(System.Guid id, UpdateEmployeeDto updateEmployeeDto){
        var employee = applicationDbContext.Employes.Find(id);
        if(employee == null){
            return NotFound();
        }
        employee.Email = updateEmployeeDto.Email;
        employee.Salary = updateEmployeeDto.Salary;
        employee.Name = updateEmployeeDto.Name;
        employee.Phone = updateEmployeeDto.Phone;
        applicationDbContext.SaveChanges();
        return Ok();
    }
    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult DeleteEmploye(Guid id)
    {
        var employee = applicationDbContext.Employes.Find(id);
        applicationDbContext.Employes.Remove(employee);
        applicationDbContext.SaveChanges();
        return Ok();
    }
}