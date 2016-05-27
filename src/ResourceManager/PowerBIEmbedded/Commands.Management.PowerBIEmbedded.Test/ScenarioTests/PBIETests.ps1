# ----------------------------------------------------------------------------------
#
# Copyright Microsoft Corporation
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ----------------------------------------------------------------------------------


#################################
## WorkspaceCollection Cmdlets ##
#################################

<#
.SYNOPSIS
Test Get-WorkspaceCollection with (List)
#>
function Test-GetWorkspaceCollection_ListAll
{
    Execute-PbiTest -RegionCount 2 {
        $wcList = Get-AzureRmPowerBIWorkspaceCollection
        
        Assert-True ($wcList.Count -ge 2)
        Assert-AreEqual 1 ($wcList |? Name -eq $WorkspaceCollectionName[0].Name).Count
        Assert-AreEqual 1 ($wcList |? Name -eq $WorkspaceCollectionName[1].Name).Count
    }
}

<#
.SYNOPSIS
Test Get-WorkspaceCollection with (List by RG)
#>
function Test-GetWorkspaceCollection_ListByRegionName
{
    Execute-PbiTest -RegionCount 2 {
        $wcList = Get-AzureRmPowerBIWorkspaceCollection -ResourceGroupName $ResourceGroupName[0]
        
        Assert-True 1 $wcList.Count
        Assert-AreEqual $WorkspaceCollectionName[0] $wcList[0].Name
    }
}

<#
.SYNOPSIS
Test Get-WorkspaceCollection (by RegionName and WorkspaceCollectionName)
#>
function Test-GetWorkspaceCollection_ByName
{
    Execute-PbiTest {
        $wc = Get-AzureRmPowerBIWorkspaceCollection -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName

        Assert-AreEqual $WorkspaceCollectionName $wc.Name
    }
}


#######################
## Workspace Cmdlets ##
#######################

<#
.SYNOPSIS
Test Get-Workspace (Empty)
#>
function Test-GetWorkspace_EmptyCollection
{
    Execute-PbiTest {
        $w = Get-Workspace -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName

        Assert-AreEqual 0 $w.Count
    }
}


###########################################
## WorkspaceCollectionAccessKeys Cmdlets ##
###########################################

<#
.SYNOPSIS
Test Reset-WorkspaceAccessKeys (Key1)
#>
function Test-ResetWorkspaceCollectionAccessKeys_Key1
{
    Execute-PbiTest {
        $k1 = Get-AzureRmPowerBIWorkspaceCollectionAccessKeys -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName
        $kr = Reset-AzureRmPowerBIWorkspaceCollectionAccessKeys -Key1 -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName
        $k2 = Get-AzureRmPowerBIWorkspaceCollectionAccessKeys -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName

        Assert-AreNotEqual $k1.Key1 $kr.Key1
        Assert-AreEqual $k1.Key2 $kr.Key2

        Assert-AreEqual $kr.Key1 $k2.Key1
        Assert-AreEqual $kr.Key2 $k2.Key2
    }
}

<#
.SYNOPSIS
Test Reset-WorkspaceAccessKeys (Key2)
#>
function Test-ResetWorkspaceCollectionAccessKeys_Key2
{
    Execute-PbiTest {
        $k1 = Get-AzureRmPowerBIWorkspaceCollectionAccessKeys -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName
        $kr = Reset-AzureRmPowerBIWorkspaceCollectionAccessKeys -Key2 -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName
        $k2 = Get-AzureRmPowerBIWorkspaceCollectionAccessKeys -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName

        Assert-AreEqual $k1.Key1 $kr.Key1
        Assert-AreNotEqual $k1.Key2 $kr.Key2

        Assert-AreEqual $kr.Key1 $k2.Key1
        Assert-AreEqual $kr.Key2 $k2.Key2
    }
}

<#
.SYNOPSIS
Test Get-WorkspaceAccessKeys
#>
function Test-GetWorkspaceCollectionAccessKeys
{
    Execute-PbiTest {
        $keys = Get-AzureRmPowerBIWorkspaceCollectionAccessKeys -ResourceGroupName $ResourceGroupName -WorkspaceCollectionName $WorkspaceCollectionName

        Assert-AreNotEqual $null $keys
    }
}


#################
## Common Code ##
#################

function Execute-PbiTest
{
    param(
      [ScriptBlock] $Test,
      [int] $RegionCount = 1,
      [ScriptBlock] $Initialize = {},
      [ScriptBlock] $Cleanup = {}
    )

    if ($RegionCount -le 0) { throw "Invalid region count: " + $RegionCount }

    $ResourceGroupName = 1..$RegionCount |% { Get-PowerBIEmbeddedTestResourceName }
    $WorkspaceCollectionName = $ResourceGroupName |% { 'wcn' + $_ }

    $Location = 'West US' # Get-ComputeVMLocation

    Invoke-Command -NoNewScope $Initialize

    try
    {
        $ResourceGroupName |% { New-AzureRmResourceGroup -Name $_ -Location $Location }

        $WorkspaceCollection = 1..$RegionCount |% { $i = $_ - 1
            New-AzureRmPowerBIWorkspaceCollection `
                -ResourceGroupName $ResourceGroupName[$i] `
                -WorkspaceCollectionName $WorkspaceCollectionName[$i] `
                -Location $Location
        }

        Invoke-Command -NoNewScope $Test

        $WorkspaceCollection | Remove-AzureRmPowerBIWorkspaceCollection
    }
    finally
    {
        Invoke-Command -NoNewScope $Cleanup

        $ResourceGroupName |% { try { Clean-ResourceGroup $_ } catch { } }
    }
}