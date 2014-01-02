using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NgVal.Tests.Models;

namespace NgVal.Tests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void MustReturnRequiredClientValidationMessage()
        {
            HtmlHelper<TestEntity> html = null;
            var result = html.NgValFor(s => s.RequiredProperty);
            var expectedValue = "ngval='{\"required\":\"The RequiredProperty field is required.\"}' required";
            Assert.AreEqual(expectedValue, result.ToString());
        }

        [TestMethod]
        public void MustReturnStringLength10ClientValidationMessage()
        {
            HtmlHelper<TestEntity> html = null;
            var result = html.NgValFor(s => s.Length10Property);
            var expectedValue = "ngval='{\"length\":\"The field Length10Property must be a string with a maximum length of 10.\"}'";
            Assert.AreEqual(expectedValue, result.ToString());
        }
    }
}