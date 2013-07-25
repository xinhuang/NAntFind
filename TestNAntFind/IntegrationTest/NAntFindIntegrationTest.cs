using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestNAntFind.IntegrationTest
{
    [TestClass]
    public class NAntFindIntegrationTest
    {
        [TestMethod]
        public void should_find_visual_studio_110_from_registry()
        {
            Assert("TestFindFromRegistry.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_hint()
        {
            Assert("TestFindFromHint.build");
        }

        [TestMethod]
        public void should_find_visual_studio_110_from_env()
        {
            Assert("TestFindFromEnv.build");
        }

        [TestMethod]
        public void should_find_devenv_in_visual_studio_110()
        {
            Assert("TestFindExistingFile.build");
        }

        [TestMethod]
        public void given_no_version_specified_should_return_default_version()
        {
            Assert("GivenNoVersionShouldReturnDefault.build");
        }

        [TestMethod]
        public void given_no_version_specified_and_no_default_should_fail()
        {
            AssertFail("GivenNoVersionSpecifiedAndDefaultMissingShouldFail.build");
        }

        [TestMethod]
        public void given_no_version_specified_in_find_and_module_should_return_only_one_as_implicit_default()
        {
            Assert("TestFindImplicitDefault.build");
        }

        private static void AssertFail(string script)
        {
            NAnt.AssertFail(script);
        }

        private static void Assert(string script)
        {
            NAnt.Assert(script);
        }
    }
}
