using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ScriptCs.DynamicsCrm.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicsCrm dynamicsCrm = new DynamicsCrm();

            var orgService = dynamicsCrm.GetOrganizationService("", "", "", "", "");
            QueryByAttribute querybyattribute = new QueryByAttribute("account");
            querybyattribute.ColumnSet = new ColumnSet("name");
            querybyattribute.Attributes.AddRange("address1_city");
            querybyattribute.Values.AddRange("Redmond");
            EntityCollection retrieved = orgService.RetrieveMultiple(querybyattribute);
            foreach (var account in retrieved.Entities)
            {
                Console.WriteLine(account["name"]);
            }
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
