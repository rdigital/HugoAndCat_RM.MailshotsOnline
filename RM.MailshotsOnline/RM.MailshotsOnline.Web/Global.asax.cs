using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Glass.Mapper.Umb;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using Microsoft.WindowsAzure;
using RM.MailshotsOnline.Web.App_Start;
using RM.MailshotsOnline.Web.Plumbing;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Umbraco.Core.Services;

namespace RM.MailshotsOnline.Web
{
    public class Global : Umbraco.Web.UmbracoApplication
    {
        // NOTE:
        // Umbraco app start / end events are set by creating a class that inherits from ApplicationEventHandler
        // I have set one of these up in App_Start/RegisterIoCEvents.cs
    }
}