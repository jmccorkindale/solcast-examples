using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Solcast.Examples
{
    /// <summary>
    /// To send rooftop sites to the Solcast API, the following attributes need to present in the payload:
    ///     - name: a friendly name of the rooftop site
    ///     - longitude: the longitude of the rooftop site
    ///     - latitude: the latitude of the rooftop site
    ///     - azimuth (optional): azimuth of the rooftop site (180 to -180) where 0 is North
    ///     - tilt (optional): tilt of the rooftop site (0 to 90) where 0 is flat.
    /// </summary>
    public class RooftopSite
    {   
        public RooftopSite(string name, double longitude, double latitude, double? azimuth = null, double? tilt = null)
        {
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
            Azimuth = azimuth;
            Tilt = tilt;
        }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; }
        
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; }
        
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; }
        
        [JsonProperty(PropertyName = "azimuth")]
        public double? Azimuth { get; }
        
        [JsonProperty(PropertyName = "tilt")]
        public double? Tilt { get; }
    }

    /// <summary>
    /// Converts the rooftop site to a json string and performs an asynchronous post request to the Solcast API.
    /// </summary>
    public class CreateRooftopSiteClient
    {
        private readonly string ApiKey;
        private readonly string Url;
        
        public CreateRooftopSiteClient(string apiKey, string url)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            this.ApiKey = apiKey;
            this.Url = url;
        }

        public async Task<HttpResponseMessage> PostAsync(RooftopSite site)
        {
            if (site == null) throw new ArgumentNullException(nameof(site));

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(site);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                return await client.PostAsync($"{Url}?format=json&api_key={ApiKey}", content);
            }
        }
    }
    
    public class CreateRooftopSite
    {
        public async Task<string> Run()
        {
            var API_KEY = "[insert your api key here]";
            var SOLCAST_API_URL = "https://api.solcast.com.au/rooftop_sites";
            
            var site = new RooftopSite("My Site", -149.117, 35.2);
            var client = new CreateRooftopSiteClient(API_KEY, SOLCAST_API_URL);

            var response = await client.PostAsync(site);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}