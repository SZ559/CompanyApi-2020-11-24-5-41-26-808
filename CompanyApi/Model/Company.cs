﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApi.Model
{
    public class Company : IEquatable<Company>
    {
        public Company(string name)
        {
            Name = name;
        }

        public Company()
        {
        }

        public string CompanyID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public List<Employee> Employees { get; set; }

        public bool Equals(Company company)
        {
            if (company == null)
            {
                return false;
            }

            if (company.GetType() != this.GetType())
            {
                return false;
            }

            return Name == company.Name && CompanyID == company.CompanyID;
        }
    }
}
