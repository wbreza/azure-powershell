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
using Microsoft.Azure.Commands.Common.Authentication.Models;
using Microsoft.Azure.Commands.Management.PowerBIEmbedded.Models;
using Microsoft.Azure.Commands.ResourceManager.Common;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Management.PowerBIEmbedded;
using Microsoft.Azure.Management.PowerBIEmbedded.Models;
using Microsoft.WindowsAzure.Commands.Utilities.Common;

namespace Microsoft.Azure.Commands.Management.PowerBIEmbedded.WorkspaceCollection
{
    public class WorkspaceCollectionBaseCmdlet : AzureRMCmdlet
    {
        protected const string ArmApiVersion = "2016-01-29";

        // Command Names
        protected const string ResourceGroupParameterSet = "ResourceGroupParameterSet";
        protected const string WorkspaceCollectionNameParameterSet = "WorkspaceCollectionNameParameterSet";
        protected const string LocationParameterSet = "LocationParameterSet";
        protected const string WorkspaceCollectionAccessKeyNameParameterSet = "WorkspaceCollectionAccessKeyNameParameterSet";

        private IPowerBIEmbeddedManagementClient powerBIClient;

        protected IPowerBIEmbeddedManagementClient PowerBIClient
        {
            get
            {
                if (this.powerBIClient == null)
                {
                    this.powerBIClient = AzureSession.ClientFactory.CreateArmClient<PowerBIEmbeddedManagementClient>(DefaultProfile.Context, AzureEnvironment.Endpoint.ResourceManager);
                    return this.powerBIClient;
                }
                else
                {
                    return this.powerBIClient;
                }
            }
        }

        public string SubscriptionId => DefaultProfile.Context.Subscription.Id.ToString();

        protected void WriteWorkspaceCollection(Azure.Management.PowerBIEmbedded.Models.WorkspaceCollection workspaceCollection)
        {
            this.WriteObject(PSWorkspaceCollection.Create(workspaceCollection));
        }

        protected void WriteWorkspaceCollectionAccessKeys(WorkspaceCollectionAccessKeys accessKeys)
        {
            this.WriteObject(PSWorkspaceCollectionAccessKey.CreateList(accessKeys), true);
        }

        protected void WriteWorkspace(Workspace workspace)
        {
            this.WriteObject(PSWorkspace.Create(workspace));
        }

        protected void WriteWorkspaceList(IEnumerable<Workspace> workspaces)
        {
            List<PSWorkspace> output = new List<PSWorkspace>();
            workspaces.ForEach(workspace => output.Add(PSWorkspace.Create(workspace)));
            this.WriteObject(output, true);
        }

        protected void WriteWorkspaceCollectionList(IEnumerable<Azure.Management.PowerBIEmbedded.Models.WorkspaceCollection> workspaceCollections)
        {
            List<PSWorkspaceCollection> output = new List<PSWorkspaceCollection>();
            workspaceCollections.ForEach(workspaceCollection => output.Add(PSWorkspaceCollection.Create(workspaceCollection)));
            this.WriteObject(output, true);
        }
    }
}
