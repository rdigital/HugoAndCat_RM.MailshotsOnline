using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace RM.MailshotsOnline.Web.Plumbing
{
    // Needed to allow injectino for API controllers
    // See: http://stackoverflow.com/questions/21835555/umbraco-mvc-with-castle-windsor
    // See also: https://gist.github.com/florisrobbemont/5821863
    // See also: http://www.wearesicc.com/getting-started-with-umbraco-7-and-structuremap-v3/
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        private readonly DefaultHttpControllerActivator _defaultHttpControllerActivator;
        private readonly IKernel _kernel;

        public WindsorCompositionRoot(IKernel kernel)
        {
            this._kernel = kernel;
            this._defaultHttpControllerActivator = new DefaultHttpControllerActivator();
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController controller;
            
            if (Helpers.ControllersHelper.IsUmbracoController(controllerType))
            {
                controller = this._defaultHttpControllerActivator.Create(request, controllerDescriptor, controllerType);
            }
            else
            {
                controller = (IHttpController)_kernel.Resolve(controllerType);
                request.RegisterForDispose(new Release(() => _kernel.ReleaseComponent(controller)));
            }

            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action release;

            public Release(Action release)
            {
                this.release = release;
            }

            public void Dispose()
            {
                this.release();
            }
        }
    }
}