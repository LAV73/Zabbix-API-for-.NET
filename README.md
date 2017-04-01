[![Build Status](https://travis-ci.org/yosqueoy/Zabbix-API-for-.NET.svg)](https://travis-ci.org/yosqueoy/Zabbix-API-for-.NET) 

# Zabbix API for .NET
A thin wrapper of Zabbix API for .NET 4.x
# Installation
NuGet package is available.
https://www.nuget.org/packages/ZabbixApi/
# Overview
This library allows you to send all kind of Zabbix API methods like _host.get_, _item.create_, _trigger.delete_ etc. First, you need to instantiate __ApiClient__ class. The signiture is here.
```C#
public ApiClient(string url, string user, string password)
```
__ApiClient.Call__ method takes a Zabbix API method name and a dynamic object. 
```C#
public Response Call(string method, dynamic param)
```
__Call__ method sends JSON string like this to Zabbix server:
```javascript
{
    "jsonrpc": "2.0",
    "method": "item.get",
    "params": {...},
    "auth": "...",
    "id": 1
}
```
__Call__ method returns the server response as __Response__ object. 
```javascript
{
    "jsonrpc": "2.0",
    "result": [
        {
            itemid: 25,
            ...
        },
        {
            itemid: 33,
            ...
        }
    ],
    "id": 1
}
```
You can access the first itemid by __Response.Result[0].itemid__ dynamic property in your .NET code.
# Examples
## Retrieving triggers in problem state
https://www.zabbix.com/documentation/2.4/manual/api/reference/trigger/get
```C#
var api = new ApiClient("http://zabbix.example.com/api_jsonrpc.php", "user1", "pass");

// Login method stores auth key in the ApiClient instance
api.Login();

// Build JSON request as dynamic object
dynamic param = new ExpandoObject();
param.output = new[] { "triggerid", "description", "priority" };
param.filter = new ExpandoObject();
param.filter.value = 1;
param.sortfield = "priority";
param.sortorder = "DESC";

// Call method sends JSON string to Zabbix server
Response response = api.Call("trigger.get", param);

// Retrieve the results from the response object. Console output is like this.
//   triggerid: 13907
//   description: Zabbix self-monitoring processes < 100% busy
//   priority: 4
//   triggerid: 13824
//   description: Zabbix discoverer processes more than 75% busy
//   priority: 3
foreach (dynamic trigger in response.Result)
{
    Console.WriteLine("triggerid: {0}", trigger.triggerid);
    Console.WriteLine("description: {0}", trigger.description);
    Console.WriteLine("priority: {0}", trigger.priority);
}

// Cleanup
api.Logout();
```
### Generated JSON
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
## Searching by host inventory data
https://www.zabbix.com/documentation/2.4/manual/api/reference/host/get
```C#
var api = new ApiClient("http://zabbix.example.com/api_jsonrpc.php", "user1", "pass");

// Login method stores auth key in the ApiClient instance
api.Login();

// Build JSON request as dynamic object
dynamic param = new ExpandoObject();
param.output = new[] { "host" };
param.selectInventory = new[] { "os" };
param.searchInventory = new Dictionary<string, string>();
param.searchInventory.Add("os", "Linux");

// Call method sends JSON string to Zabbix server
Response response = api.Call("host.get", param);

// Cleanup
api.Logout();
```
### Generated JSON
Request:
```javascript
{
    "jsonrpc": "2.0",
    "method": "host.get",
    "params": {
        "output": [
            "host"
        ],
        "selectInventory": [
            "os"
        ],
        "searchInventory": {
            "os": "Linux"
        }
    },
    "id": 2,
    "auth": "7f9e00124c75e8f25facd5c093f3e9a0"
}
```
Response:
```javascript
{
    "jsonrpc": "2.0",
    "result": [
        {
            "hostid": "10084",
            "host": "Zabbix server",
            "inventory": {
                "os": "Linux Ubuntu"
            }
        },
        {
            "hostid": "10107",
            "host": "Linux server",
            "inventory": {
                "os": "Linux Mint"
            }
        }
    ],
    "id": 1
}
```
