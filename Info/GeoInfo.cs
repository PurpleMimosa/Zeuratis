namespace Zeuratis;

class GeoLocator
{
    private readonly GeoInfo _geoInfo = new();
    private static readonly HttpClient s_web = new();

    public GeoLocator()
    {
        IP_API_Data IPData;

        s_web.DefaultRequestHeaders.Accept.Add(new("application/json"));

        HttpResponseMessage response = s_web.GetAsync("http://ip-api.com/json/" + Net.GetPublicIP()
            .GetAwaiter()
            .GetResult())
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        if (response.IsSuccessStatusCode)
        {
            IPData = JsonConvert.DeserializeObject<IP_API_Data>(response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult())!;

            _geoInfo.Zip = IPData.Zip;
            _geoInfo.Country = IPData.Country;
            _geoInfo.CountryCode = IPData.CountryCode;
            _geoInfo.RegionName = IPData.RegionName;
            _geoInfo.Region = IPData.Region;
            _geoInfo.Timezone = IPData.Timezone;
            _geoInfo.Lat = IPData.Lat;
            _geoInfo.Lon = IPData.Lon;
            _geoInfo.City = IPData.City;
        }
    }

    public GeoInfo GetAll()
    {
        return _geoInfo;
    }
}

public class GeoInfo
{
    public String? City { get; set; }
    public String? Country { get; set; }
    public String? CountryCode { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public String? Region { get; set; }
    public String? RegionName { get; set; }
    public String? Timezone { get; set; }
    public String? Zip { get; set; }
}
