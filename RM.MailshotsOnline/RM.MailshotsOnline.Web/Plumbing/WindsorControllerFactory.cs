﻿using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Web.Mvc;

namespace RM.MailshotsOnline.Web.Plumbing
{
    /// <summary>
    /// Standard controller factory as suggested by the Castle Windsor documentation
    /// </summary>
    /// <see cref="https://github.com/castleproject/Windsor/blob/master/docs/mvc-tutorial-part-2-plugging-windsor-in.md"/>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        protected readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }
            return (IController)kernel.Resolve(controllerType);
        }
    }
}