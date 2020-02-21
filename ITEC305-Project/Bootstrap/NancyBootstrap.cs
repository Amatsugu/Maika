using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Nancy.Configuration;

namespace Maika.Bootstrap
{
	public class NancyBootstrap : DefaultNancyBootstrapper
	{
		private string PROJECT => "Maika";
		/*private byte[] favicon;

		protected override byte[] FavIcon
		{
			get { return favicon ?? (favicon = LoadFavIcon()); }
		}

		private byte[] LoadFavIcon()
		{
			var ico = Assembly.GetEntryAssembly().GetManifestResourceStream($"{PROJECT}.{PROJECT}Web.res.img.{PROJECT}.ico");
			favicon = new byte[ico.Length];
			ico.Read(favicon, 0, favicon.Length);
			return favicon;
		}*/

		public override void Configure(INancyEnvironment environment)
		{
			base.Configure(environment);
			environment.Tracing(enabled: false, displayErrorTraces: true);

#if DEBUG
			environment.Views(runtimeViewUpdates: true);
#endif
		}

		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			Conventions.ViewLocationConventions.Add((viewName, model, context) =>
			{
				return $@"{PROJECT}Web/{viewName}";
			});
		}

		protected override IRootPathProvider RootPathProvider
		{
			get { return new RootProvider(); }
		}

		protected override void ConfigureConventions(NancyConventions nancyConventions)
		{
			nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("res", $@"{PROJECT}Web/res"));
		}
	}

	public class RootProvider : IRootPathProvider
	{
		public string GetRootPath()
		{
#if DEBUG
			return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
#else
			return Directory.GetCurrentDirectory();
#endif
		}
	}
	
}
