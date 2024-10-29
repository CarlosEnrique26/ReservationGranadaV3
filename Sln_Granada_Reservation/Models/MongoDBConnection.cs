using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using Sln_Granada_Reservation.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;

namespace Sln_Granada_Reservation.Models
{
    public class MongoDBConnection
    {
        public IMongoDatabase ConnectDB()
        {
            var client = new MongoClient("mongodb://82.223.43.242:27017");
            var database = client.GetDatabase("customvoterDB");
            return database;
        }

        public MongoDBConnection() {
            

        }

        public List<UserMail> GetDocumentParticipant()
        {
            var dbClient = new MongoClient("mongodb://82.223.43.242:27017");
            var database = dbClient.GetDatabase("customvoterDB");

            var collection = database.GetCollection<BsonDocument>("eventmails");
            //enviados
            var filter = Builders<BsonDocument>.Filter.Regex("subject", new BsonRegularExpression(".*asientos.*"));
            var data = collection.Find<BsonDocument>(filter).ToList();

            var listSend = data.Select(v => BsonSerializer.Deserialize<eventMails>(v)).ToList();

            var listUsersSend = new List<UserMail>();
            listSend = listSend.OrderBy(x => x.lastInteraction).ToList();
            foreach (var item in listSend)
            {
                var historyCount = item.history.Count;
                var user = new UserMail { 
                    destination = item.destination,
                    lastEvent = item.lastEvent,
                    lastInteraction = item.history[historyCount - 1].timestamp.ToString()
                };
                listUsersSend.Add(user);
            }


            var filterTwo = Builders<BsonDocument>.Filter.Regex("subject", new BsonRegularExpression(".*reserva.*"));
            var dataTwo = collection.Find<BsonDocument>(filterTwo).ToList();

            var listReservation = dataTwo.Select(v => BsonSerializer.Deserialize<eventMails>(v)).ToList();



            var result = IMongoCollectionExtensions.AsQueryable(collection).ToList();


            var commonFilter = Builders<BsonDocument>.Filter.In("ID_ST", new[] { 2043, 2040 });

            return listUsersSend;
        }
    }
}