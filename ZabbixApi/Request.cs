using Newtonsoft.Json;

namespace Ysq.Zabbix
{
    public class Request
    {
        public Request()
        {
            JsonRpc = "2.0";
            Id = 1;
        }

        [JsonProperty(PropertyName = "auth")]
        public string Auth { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "params")]
        public dynamic Params { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(
                this, 
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}