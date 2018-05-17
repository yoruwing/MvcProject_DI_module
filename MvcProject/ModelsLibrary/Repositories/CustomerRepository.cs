﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ModelsLibrary.DtO_Models;
using Dapper;
using Abstracts;

namespace ModelsLibrary.Repositories
{
   public class CustomerRepository : IRepository<Customer>
    {
        public void Create(Customer model)
        {
            var connection = new SqlConnection("data source=.;database=BuildSchool;integrated security=true");
            var sql = "INSERT INTO Customer VALUES(@CustomerName,@Birthday,@Password,@ShoppingCash,@Account,@Phone,@Email)";
            connection.Execute(sql, 
                new {
                        model.CustomerName,
                        model.Birthday,
                        model.Password,
                        model.ShoppingCash,
                        model.Account,
                        model.Phone,
                        model.Email
                    });
        }

        public void Update(Customer model)
        {
            var connection = new SqlConnection("data source=.;database=BuildSchool;integrated security=true");
            var sql = "UPDATE Customer SET CustomerName=@CustomerName,Password=@Password,ShoppingCash=@ShoppingCash,Email=@Email,Phone=@Phone WHERE Account=@Account";
            connection.Execute(sql,
                new {
                        model.Account,
                        model.CustomerName,    
                        model.Password,
                        model.ShoppingCash,
                        model.Email,
                        model.Phone
                    });
        }
        public void Delete(Customer model)
        {
            var connection = new SqlConnection("data source=.;database=BuildSchool;integrated security=true");
            var sql = "DELETE FROM Customer WHERE Account=@Account";
            connection.Execute(sql,new {model.Account});

        }
        public Customer FindByCustomerAccount(string Account)
        {
            var connection = new SqlConnection("data source=.;database=BuildSchool;integrated security=true");
            var sql = "SELECT * FROM Customer WHERE Account=@Account";
            var result = connection.QueryFirstOrDefault<Customer>(sql,new { Account });
            return result;
        }

        public IEnumerable<Customer> GetAll()
        {
            var connection = new SqlConnection("data source=.;database=BuildSchool;integrated security=true");            
            return connection.Query<Customer>("SELECT * FROM Customer");

        }

    }
}
