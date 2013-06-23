using Microsoft.Xrm.Sdk;
using ScriptCs.Contracts;

namespace ScriptCs.DynamicsCrm
{
    public class DynamicsCrm : IScriptPackContext
    {
        public IOrganizationService GetOrganizationService(string discoveryService, string organization, string username = null, string password = null, string domain = null)
        {
            var app = new CrmConnection(discoveryService, organization, username, password, domain);
            return app.GetOrganizationService();
        }
    }
}

