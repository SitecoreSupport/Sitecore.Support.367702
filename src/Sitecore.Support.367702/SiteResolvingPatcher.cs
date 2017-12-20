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
      (typeof(LinkProvider).GetField("defaultUrlOptions", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(provider) as UrlOptions).SiteResolving = Settings.Rendering.SiteResolving;
    }
  }
}
