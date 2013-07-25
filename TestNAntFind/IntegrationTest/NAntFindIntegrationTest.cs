﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestNAntFind.IntegrationTest
{
    [TestClass]
    public class NAntFindIntegrationTest
    {
        [TestMethod]
        public void should_find_visual_studio_110_from_registry()
        {
            NAnt.Assert("TestFindFromRegistry.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_hint()
        {
            NAnt.Assert("TestFindFromHint.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_env()
        {
            NAnt.Assert("TestFindFromEnv.build");
        }

        [TestMethod]
        public void should_find_devenv_in_visual_studio_110()
        {
            NAnt.Assert("TestFindExistingFile.build");
        }

        [TestMethod]
        public void given_no_version_specified_should_return_default_version()
        {
            NAnt.Assert("GivenNoVersionShouldReturnDefault.build");
        }

        [TestMethod]
        public void given_no_version_specified_and_no_default_should_fail()
        {
            NAnt.AssertFail("GivenNoVersionSpecifiedAndDefaultMissingShouldFail.build");
        }

        [TestMethod]
        public void given_no_version_specified_in_find_and_module_should_return_only_one_as_implicit_default()
        {
            NAnt.Assert("TestFindImplicitDefault.build");
        }
    }
}
