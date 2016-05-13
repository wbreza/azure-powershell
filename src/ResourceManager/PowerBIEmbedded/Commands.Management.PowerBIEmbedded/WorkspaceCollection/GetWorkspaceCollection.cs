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
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = WorkspaceCollectionNameParameterSet,
            HelpMessage = "Workspace Collection Name.")]
        [ValidateNotNullOrEmpty]
        public string WorkspaceCollectionName { get; set; }

        public override void ExecuteCmdlet()
        {
            var workspaceCollections = new List<Azure.Management.PowerBIEmbedded.Models.WorkspaceCollection>();

            if (ParameterSetName == ResourceGroupParameterSet)
            {
                // Workspace collections within a subscription
                if (string.IsNullOrEmpty(this.ResourceGroupName))
                {
                    var collections = this.PowerBIClient.GetWorkspacesCollectionsInSubscription(this.SubscriptionId, ArmApiVersion);
                    this.WriteWorkspaceCollectionList(collections.Value);
                }

                // Workspace collections within a resource group
                else
                {
                    var collections = this.PowerBIClient.GetWorkspacesCollectionsInResourceGroup(this.SubscriptionId, this.ResourceGroupName, ArmApiVersion);
                    this.WriteWorkspaceCollectionList(collections.Value);
                }
            }

            // Get single workspace by resource group and name
            else
            {
                var collection = this.PowerBIClient.GetWorkspaceCollection(this.SubscriptionId, this.ResourceGroupName, this.WorkspaceCollectionName, ArmApiVersion);
                this.WriteWorkspaceCollection(collection);
            }
        }
    }
}
