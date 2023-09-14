namespace NewsVowel
{
    internal class NewsResponseModel
    {
        public string? Status { get; set; }
        public int TotalResults { get; set; }
        public IEnumerable<ArticleResponseModel>? Articles { get; set; }
    }

    internal class ArticleResponseModel
    {
        public SourceResponseModel? Source { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? Content { get; set; }
    }

    internal class SourceResponseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
