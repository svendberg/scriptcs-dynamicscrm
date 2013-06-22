// Script from Microsoft Dynamics CRM 2011 SDK
// The only change is adding the two first lines and added 
// deleteAuditDataCreatedBefore parameter

var crm = Require<DynamicsCrm>();
var _service = crm.GetOrganizationService("http://crm/XrmServices/2011/Discovery.svc", "Organization");
var deleteAuditDataCreatedBefore = new DateTime(2002,1,1);

// Get the list of audit partitions.
RetrieveAuditPartitionListResponse partitionRequest =
    (RetrieveAuditPartitionListResponse)_service.Execute(new RetrieveAuditPartitionListRequest());
AuditPartitionDetailCollection partitions = partitionRequest.AuditPartitionDetailCollection;

// Create a delete request with an end date earlier than possible.
DeleteAuditDataRequest deleteRequest = new DeleteAuditDataRequest();
deleteRequest.EndDate = deleteAuditDataCreatedBefore;

// Check if partitions are not supported as is the case with SQL Server Standard edition.
if (partitions.IsLogicalCollection)
{
    // Delete all audit records created up until now.
    deleteRequest.EndDate = DateTime.Now;
}

// Otherwise, delete all partitions that are older than the current partition.
// Hint: The partitions in the collection are returned in sorted order where the 
// partition with the oldest end date is at index 0.
else
{
    for (int n = partitions.Count - 1; n >= 0; --n)
    {
        if (partitions[n].EndDate <= DateTime.Now && partitions[n].EndDate > deleteRequest.EndDate)
        {
            deleteRequest.EndDate = (DateTime)partitions[n].EndDate;
            break;
        }
    }
}

// Delete the audit records.
if (deleteRequest.EndDate != deleteAuditDataCreatedBefore)
{
    _service.Execute(deleteRequest);
    Console.WriteLine("Audit records have been deleted.");
}
else
    Console.WriteLine("There were no audit records that could be deleted.");
