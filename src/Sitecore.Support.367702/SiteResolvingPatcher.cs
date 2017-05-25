using Sitecore.Configuration;
using Sitecore.Links;
using Sitecore.Pipelines;
using System;
using System.Reflection;


namespace Sitecore.Support
{
    public class SiteResolvingPatcher
    {
        public void Process(PipelineArgs args)
        {
            LinkProvider provider = LinkManager.Provider;
            FieldInfo field = typeof(LinkProvider).GetField("defaultUrlOptions", BindingFlags.Instance | BindingFlags.NonPublic);
            UrlOptions urlOptions = field.GetValue(provider) as UrlOptions;
            urlOptions.SiteResolving = Settings.Rendering.SiteResolving;
        }
    }
}