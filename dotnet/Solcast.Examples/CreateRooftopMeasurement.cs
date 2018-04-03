using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Solcast.Examples
{
    public class CreateRooftopMeasurement
    {
        public async Task Run()
        {
            var API_KEY = "[insert your api key here]";
            var SOLCAST_API_URL = "https://api.solcast.com.au/rooftop_sites";
            var RESOURCE_ID = "[insert the resource id of the rooftop site you are posting measurements for]";
            
            var measurement = new RooftopMeasurement(new DateTime(2018, 2, 2, 3, 30, 0), "PT5M", 1.2345);
            var client = new CreateRooftopMeasurementClient(API_KEY, SOLCAST_API_URL);

            await client.PostAsync(measurement, RESOURCE_ID);
        }
    }

    public class RooftopMeasurement
    {
        public RooftopMeasurement(DateTime periodEnd, string period, double totalPower)
        {
            this.PeriodEnd = periodEnd;
            this.Period = period;
            this.TotalPower = totalPower;
        }
        
        [JsonProperty(PropertyName = "period_end")]
        public DateTime PeriodEnd { get; }
        
        [JsonProperty(PropertyName = "period")]
        public string Period { get; }
        
        [JsonProperty(PropertyName = "total_power")]
        public double TotalPower { get; }
    }

    public class CreateRooftopMeasurementClient
    {
        private readonly string ApiKey;
        private readonly string Url;
        
        public CreateRooftopMeasurementClient(string apiKey, string url)
        {
            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            this.ApiKey = apiKey;
            this.Url = url;
        }

        public async Task<HttpResponseMessage> PostAsync(RooftopMeasurement measurement, string resourceId)
        {
            if (measurement == null) throw new ArgumentNullException(nameof(measurement));
            if (string.IsNullOrEmpty(resourceId)) throw new ArgumentNullException(nameof(resourceId));

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(measurement);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                return await client.PostAsync($"{Url}/{resourceId}/measurements?format=json&api_key={ApiKey}", content);
            }
        }
    }
}