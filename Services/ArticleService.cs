using ArticleManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
namespace ArticleManagement.Services
{
    public class ArticleService
    {
        private readonly IMongoCollection<Article> _articlesCollection;

        public ArticleService(
       IOptions<ArticleDatabaseSettings> articleDatabaseSettings)
        {
            bool isMongoLive = MongoDBConnectionProbe("mongodb://localhost:27017", "ArticleDB"); //database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000);            
            if (isMongoLive)
            {
                var mongoClient = new MongoClient("mongodb://localhost:27017"); //(articleDatabaseSettings.Value.ConnectionString);
                var mongoDatabase = mongoClient.GetDatabase("ArticleDB"); //(articleDatabaseSettings.Value.DatabaseName);
                _articlesCollection = mongoDatabase.GetCollection<Article>("Articles"); //(articleDatabaseSettings.Value.ArticleCollectionName); 
            }
            else
            {
                // couldn't connect
            }
        }

        public async Task<List<Article>> GetAsync() =>
        await _articlesCollection.Find(_ => true).ToListAsync();

        public async Task<Article?> GetAsync(string id) =>
            await _articlesCollection.Find(x => x.ArticleId == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Article newArticle) =>
            await _articlesCollection.InsertOneAsync(newArticle);

        public async Task UpdateAsync(string id, Article updatedArticle) =>
            await _articlesCollection.ReplaceOneAsync(x => x.ArticleId == id, updatedArticle);

        public async Task RemoveAsync(string id) =>
            await _articlesCollection.DeleteOneAsync(x => x.ArticleId == id);

        private static bool MongoDBConnectionProbe(string connectionString, string dbName)
        {
            var probeTask =
                    Task.Run(() =>
                    {
                        var isAlive = false;
                        var client = new MongoDB.Driver.MongoClient(connectionString);

                        for (var k = 0; k < 6; k++)
                        {
                            client.GetDatabase(dbName);
                            var server = client.Cluster.Description.Servers.FirstOrDefault();
                            isAlive = (server != null &&
                                   server.HeartbeatException == null &&
                                   server.State == MongoDB.Driver.Core.Servers.ServerState.Connected);
                            if (isAlive)
                            {
                                break;
                            }
                            System.Threading.Thread.Sleep(300);
                        }
                        return isAlive;
                    });
            probeTask.Wait();
            return probeTask.Result;
        }
    }
}
