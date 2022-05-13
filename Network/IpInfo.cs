namespace Zeuratis;

public static class Net
{
    private static readonly HttpClient s_web = new();

    public static async Task<String> GetPublicIP()
    {
        HttpResponseMessage IP = await s_web.GetAsync("https://api64.ipify.org/").ConfigureAwait(false);

        if (IP.IsSuccessStatusCode)
            return await IP.Content.ReadAsStringAsync();

        return "0.0.0.0";
    }
}

public class IP_API_Data
{
    public String? Query { get; set; }
    public String? City { get; set; }
    public String? Country { get; set; }
    public String? CountryCode { get; set; }
    public String? Isp { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public String? Org { get; set; }
    public String? Region { get; set; }
    public String? RegionName { get; set; }
    public String? Status { get; set; }
    public String? Timezone { get; set; }
    public String? Zip { get; set; }
}
