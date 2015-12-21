using System;
using System.Web;

namespace ContentHiderModule
{
    public class ContentHiderModule : IHttpModule
    {
        private EventHandler onBeginRequest;

        private Func<HttpContext, bool> isAllowedFunc;

        public ContentHiderModule(Func<HttpContext, bool> isAllowedFunc)
        {
            this.isAllowedFunc = isAllowedFunc;
            onBeginRequest = new EventHandler(this.HandleBeginRequest);
        }

        private void HandleBeginRequest(object sender, EventArgs evargs)
        {
            HttpApplication app = sender as HttpApplication;
            if (!isAllowedFunc.Invoke(app.Context))
            {
                app.Context.Response.StatusCode = 404;
                app.Context.Response.SuppressContent = true;
                app.Context.Response.End();
            }
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += onBeginRequest;
        }

        public void Dispose()
        {

        }
    }
}