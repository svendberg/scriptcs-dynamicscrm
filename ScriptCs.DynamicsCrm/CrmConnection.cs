using System;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;

namespace ScriptCs.DynamicsCrm
{
    /// <summary>
    /// Modified version of code found in Dynamics CRM SDK
    /// </summary>
    public class CrmConnection
    {
        // To get discovery service address and organization unique name, 
        // Sign in to your CRM org and click Settings, Customization, Developer Resources.
        // On Developer Resource page, find the discovery service address under Service Endpoints and organization unique name under Your Organization Information.
        private readonly string _discoveryServiceAddress; 
        private readonly string _organizationUniqueName; 
        private readonly string _userName; 
        private readonly string _password; 

        // Provide domain name for the On-Premises org.
        private readonly string _domain;

        public CrmConnection(string discoveryServiceAddress, string organizationUniqueName, string username = null, string password = null, string domain = null)
        {
            _discoveryServiceAddress = discoveryServiceAddress;
            _organizationUniqueName = organizationUniqueName;
            _userName = username;
            _password = password;
            _domain = domain;
        }


        public IOrganizationService GetOrganizationService()
        {
            IServiceManagement<IDiscoveryService> serviceManagement = ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(new Uri(_discoveryServiceAddress));
            AuthenticationProviderType endpointType = serviceManagement.AuthenticationType;

            // Set the credentials.
            AuthenticationCredentials authCredentials = GetCredentials(endpointType);


            String organizationUri = String.Empty;
            // Get the discovery service proxy.
            using (DiscoveryServiceProxy discoveryProxy = GetProxy<IDiscoveryService, DiscoveryServiceProxy>(serviceManagement, authCredentials))
            {
                // Obtain organization information from the Discovery service. 
                if (discoveryProxy != null)
                {
                    // Obtain information about the organizations that the system user belongs to.
                    OrganizationDetailCollection orgs = DiscoverOrganizations(discoveryProxy);
                    // Obtains the Web address (Uri) of the target organization.
                    organizationUri = FindOrganization(_organizationUniqueName,
                        orgs.ToArray()).Endpoints[EndpointType.OrganizationService];
                }
            }

            if (!String.IsNullOrWhiteSpace(organizationUri))
            {
                IServiceManagement<IOrganizationService> orgServiceManagement =ServiceConfigurationFactory.CreateManagement<IOrganizationService>(new Uri(organizationUri));

                // Set the credentials.
                AuthenticationCredentials credentials = GetCredentials(endpointType);

                // Get the organization service proxy.
                OrganizationServiceProxy organizationProxy = GetProxy<IOrganizationService, OrganizationServiceProxy>(orgServiceManagement, credentials);
                // This statement is required to enable early-bound type support.
                organizationProxy.EnableProxyTypes();
                return organizationProxy;
            }
            throw new Exception("Could not get service");
        }


        private AuthenticationCredentials GetCredentials(AuthenticationProviderType endpointType)
        {

            AuthenticationCredentials authCredentials = new AuthenticationCredentials();
            switch (endpointType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    authCredentials.ClientCredentials.Windows.ClientCredential =
                        new System.Net.NetworkCredential(_userName,
                            _password,
                            _domain);
                    break;
                case AuthenticationProviderType.LiveId:
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    authCredentials.SupportingCredentials = new AuthenticationCredentials();
                    authCredentials.SupportingCredentials.ClientCredentials =
                        Microsoft.Crm.Services.Utility.DeviceIdManager.LoadOrRegisterDevice();
                    break;
                default: // For Federated and OnlineFederated environments.                    
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    // For OnlineFederated single-sign on, you could just use current UserPrincipalName instead of passing user name and password.
                    // authCredentials.UserPrincipalName = UserPrincipal.Current.UserPrincipalName;  //Windows/Kerberos
                    break;
            }

            return authCredentials;
        }

        public OrganizationDetailCollection DiscoverOrganizations(
            IDiscoveryService service)
        {
            if (service == null) throw new ArgumentNullException("service");
            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse =
                (RetrieveOrganizationsResponse)service.Execute(orgRequest);

            return orgResponse.Details;
        }

        /// <seealso cref="DiscoveryOrganizations"/>
        public OrganizationDetail FindOrganization(string orgFriendlyName,
            OrganizationDetail[] orgDetails)
        {
            if (String.IsNullOrWhiteSpace(orgFriendlyName))
                throw new ArgumentNullException("orgFriendlyName");
            if (orgDetails == null)
                throw new ArgumentNullException("orgDetails");
            OrganizationDetail orgDetail = null;

            foreach (OrganizationDetail detail in orgDetails)
            {
                if (String.Compare(detail.FriendlyName, orgFriendlyName,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    orgDetail = detail;
                    break;
                }
            }
            return orgDetail;
        }

      
        private TProxy GetProxy<TService, TProxy>(
            IServiceManagement<TService> serviceManagement,
            AuthenticationCredentials authCredentials)
            where TService : class
            where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);

            if (serviceManagement.AuthenticationType !=
                AuthenticationProviderType.ActiveDirectory)
            {
                AuthenticationCredentials tokenCredentials =
                    serviceManagement.Authenticate(authCredentials);
                return (TProxy)classType
                    .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) })
                    .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
            }
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) })
                .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });
        }
    }
}
