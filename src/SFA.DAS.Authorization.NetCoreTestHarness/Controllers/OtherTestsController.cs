using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using StructureMap;

namespace SFA.DAS.Authorization.NetCoreTestHarness.Controllers
{
    public class OtherTestResult
    {
        public string Message { get; set; }
        public bool Successful { get; set; }
    }

    public class OtherTestsController : Controller
    {
        private readonly IContainer _container;

        public OtherTestsController(IContainer container)
        {
            _container = container;
        }

        public ActionResult Index()
        {
            return View(new OtherTestResult {Message = ""});
        }
        
        [Route("GetCommitmentsAuthorizationHandler")]
        public ActionResult GetCommitmentsAuthorizationHandler()
        {
            return ResolveType<CommitmentPermissions.AuthorizationHandler>();
        }

        [Route("GetIAuthorizationService")]
        public ActionResult GetIAuthorizationService()
        {
            return ResolveType<IAuthorizationService>();
        }
        
        private ActionResult ResolveType<T>()
        {
            try
            {
                var instance = _container.GetInstance<T>();
                return View("Index",new OtherTestResult { Message=$"Created {instance.GetType().Name} for {typeof(T).Name}"});
            }
            catch (Exception ex)
            {
                return View("Index", new OtherTestResult { Message=$"Got error {ex.GetType().Name} resolving {typeof(T).Name}<BR>{GetExceptionMessage(ex)}"});
            }
        }

        private string GetExceptionMessage(Exception ex)
        {
            var sb = new StringBuilder();
            AddToException(sb, ex);
            return sb.ToString();
        }

        private void AddToException(StringBuilder sb, Exception ex)
        {
            if(ex == null)
            {
                return;
            }

            sb.Append(ex.GetType());
            sb.Append("<P>");
            sb.Append(ex.Message.Replace(Environment.NewLine, "<P/>"));
            sb.Append("<HR/>");
            AddToException(sb, ex.InnerException);
        }
    }
}