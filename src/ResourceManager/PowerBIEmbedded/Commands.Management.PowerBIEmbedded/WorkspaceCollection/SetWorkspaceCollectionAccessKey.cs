﻿// ----------------------------------------------------------------------------------
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
    [Cmdlet(VerbsCommon.Set, Nouns.WorkspaceCollectionAccessKeys), OutputType(typeof(PSWorkspaceCollectionAccessKeys))]
    public class SetWorkspaceCollectionAccessKey : WorkspaceCollectionBaseCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ParameterSetName = ResourceGroupParameterSet,
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
        public string Name { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = WorkspaceCollectionAccessKeyNameParameterSet,
            HelpMessage = "Access Key Name.")]
        [ValidateNotNullOrEmpty]
        public string KeyName { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            var accessKeyRequest = new WorkspaceCollectionAccessKey
            {
                KeyName = this.KeyName
            };

            var accessKeys = this.PowerBIClient.RegenerateWorkspaceCollectionAccessKey(this.SubscriptionId, this.ResourceGroupName, this.Name, ArmApiVersion, accessKeyRequest);
            this.WriteWorkspaceCollectionAccessKeys(accessKeys);
        }
    }
}
