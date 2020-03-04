namespace Ysq.Zabbix
{
    public class Response
    {
        public int? Id { get; set; }
        public string JsonRpc { get; set; }
        public dynamic Result { get; set; }
        public ZabbixError Error { get; set; }
    }
}
