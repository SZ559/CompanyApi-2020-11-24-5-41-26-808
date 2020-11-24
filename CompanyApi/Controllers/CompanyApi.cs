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
    public static class FakeDatabase
    {
        public static IList<Company> Companies { get; } = new List<Company>();
        public static void ClearCompanies()
        {
            Companies.Clear();
        }

        public static bool ContainsCompanyName(Company company)
        {
            return Companies.FirstOrDefault(companyInMemory => companyInMemory.Name == company.Name) != null;
        }

        public static Company GetCompanyByID(string id)
        {
            return Companies.FirstOrDefault(companyInMemory => companyInMemory.CompanyID == id);
        }
    }

    [ApiController]
    [Route("Companies")]
    public class CompanyApi : ControllerBase
    {
        [HttpPost]
        public ActionResult<Company> AddNewCompany(Company company)
        {
            if (FakeDatabase.ContainsCompanyName(company))
            {
                return Conflict();
            }

            company.CompanyID = Guid.NewGuid().ToString();
            FakeDatabase.Companies.Add(company);

            var response = new ObjectResult(company)
            {
                StatusCode = (int)HttpStatusCode.OK,
            };

            Response.Headers.Add("Location", $"/Companies/{company.CompanyID}");
            return response;
        }

        [HttpGet]
        public ActionResult<Company> GetAllCompanies([FromQuery] int? pageSize, [FromQuery] int? pageIndex)
        {
            if (pageSize.HasValue && pageIndex.HasValue)
            {
                var startIndex = (pageIndex.Value - 1) * pageSize.Value;
                var endIndex = startIndex + pageSize.Value - 1;
                return Ok(FakeDatabase.Companies.Where((company, index) => index >= startIndex && index <= endIndex).ToList());
            }

            return Ok(FakeDatabase.Companies);
        }

        [HttpGet("{companyID}")]
        public ActionResult<Company> GetAllCompanies(string companyID)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPatch("{companyID}")]
        public ActionResult<Company> UpdateCompanyInformation(string companyID, CompanyUpdatedModel updatedModel)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null)
            {
                return NotFound();
            }

            company.Name = updatedModel.Name;

            return Ok(company);
        }

        [HttpDelete("{companyID}")]
        public ActionResult<Company> DeleteCompany(string companyID)
        {
            var company = FakeDatabase.GetCompanyByID(companyID);

            if (company == null)
            {
                return NotFound();
            }

            FakeDatabase.Companies.Remove(company);
            return NoContent();
        }
    }
}
