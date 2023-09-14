using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NewsVowel
{
    internal class NewsAnalyzer
    {
        private readonly HttpClient _client;
        private readonly string[] _languages = { "ru", "en" };
        private readonly JsonSerializerOptions _serializeOptions;

        public NewsAnalyzer()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "C# App"); // to avoid error with anonymous requests

            _serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task GetWordsWithMostVowels(string? theme, string? partOfNews, string? apiKey)
        {
            string baseUrl = "https://newsapi.org/v2/everything"; //better to put it in another file/config
            string defaultTheme = "космос";

            try
            {
                if (string.IsNullOrEmpty(theme))
                {
                    theme = defaultTheme;
                }

                if (string.IsNullOrEmpty(partOfNews) || (partOfNews != "title" && partOfNews != "content" && partOfNews != "description"))
                {
                    throw new Exception("You can only search in title, content and description.");
                }

                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new Exception("Api key can't be empty.");
                }

                foreach (var language in _languages)
                {
                    string query = baseUrl + "?q=" + theme + "&searchIn=" + partOfNews + "&apiKey=" + apiKey + "&language=" + language;

                    var response = await _client.GetAsync(query);

                    if (!response.IsSuccessStatusCode)
                    {
                        ErrorResponseModel? error = await response.Content.ReadFromJsonAsync<ErrorResponseModel>(_serializeOptions);

                        throw new Exception(error?.Message);
                    }

                    NewsResponseModel? news = await response.Content.ReadFromJsonAsync<NewsResponseModel>(_serializeOptions);

                    if (news == null || news.Articles == null)
                    {
                        throw new Exception("Problems with response from server.");
                    }

                    if (news.Articles.Count() == 0)
                    {
                        throw new Exception("Can't find any news with this parameters.");
                    }

                    foreach (var article in news.Articles)
                    {
                        var punctuation = article.Description?.Where(c => !Char.IsLetterOrDigit(c)).Distinct().ToArray(); //all possible separators

                        var words = article.Description?.Split(punctuation);

                        var regex = new Regex("(a|e|i|o|u|A|E|I|O|U|а|я|у|ю|о|ё|э|и|ы|А|Я|У|Ю|О|Ё|Э|И|Ы)"); //all vowels in English and Russian (better to declare it in some helper)

                        var mostVowels = words?.Max(y => regex.Matches(y).Count);

                        var wordWithMostVowels = words?.First(x => regex.Matches(x).Count == mostVowels); //getting first word with maximum amount of vowels

                        Console.WriteLine("Description: " + article.Description);
                        Console.WriteLine("Word with most vowels: " + wordWithMostVowels);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
