﻿using Abstracts;
using Dapper;
using ModelsLibrary.DtO_Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ModelsLibrary.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private string sqlstr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        public void Create(Order model)  //新增訂單
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"INSERT INTO [Order](OrderDay,CustomerID,Transport,Payment,Status,StatusUpdateDay,Address) 
                        VALUES (GETDATE() , @CustomerID , @Transport , @Payment , @Status , GETDATE(),@Address)";
            connection.Execute(sql, 
                new {
                    model.CustomerID,
                    model.Transport,
                    model.Payment,
                    model.Status,
                    model.Address
                });

        }

        public void Delete(Order model)
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = "DELETE FROM [Order] WHERE OrderID=@OrderID";
            connection.Execute(sql, new { model.OrderID});

        }

        public void UpdateStatus(Order model)  //修改訂單狀態
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"UPDATE [Order] 
                        SET Status = @Status , StatusUpdateDay = GETDATE()
                        WHERE OrderID = @OrderID";
            connection.Execute(sql,
                new
                {
                    model.OrderID,
                    model.Status,
                });

        }

        public Order CheckStatus(int orderId) //查詢訂單狀態
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"SELECT Status
                        FROM [Order]
                        WHERE OrderID = @OrderID";
            var result =  connection.QueryMultiple(sql , new {orderId});
            var orders = result.Read<Order>().Single();

            return orders;

        }
        
        public Order FindById(int orderId) //用id查詢
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"SELECT * FROM [Order] 
                                    WHERE OrderID = @OrderID";
            var result = connection.Query<Order>(sql, new { orderId }).ToList();
            if (result.Count() == 0)
            {
                return null;
            }
            return result.Single();         
        }

        public int FindLastOrderByCustomerID(int customerID) //用id查詢
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"SELECT TOP 1 * FROM [Order] 
                        WHERE CustomerID = @CustomerID
                        ORDER BY OrderDay DESC";
            var result = connection.QueryMultiple(sql, new { customerID });
            var orders = result.Read<Order>().Single();

            return orders.OrderID;
        }

        public IEnumerable<Order> FindCustomerOrderByCustomerID(int customerID) //同一個客戶的所有訂單
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"SELECT * FROM [Order] WHERE CustomerID = @CustomerID";
            return connection.Query<Order>(sql, new { customerID });
        }

        public IEnumerable<Order> GetAll()  //查詢所有資料
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            return connection.Query<Order>("SELECT * FROM [Order]");
        }

        public void Update(Order model)  //修改訂單
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"UPDATE [Order] 
                        SET Transport = @Transport,Payment = @Payment ,Status = @Status , StatusUpdateDay = GETDATE(),Address = @Address
                        WHERE OrderID = @OrderID";
            connection.Execute(sql,
                new
                {
                    model.OrderID,
                    model.Transport,
                    model.Payment,
                    model.Status,
                    model.Address
                });

        }

        public IEnumerable<Order> ChoiceStatus(string Status) //查詢訂單狀態
        {
            SqlConnection connection = new SqlConnection(sqlstr);
            var sql = @"SELECT *
                        FROM [Order]
                        WHERE Status = @Status";
            var result = connection.Query<Order>(sql, new { Status });
            return result;

        }
    }
}

