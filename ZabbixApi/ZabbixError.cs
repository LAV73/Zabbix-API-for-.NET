namespace Ysq.Zabbix
{
    public class ZabbixError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return string.Format("Code: \"{0}\", Data: \"{1}\", Message: \"{2}\"", Code, Data, Message);
        }
    }
}