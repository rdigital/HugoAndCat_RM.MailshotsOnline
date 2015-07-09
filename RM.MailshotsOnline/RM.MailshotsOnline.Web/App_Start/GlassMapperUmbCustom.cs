using System.Collections.Generic;
using Castle.Windsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace RM.MailshotsOnline.Web.App_Start
{
    public static class GlassMapperUmbCustom
    {
		public static void CastleConfig(IWindsorContainer container){
			var config = new Config();
            config.UseWindsorContructor = true;
            var umbracoInstaller = new UmbracoInstaller(config);
            container.Install(umbracoInstaller);
		}

		public static IConfigurationLoader[] GlassLoaders(){
			var attributes = new UmbracoAttributeConfigurationLoader("RM.MailshotsOnline.Web");
			
			return new IConfigurationLoader[]{attributes};
		}
    }
}
