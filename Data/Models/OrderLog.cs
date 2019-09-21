using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SwaggerApp.Models;

namespace SwaggerApp.Data.Models
{
    public class OrderLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }   //not validated
        public Merchant Merchant { get; set; }  //validated
        public List<OrderItem> OrderItems { get; set; } //validated
        //public DateTime DateOrdered { get; set; }   //not validated
        //public User User { get; set; }    //gotten from the token
        //public double TotalPrice { get; set; }  //not validated
        public bool UseDefaultLocation { get; } = false; //validated
        public PaymentDetail PaymentDetail { get; set; }   
    }
}