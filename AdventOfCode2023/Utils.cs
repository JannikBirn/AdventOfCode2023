public static class Utils
{
    static string CacheDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AdventOfCode" , "2023");

    static string TokenFilePath = Path.ChangeExtension(Path.Combine(CacheDir, "token"), ".txt");

    static HttpClient _client;

    public static string GetAdventInput(string url)
    {
        var urlPath = url.Replace("https://", string.Empty).Replace(".", string.Empty).Replace('/', '_');
        var filePath = Path.ChangeExtension(Path.Combine(CacheDir, urlPath), ".txt");
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }

        var client = GetClient();
        var responseTask = client.GetAsync(url);
        responseTask.Wait();
        var response = responseTask.Result;
        var resultTask = response.Content.ReadAsStringAsync();
        resultTask.Wait();
        var result = resultTask.Result;
        Directory.CreateDirectory(CacheDir);
        File.WriteAllText(filePath, result);
        return result;
    }

    static HttpClient GetClient()
    {
        if (_client != null) return _client;

        _client = new HttpClient();
        var token = string.Empty;
        if (File.Exists(TokenFilePath))
        {
            token = File.ReadAllText(TokenFilePath);
        }
        else
        {
            Console.WriteLine("Advent of Code Session token is requiered to download the input data. Please input full session token:");
            token = Console.ReadLine();

            var answer = string.Empty;
            do {
                Console.WriteLine("Do you want to cache the token? [Warning: Insecure] (y/n)");
                answer = Console.ReadLine();                

            } while (answer != "y" && answer != "n");

            if (answer == "y")
            {
                File.WriteAllText(TokenFilePath, token);
            }
        }

        _client.DefaultRequestHeaders.Add("Cookie", token);

        return _client;
    }
}