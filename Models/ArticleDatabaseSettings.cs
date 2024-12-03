namespace ArticleManagement.Models
{
    public class ArticleDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ArticleCollectionName { get; set; } = null!;
    }
}
