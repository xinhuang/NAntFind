using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAntFind;

namespace TestNAntFind
{
    [TestClass]
    public class VersionTest
    {
        [TestMethod]
        public void given_test_xml_with_hints_when_query_hints_should_return_currect_hint()
        {
            var doc = new XmlDocument();
            doc.LoadXml(
                "<version value=\"11.0\"><hints><hint value=\"C:\\Program Files\\Microsoft Visual Studio 11.0\" /><hint value=\"C:\\Program Files (x86)\\Microsoft Visual Studio 11.0\" /></hints><names><name value=\"Common7\\IDE\\devenv.exe\" /><name value=\"Common7\\IDE\\tf.exe\" /></names></version>");

            var sut = new Version(doc.FirstChild);

            Assert.AreEqual(2, sut.Hints.Count);
        }

        [TestMethod]
        public void given_test_xml_with_hints_when_query_names_should_return_currect_hint()
        {
            var doc = new XmlDocument();
            doc.LoadXml(
                "<version value=\"11.0\"><hints><hint value=\"C:\\Program Files\\Microsoft Visual Studio 11.0\" /><hint value=\"C:\\Program Files (x86)\\Microsoft Visual Studio 11.0\" /></hints><names><name value=\"Common7\\IDE\\devenv.exe\" /><name value=\"Common7\\IDE\\tf.exe\" /></names></version>");

            var sut = new Version(doc.FirstChild);

            Assert.AreEqual(2, sut.Names.Count);
            CollectionAssert.AreEqual(
                new List<string>
                {
                    "Common7\\IDE\\devenv.exe",
                    "Common7\\IDE\\tf.exe"
                },
                sut.Names);
        }
    }
}