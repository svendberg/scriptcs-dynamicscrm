# Dynamics CRM 2011 scriptcs Script Pack

## What is it?
A scriptcs script pack for communication with Dynamics CRM 2011 OrganizationService.
Only tested on-premise so far, but should work for online as well. 

Read more on scriptcs: http://scriptcs.net/

## Usage
For now you have to clone the repo, compile and copy bin files to you scripts bin folder. Soon it will be avilable on NuGet.

## Example script
```csharp
var crm = Require<DynamicsCrm>();
var orgService = crm.GetOrganizationService("http://crm2:5555/XrmServices/2011/Discovery.svc", "company", "user", "password", "domain");
var account = orgService.Retrieve("account", new Guid("B39030B8-F736-E111-9E16-0800277C14DD"), new ColumnSet(true));
Console.WriteLine(account["name"]);
```

