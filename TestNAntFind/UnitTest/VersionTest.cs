using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAntFind;

namespace TestNAntFind
{
    [TestClass]
    public class VersionTest
    {
        const string VisualStudioFindModuleXml = "<version value=\"11.0\"><hints><hint value=\"C:\\Program Files\\Microsoft Visual Studio 11.0\" /><hint value=\"C:\\Program Files (x86)\\Microsoft Visual Studio 11.0\" /></hints><names><name value=\"Common7\\IDE\\devenv.exe\" /><name value=\"Common7\\IDE\\tf.exe\" /></names></version>";
        Version sut;

        [TestInitialize]
        public void SetUp()
        {
            var doc = new XmlDocument();
            doc.LoadXml(VisualStudioFindModuleXml);
            sut = new Version(doc.FirstChild);
        }

        [TestMethod]
        public void given_test_xml_when_query_hints_should_return_currect_value()
        {
            var expected = new List<string> { "C:\\Program Files\\Microsoft Visual Studio 11.0", "C:\\Program Files (x86)\\Microsoft Visual Studio 11.0" };

            AssertEnumeration(expected, sut.Hints);
        }

        [TestMethod]
        public void given_test_xml_when_query_names_should_return_currect_value()
        {
            CollectionAssert.AreEqual(
                new List<string>
                {
                    "Common7\\IDE\\devenv.exe",
                    "Common7\\IDE\\tf.exe"
                },
                sut.Names);
        }

        private void AssertEnumeration(List<string> expected, IEnumerable<string> hints)
        {
            int i = 0;
            foreach (var value in hints)
            {
                Assert.AreEqual(expected[i], value);
                ++i;
            }
            Assert.AreEqual(expected.Count, i);
        }
    }
}