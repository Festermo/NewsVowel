namespace NewsVowel
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            NewsAnalyzer newsAnalyzer = new NewsAnalyzer();

            Console.WriteLine("Do you want to find some news? (y/n)");
            string? answer = Console.ReadLine()?.ToLower();

            while (answer != "n")
            {
                if (answer != "y")
                {
                    Console.WriteLine("Do you want to find some news? (y/n)");
                    answer = Console.ReadLine()?.ToLower();
                    continue;
                }

                Console.WriteLine("What theme are you interested in?");
                string? theme = Console.ReadLine()?.ToLower();

                Console.WriteLine("In which part of news do you want me to search in? (title/content/description)");
                string? partOfNews = Console.ReadLine()?.ToLower();

                Console.WriteLine("Tell me your api key.");
                string? apiKey = Console.ReadLine()?.ToLower();

                await newsAnalyzer.GetWordsWithMostVowels(theme, partOfNews, apiKey);

                Console.WriteLine("Do you want to find more news? (y/n)");
                answer = Console.ReadLine()?.ToLower();
            }
        }
    }
}
