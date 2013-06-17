using System;
using System.Diagnostics.Contracts;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using ScriptCs.Contracts;

namespace ScriptCs.DynamicsCrm
{
    public class DynamicsCrm : IScriptPackContext
    {
       public IOrganizationService GetOrganizationService(string server, string organization)
       {
          var clientCreds = new ClientCredentials();
          var orgConfigInfo = ServiceConfigurationFactory.CreateConfiguration<IOrganizationService>(new Uri(server + "/" + organization + "/XRMServices/2011/Organization.svc"));
          var orgserv = new OrganizationServiceProxy(orgConfigInfo, clientCreds);
          orgserv.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
          return orgserv;
       }
    }
}
