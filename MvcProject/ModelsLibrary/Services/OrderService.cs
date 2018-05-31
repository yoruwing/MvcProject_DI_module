﻿using ModelsLibrary.DtO_Models;
using ModelsLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.Services
{
    public class OrderService
    {
        
        public void Create(Order model)
        {
            var repository = new OrderRepository();
            repository.Create(model);
            
        }

        public void UpdateStatus(Order model)
        {
            var repository = new OrderRepository();
            repository.UpdateStatus(model);
        }

        public Order CheckStatus(int orderId)
        {
            var repository = new OrderRepository();
            return repository.CheckStatus(orderId);
        }

        public Order FindById(int orderId)
        {
            var repository = new OrderRepository();
            return repository.FindById(orderId);
        }

        public IEnumerable<Order> FindCustomerOrderByCustomerID(int customerID)
        {
            var repository = new OrderRepository();
            return repository.FindCustomerOrderByCustomerID(customerID);
        }

        public IEnumerable<Order> GetAll()
        {
            var repository = new OrderRepository();
            return repository.GetAll();
        }

        public void Cancelorder(int orderid)//取消訂單
        {
            var productsRepository = new ProductsRepository();
            var orderRepository = new OrderRepository();
            var orderDetailsRepository = new OrderDetailsRepository();
            //更改status狀態
            var status = orderRepository.FindById(orderid).Status;

            if (status == "申請取消")
                orderRepository.UpdateStatus(new Order { OrderID = orderid, Status = "訂單已取消" });
            if (status == "申請退貨")
                orderRepository.UpdateStatus(new Order { OrderID = orderid, Status = "退貨已完成" });

            //庫存數量調整
            var orderDetails = orderDetailsRepository.FindById(orderid);
            foreach (var item in orderDetails)
            {
                productsRepository.UpdateStockPplus(new Products { ProductID = item.ProductID }, item.Quantity);
            }
        }
    }
}
