var crm = Require<DynamicsCrm>();
var orgService = crm.GetOrganizationService("http://crm/XrmServices/2011/Discovery.svc", "company", "user", "password", "domain");
QueryByAttribute querybyattribute = new QueryByAttribute("account");
querybyattribute.ColumnSet = new ColumnSet("name");
querybyattribute.Attributes.AddRange("address1_city");
querybyattribute.Values.AddRange("Redmond");
EntityCollection retrieved = orgService.RetrieveMultiple(querybyattribute);
foreach(var account in retrieved.Entities)
{
	Console.WriteLine(account["name"]);
}