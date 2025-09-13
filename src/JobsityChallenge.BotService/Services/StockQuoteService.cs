using JobsityChallenge.BotService.Interfaces;

namespace JobsityChallenge.BotService.Services;

public class StockQuoteService(IHttpClientFactory httpClientFactory) : IStockQuoteService
{
    private const string BaseUrl = "https://stooq.com/q/l/";

    public async Task<string> GetStockQuoteAsync(string stockCode)
    {
        if (string.IsNullOrWhiteSpace(stockCode))
            return "Stock code cannot be empty.";

        var client = httpClientFactory.CreateClient(nameof(StockQuoteService));
        var response = await client.GetAsync(BuildUrl(stockCode));

        if (!response.IsSuccessStatusCode)
            return $"Could not retrieve data for stock code {stockCode}.";

        var csvData = await response.Content.ReadAsStringAsync();
        return ParseCsvResponse(stockCode, csvData);
    }

    private static string BuildUrl(string stockCode)
        => $"{BaseUrl}?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

    private static string ParseCsvResponse(string stockCode, string csvData)
    {
        using var reader = new StringReader(csvData);
        reader.ReadLine(); 
        var line = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(line))
            return "No data available.";

        var values = line.Split(',');

        return values.Length >= 5 && !string.IsNullOrEmpty(values[3]) && values[3] != "N/D"
            ? $"{stockCode.ToUpper()} quote is ${values[3]} per share."
            : $"Could not retrieve valid quote for {stockCode}.";
    }
}
