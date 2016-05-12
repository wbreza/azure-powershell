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

using System.Management.Automation;
using Microsoft.Azure.Commands.Management.PowerBIEmbedded.Models;
using Microsoft.Azure.Management.PowerBIEmbedded;
using Microsoft.Azure.Management.PowerBIEmbedded.Models;

namespace Microsoft.Azure.Commands.Management.PowerBIEmbedded.WorkspaceCollection
{
    [Cmdlet(VerbsCommon.New, Nouns.WorkspaceCollection), OutputType(typeof(PSWorkspaceCollection))]
    public class NewWorkspaceCollection : WorkspaceCollectionBaseCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Resource Group Name.")]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Workspace Collection Name.")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(
        Position = 2,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = LocationParameterSet,
            HelpMessage = "Location.")]
        [ValidateNotNullOrEmpty]
        public string Location { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            var createWorkspaceRequest = new CreateWorkspaceCollectionRequest
            {
                Location = this.Location,
                Sku = new AzureSku { Name = "S1", Tier = "Standard" }
            };

            var workspce = this.PowerBIClient.CreateWorkspaceCollection(this.SubscriptionId, this.ResourceGroupName, this.Name, ArmApiVersion, createWorkspaceRequest);
            this.WriteWorkspaceCollection(workspce);
        }
    }
}
