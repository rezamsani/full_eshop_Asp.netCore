using Application.Interfaces.Contexts;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Contexts.MongoContext
{
    public class MongoDbContext<T> : IMongoDbContext<T>
    {

        private readonly IMongoDatabase db;
        private readonly IMongoCollection<T> mongoCollection;
        private readonly IConfiguration _configuration;
        public MongoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var client = new MongoClient();
            db = client.GetDatabase(_configuration["MongoDbName"]);
            mongoCollection = db.GetCollection<T>(typeof(T).Name); // "Product" === typeof(T).Name
        }

        public IMongoCollection<T> GetCollection()
        {
            return mongoCollection;
        }
    }
}
