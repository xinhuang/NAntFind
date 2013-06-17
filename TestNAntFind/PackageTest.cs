using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAntFind;

namespace TestNAntFind
{
    [TestClass]
    public class PackageTest
    {
        [TestMethod]
        public void given_package_xml_when_query_name_should_return_correct_name()
        {
            const string xml = "<package name=\"SQLServer\"></package>";

            var sut = new Package(xml);

            Assert.AreEqual("SQLServer", sut.Name);
        }

        [TestMethod]
        public void given_package_xml_with_0_version_when_query_versions_should_return_empty_dictionary()
        {
            const string xml = "<package name=\"SQLServer\"></package>";

            var sut = new Package(xml);

            Assert.AreEqual(0, sut.Versions.Count);
        }

        [TestMethod]
        public void given_package_xml_with_1_version_when_query_the_version_should_return_not_null()
        {
            const string xml = "<package name=\"SQLServer\"><version value=\"11\"></version></package>";

            var sut = new Package(xml);


            Assert.IsNotNull(sut.Versions["11"]);
            Assert.IsNotNull(sut.Versions["11"]);
        }
    }
}
