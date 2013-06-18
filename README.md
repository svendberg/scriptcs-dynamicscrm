# Dynamics CRM 2011 Script Pack

## What is it?
A ScriptCs script pack for communication with Dynamics CRM 2011 OrganizationService.
Only tested on-premise so far, but should work for online as well. 

Read more on ScriptCs: http://scriptcs.net/

## Usage
1. Follow the instructions on http://scriptcs.net/ to install ScriptCs. 
3. Navigate to your script folder.
2. Install the DynamicsCrm script pack using nuget to get the latest version.
```csharp
scriptcs -install ScriptCs.DynamicsCrm
```

3. Write your scriptfile

4. Run your scriptfile
```csharp
scriptcs scriptname.csx
```

## Example script
```csharp
var crm = Require<DynamicsCrm>();
var orgService = crm.GetOrganizationService("http://crm2:5555/XrmServices/2011/Discovery.svc", "organization unique name", "user", "password", "domain");
var account = orgService.Retrieve("account", new Guid("B39030B8-F736-E111-9E16-0800277C14DD"), new ColumnSet(true));
Console.WriteLine(account["name"]);
```
To get discovery service address and organization unique name, 
Sign in to your CRM org and click Settings, Customization, Developer Resources.
On Developer Resource page, find the discovery service address under Service Endpoints and organization unique name under Your Organization Information.

