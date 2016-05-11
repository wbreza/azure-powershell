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
using Microsoft.WindowsAzure.Commands.Utilities.Common;

namespace Microsoft.Azure.Commands.Management.PowerBIEmbedded.WorkspaceCollection
{
    public class WorkspaceCollectionBaseCmdlet : AzureRMCmdlet
    {
        protected const string WorkspaceCollectionNounStr = "AzureRmPowerBIWorkspaceCollection";
        protected const string ArmApiVersion = "2016-01-29";

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
            WriteObject(PSWorkspaceCollection.Create(workspaceCollection));
        }

        protected void WriteWorkspaceCollectionList(IEnumerable<Azure.Management.PowerBIEmbedded.Models.WorkspaceCollection> workspaceCollections)
        {
            List<PSWorkspaceCollection> output = new List<PSWorkspaceCollection>();
            workspaceCollections.ForEach(workspaceCollection => output.Add(PSWorkspaceCollection.Create(workspaceCollection)));
            WriteObject(output, true);
        }
    }
}
