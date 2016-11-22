using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Persistance
{
    public interface IMongoConnection
    {
        IMongoDatabase Database { get; }
    }

    public class MongoConnection : IMongoConnection
    {

        public MongoConnection(string connectionString)
        {
            var url = new MongoUrl(connectionString);
            Database = new MongoClient(url).GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase Database
        {
            get;
        }
    }
}
