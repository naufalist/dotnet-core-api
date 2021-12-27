using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public class MongodbContext
    {
        private readonly IMongoDatabase _db;

        public MongodbContext(IMongoClient mongoClient, string databaseName)
        {
            _db = mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<StudentOutput> Students => _db.GetCollection<StudentOutput>("students");
    }
}
