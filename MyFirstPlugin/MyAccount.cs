using System;
using Microsoft.Xrm.Sdk;

namespace MyFirstPlugin
{
    /// <summary>
    /// MyAccount plugin for Account table
    /// Implements pre-operation plugin to update the name field
    /// </summary>
    public class MyAccount : IPlugin
    {
        /// <summary>
        /// Executes the plugin logic
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                // Get the tracing service
                ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                tracingService.Trace("MyAccount plugin started");

                // Get the plugin execution context
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // Check if this is the correct message and entity
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity targetEntity = (Entity)context.InputParameters["Target"];

                    // Verify this is an Account entity
                    if (targetEntity.LogicalName != "account")
                    {
                        tracingService.Trace("Plugin triggered for non-account entity. Exiting.");
                        return;
                    }

                    // Get the organization service
                    IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                    // Update the name field
                    UpdateAccountName(targetEntity, service, tracingService);

                    tracingService.Trace("MyAccount plugin completed successfully");
                }
            }
            catch (Exception ex)
            {
                ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                tracingService.Trace("MyAccount plugin error: {0}", ex.ToString());
                throw new InvalidPluginExecutionException("An error occurred in MyAccount plugin.", ex);
            }
        }

        /// <summary>
        /// Updates the account name field
        /// </summary>
        /// <param name="targetEntity">The target account entity</param>
        /// <param name="service">The organization service</param>
        /// <param name="tracingService">The tracing service</param>
        private void UpdateAccountName(Entity targetEntity, IOrganizationService service, ITracingService tracingService)
        {
            tracingService.Trace("Updating account name field");

            // Check if the name attribute exists in the input
            if (targetEntity.Attributes.Contains("name"))
            {
                string currentName = targetEntity["name"].ToString();
                tracingService.Trace("Current name: {0}", currentName);

                // Update the name field with a timestamp
                string updatedName = string.Format("{0} - Updated at {1}", currentName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                
targetEntity["name"] = updatedName;
                tracingService.Trace("Updated name to: {0}", updatedName);
            }
            else
            {
                tracingService.Trace("Name attribute not found in input parameters");
            }
        }
    }
}