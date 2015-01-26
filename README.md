# Zabbix API for .NET
A thin wrapper of Zabbix API for .NET
# Getting Started
##Retrieving triggers in problem state
https://www.zabbix.com/documentation/2.4/manual/api/reference/trigger/get
```C#
var api = new ApiClient("http://zabbix.example.com/api_jsonrpc.php", "user1", "pass");

// Login method stores auth key in the ApiClient instance
api.Login();

// Build JSON request as dynamic object
dynamic param = new ExpandoObject();
params.output = new List<string>{ "triggerid", "description", "priority" };
param.filter = new ExpandoObject();
param.filter.value = 1;
param.sortfield = "priority";
param.sortorder = "DESC";

// Call method sends JSON string to Zabbix server
Response response = api.Call("trigger.get", param);

// Cleanup
api.Logout();
```
###Generated JSON
Request:
```javascript
{
    "jsonrpc": "2.0",
    "method": "trigger.get",
    "params": {
        "output": [
            "triggerid",
            "description",
            "priority"
        ],
        "filter": {
            "value": 1
        },
        "sortfield": "priority",
        "sortorder": "DESC"
    },
    "auth": "038e1d7b1735c6a5436ee9eae095879e",
    "id": 1
}
```
Response:
```javascript
{
    "jsonrpc": "2.0",
    "result": [
        {
            "triggerid": "13907",
            "description": "Zabbix self-monitoring processes < 100% busy",
            "priority": "4"
        },
        {
            "triggerid": "13824",
            "description": "Zabbix discoverer processes more than 75% busy",
            "priority": "3"
        }
    ],
    "id": 1
}
```
