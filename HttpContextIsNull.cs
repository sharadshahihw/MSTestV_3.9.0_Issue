using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeirdIssueInMSTest3._9._0
{
    /*
     * Issue: HttpContext is null in [TestMethod] after setting it up in [Test Initialize]
     *
     *
     */
    [TestClass]
    public class HttpContextIsNull
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = FakeHttpContext();
        }

        [TestMethod]
        public void HttpContext_IsNotNull()
        {
            Assert.IsNotNull(HttpContext.Current);
        }

        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://localhost:7000/", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(), 10, true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null, CallingConventions.Standard,
                    new[] { typeof(HttpSessionStateContainer) },
                    null)
                .Invoke(new object[] { sessionContainer });

            return httpContext;
        }
    }
}
