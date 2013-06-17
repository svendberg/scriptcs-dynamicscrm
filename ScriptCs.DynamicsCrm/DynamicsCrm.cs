using Microsoft.Xrm.Sdk;
using ScriptCs.Contracts;

namespace ScriptCs.DynamicsCrm
{
    public class DynamicsCrm : IScriptPackContext
    {
        public IOrganizationService GetOrganizationService(string discoveryService, string organization)
        {
            var app = new CrmConnection(discoveryService, organization);
            return app.GetOrganizationService();
        }

        public IOrganizationService GetOrganizationService(string discoveryService, string organization, string username, string password)
        {
            var app = new CrmConnection(discoveryService, organization, username, password);
            return app.GetOrganizationService();
        }

        public IOrganizationService GetOrganizationService(string discoveryService, string organization, string username, string password, string domain)
        {
            var app = new CrmConnection(discoveryService, organization, username, password, domain);
            return app.GetOrganizationService();
        }
    }
}

