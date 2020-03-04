using System;
using System.Dynamic;
using System.Net;
using Newtonsoft.Json;

namespace Ysq.Zabbix
{
    public class ApiClient
    {
        private readonly string _password;
        private readonly string _url;
        private readonly string _user;
        private string _auth;

        public ApiClient(string url, string user, string password)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new UriFormatException();
            }

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            _url = url;
            _user = user;
            _password = password;
        }

        public Response Call(string method, dynamic param)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            var request = new Request { Auth = _auth, Method = method, Params = param };
            var jsonRequest = request.ToJsonString();
            string jsonResponse;
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json-rpc");
                jsonResponse = System.Text.Encoding.UTF8.GetString(wc.UploadData(_url, System.Text.Encoding.UTF8.GetBytes(jsonRequest)));
            }

            var response = JsonConvert.DeserializeObject<Response>(jsonResponse);
            if (response.Error != null)
            {
                throw new InvalidOperationException(response.Error.ToString());
            }

            return response;
        }

        public void Login()
        {
            dynamic parameters = new ExpandoObject();
            parameters.user = _user;
            parameters.password = _password;
            var response = Call("user.login", parameters);
            _auth = response.Result;
        }

        public void Logout()
        {
            if (_auth == null)
            {
                return;
            }

            var response = Call("user.logout", null);
            if (response.Result != true)
            {
                throw new InvalidOperationException("user.logout failed");
            }

            _auth = null;
        }
    }
}
