// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Azure.Commands.Management.PowerBIEmbedded.Models;
using Microsoft.Azure.Management.PowerBIEmbedded;

namespace Microsoft.Azure.Commands.Management.PowerBIEmbedded.WorkspaceCollection
{
    [Cmdlet(VerbsCommon.Get, Nouns.WorkspaceCollection), OutputType(typeof(PSWorkspaceCollection))]
    public class GetWorkspaceCollection : WorkspaceCollectionBaseCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = false,
            ParameterSetName = ResourceGroupParameterSet,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Resource Group Name.")]
        [Parameter(
            Position = 0,
            Mandatory = true,
            ParameterSetName = WorkspaceCollectionNameParameterSet,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Resource Group Name.")]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = WorkspaceCollectionNameParameterSet,
            HelpMessage = "Workspace Collection Name.")]
        public string Name { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            var workspaceCollections = new List<Azure.Management.PowerBIEmbedded.Models.WorkspaceCollection>();

            // Workspace collections within a subscription
            if (string.IsNullOrWhiteSpace(this.Name) && string.IsNullOrWhiteSpace(this.ResourceGroupName))
            {
                var subscriptionCollections = this.PowerBIClient.GetWorkspacesCollectionsInSubscription(this.SubscriptionId, ArmApiVersion);
                workspaceCollections.AddRange(subscriptionCollections.Value);
            }

            // Workspace collections within a resource group
            else if (string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.ResourceGroupName))
            {
                var resourceGroupCollections = this.PowerBIClient.GetWorkspacesCollectionsInResourceGroup(this.SubscriptionId, this.ResourceGroupName, ArmApiVersion);
                workspaceCollections.AddRange(resourceGroupCollections.Value);
            }

            // Get single workspace by resource group and name
            else if (!string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.ResourceGroupName))
            {
                var workspace = this.PowerBIClient.GetWorkspaceCollection(this.SubscriptionId, this.ResourceGroupName, this.Name, ArmApiVersion);
                workspaceCollections.Add(workspace);
            }

            this.WriteWorkspaceCollectionList(workspaceCollections);
        }
    }
}
