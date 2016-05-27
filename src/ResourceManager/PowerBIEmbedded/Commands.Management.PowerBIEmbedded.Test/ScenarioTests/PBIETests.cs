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

using Microsoft.Azure.ServiceManagemenet.Common.Models;
using Microsoft.WindowsAzure.Commands.ScenarioTest;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Azure.Commands.Management.PBIE.Test.ScenarioTests
{
    public class PBIETests : RMTestBase
    {
        public PBIETests(ITestOutputHelper output)
        {
            XunitTracingInterceptor.AddToContext(new XunitTracingInterceptor(output));
        }
       
        [Theory]
        [InlineData("ListAll")]
        [InlineData("ListByRegionName")]
        [InlineData("ByName")]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestGetWorkspaceCollection(string psTestName)
        {
            TestController.NewInstance.RunPsTest("Test-GetWorkspaceCollection_" + psTestName);
        }

        [Theory]
        [InlineData("EmptyCollection")]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestGetWorkspace(string psTestName)
        {
            TestController.NewInstance.RunPsTest("Test-GetWorkspace_" + psTestName);
        }

        [Theory]
        [InlineData("Key1")]
        [InlineData("Key2")]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestResetWorkspaceCollectionAccessKeys(string psTestName)
        {
            TestController.NewInstance.RunPsTest("Test-ResetWorkspaceCollectionAccessKeys_" + psTestName);
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestGetWorkspaceCollectionAccessKeys()
        {
            TestController.NewInstance.RunPsTest("Test-GetWorkspaceCollectionAccessKeys");
        }
    }
}
