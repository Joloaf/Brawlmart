using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace BrawlmartTest
{
    internal class MongoConnection
    {
        private static MongoClient GetClient()
        {
            string connectionString = "mongodb+srv://joachimcarlsson:Admin1234@cluster0.rslea.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls13 };

            var client = new MongoClient(settings);
            return client;
        }

        public static IMongoCollection<Models.MongoData> CollectInterest()
        {
            var client = GetClient();
            var database = client.GetDatabase("Brawlmart");
            var interestCollection = database.GetCollection<Models.MongoData>("ProductInterest");
            return interestCollection;
        }
    }
}
