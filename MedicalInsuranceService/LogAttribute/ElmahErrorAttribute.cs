using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace MedicalInsuranceService.LogAttribute
{
    public class ElmahErrorAttribute:ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if(actionExecutedContext.Exception!=null)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
                //记录日志
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(actionExecutedContext.Exception));
               

            }
            base.OnException(actionExecutedContext);
        }
    }
}