using Newtonsoft.Json.Linq;

public class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGols = 0;
        int paginaAtual = 1;
        bool hasMorePages = true;

        while (hasMorePages)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={paginaAtual}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(responseBody);

            foreach (var match in data["data"])
            {
                totalGols += int.Parse(match["team1goals"].ToString());
            }

            hasMorePages = paginaAtual < (int)data["total_pages"];
            paginaAtual++;
        }

        paginaAtual = 1;
        hasMorePages = true;

        while (hasMorePages)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={paginaAtual}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(responseBody);

            foreach (var match in data["data"])
            {
                totalGols += int.Parse(match["team2goals"].ToString());
            }

            hasMorePages = paginaAtual < (int)data["total_pages"];
            paginaAtual++;
        }

        return totalGols;
    }

}