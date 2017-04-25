using Nancy;
using Nancy.Diagnostics;

namespace TvUndergroundDownloaderLib.EmbendedWebServer
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"123456" }; }
        }

        //protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        //{
        //    base.ConfigureApplicationContainer(container);
        //    //This should be the assembly your views are embedded in
        //    var assembly = GetType().Assembly;
        //    ResourceViewLocationProvider
        //        .RootNamespaces
        //        //TODO: replace NancyEmbeddedViews.MyViews with your resource prefix
        //        .Add(assembly, "EmbendedWebServer.View");
        //}

        //protected override NancyInternalConfiguration InternalConfiguration
        //{
        //    get
        //    {
        //        return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
        //    }
        //}

        //void OnConfigurationBuilder(NancyInternalConfiguration x)
        //{
        //    x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        //}
    }
}