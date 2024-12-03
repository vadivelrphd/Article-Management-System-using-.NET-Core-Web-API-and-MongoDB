using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ArticleManagement.Models
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ArticleId { get; set; }

        [BsonElement("ArticleName")]
        public string ArticleName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Author { get; set; } = null!;
    }
}
