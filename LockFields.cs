using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Solutions
{
    public class LockFields : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    EntityReference order = context.InputParameters["Target"] as EntityReference;
                    Entity Approvels= new Entity();
                    Entity UpdatedRecord= new Entity();

                    if (order.KeyAttributes.Keys.Contains("")) 
                    {
                        QueryExpression query = new QueryExpression("new_requestsparepartapproval");
                        query.Criteria = new FilterExpression() { Conditions = { new ConditionExpression("new_order", ConditionOperator.Equal, order.Id) } };
                        query.ColumnSet = new ColumnSet("");
                        //new_requestsparepartapprovals?$filter=_new_order_value eq " + salesOrderId
                        Approvels = service.Retrieve(order.LogicalName, order.Id, new ColumnSet("", ""));
                        UpdatedRecord = (Entity)context.OutputParameters["BusinessEntity"];
                        if (Approvels.Attributes.Contains(""))
                            UpdatedRecord["crc4a_hasapprovel"] = true;
                        else
                            UpdatedRecord["crc4a_hasapprovel"] = false;
                    }
                    else
                        UpdatedRecord["crc4a_hasapprovel"] = false;

                    // service.Update(Approvels);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

        }
    }
}
