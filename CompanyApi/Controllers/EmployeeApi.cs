﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CompanyApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("Companies/{companyID}/Employees")]
    public class EmployeeApi : ControllerBase
    {
        [HttpPost]
        public ActionResult<Company> AddNewEmployee(string companyID, Employee employee)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null)
            {
                return NotFound();
            }

            if (company.Employees == null)
            {
                company.Employees = new List<Employee>();
            }

            company.Employees.Add(employee);

            return CreatedAtAction(nameof(GetEmployee), new { companyID = employee.EmployeeID },
            employee);
        }

        [HttpGet]
        public ActionResult<Company> GetEmployee(string companyID)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company.Employees);
        }

        [HttpPatch("{employeeID}")]
        public ActionResult<Company> UpdateEmployeeInformation(string companyID, string employeeID, EmployeeDto employeeUpdatedModel)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null || company.Employees == null)
            {
                return NotFound();
            }

            var employee = company.Employees.FirstOrDefault(employee => employee.EmployeeID == employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = employeeUpdatedModel.Name == null ? employee.Name : employeeUpdatedModel.Name;
            employee.Salary = employeeUpdatedModel.Salary == null ? employee.Salary : employeeUpdatedModel.Salary.Value;
            return Ok(employee);
        }

        [HttpDelete("{employeeID}")]
        public ActionResult<Company> DeleteEmployee(string companyID, string employeeID)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null || company.Employees == null)
            {
                return NotFound();
            }

            var employee = company.Employees.FirstOrDefault(employee => employee.EmployeeID == employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            company.Employees.Remove(employee);
            return NoContent();
        }
    }
}
