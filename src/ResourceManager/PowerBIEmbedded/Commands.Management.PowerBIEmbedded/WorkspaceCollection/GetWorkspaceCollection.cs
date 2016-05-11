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
    [Cmdlet(VerbsCommon.Get, WorkspaceCollectionNounStr), OutputType(typeof(PSWorkspaceCollection))]
    public class GetWorkspaceCollection : WorkspaceCollectionBaseCmdlet
    {
        protected const string ResourceGroupParameterSet = "ResourceGroupParameterSet";
        protected const string WorkspaceCollectionNameParameterSet = "WorkspaceCollectionNameParameterSet";

        [Parameter(
            Position = 0,
            Mandatory = false,
            ParameterSetName = ResourceGroupParameterSet,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Resource Group Name.")]

        [Parameter(
            Position = 1,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = WorkspaceCollectionNameParameterSet,
            HelpMessage = "Workspace Collection Name.")]

        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        public async override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            var workspaceCollections = new List<Azure.Management.PowerBIEmbedded.Models.WorkspaceCollection>();

            // Workspace collections within a subscription
            if (string.IsNullOrWhiteSpace(this.Name) && string.IsNullOrWhiteSpace(this.ResourceGroupName))
            {
                var subscriptionCollections = await this.PowerBIClient.GetWorkspacesCollectionsBySubscriptionAsync(this.SubscriptionId, ArmApiVersion);
                workspaceCollections.AddRange(subscriptionCollections.Value);
            }

            // Workspace collections within a resource group
            if (string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.ResourceGroupName))
            {
                var resourceGroupCollections = await this.PowerBIClient.GetWorkspacesCollectionsByResourceGroupAsync(this.SubscriptionId, this.ResourceGroupName, ArmApiVersion);
                workspaceCollections.AddRange(resourceGroupCollections.Value);
            }

            if (!string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.ResourceGroupName))
            {
                var workspace = await this.PowerBIClient.GetWorkspaceCollectionAsync(this.SubscriptionId, this.ResourceGroupName, this.Name, ArmApiVersion);
                workspaceCollections.Add(workspace);
            }

            this.WriteWorkspaceCollectionList(workspaceCollections);
        }
    }
}
