var crm = Require<DynamicsCrm>();
var proxy = crm.GetOrganizationService("http://crm/XrmServices/2011/Discovery.svc", "organization unique name");

int stateCodeOpen = 0;
int statusCodeActive = 3;
string taskId = "a4c2dac7-e2fc-e111-8f9c-0050568e02e9";

SetStateRequest state = new SetStateRequest();
state.State = new OptionSetValue(stateCodeOpen);
state.Status = new OptionSetValue(statusCodeActive);
state.EntityMoniker = new EntityReference("task", new Guid(taskId));
proxy.Execute(state);
Console.WriteLine("New state set");