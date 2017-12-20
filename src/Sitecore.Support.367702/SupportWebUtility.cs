
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Text;
using Sitecore.Web;

namespace Sitecore.Support
{
  public static class SupportWebUtility
  {
    public static UrlString ChangeLanguageUrl(UrlString url, ItemUri itemUri, string languageName)
    {
      UrlString urlString = new UrlString(url.GetUrl());
      if (itemUri == (ItemUri)null)
        return (UrlString)null;
      SiteContext site = SiteContext.GetSite(WebEditUtil.SiteName);
      if (site == null)
        return (UrlString)null;
      Item itemNotNull = Client.GetItemNotNull(itemUri);
      using (new SiteContextSwitcher(site))
      {
        using (new LanguageSwitcher(itemNotNull.Language))
        {
          urlString = SupportWebUtility.BuildChangeLanguageNewUrl(languageName, url, itemNotNull);
          if (LinkManager.LanguageEmbedding == LanguageEmbedding.Never)
            urlString["sc_lang"] = languageName;
          else
            urlString.Remove("sc_lang");
        }
      }
      return urlString;
    }

    private static UrlString BuildChangeLanguageNewUrl(string languageName, UrlString url, Item item)
    {
      Assert.ArgumentNotNull((object)languageName, "languageName");
      Assert.ArgumentNotNull((object)url, "url");
      Assert.ArgumentNotNull((object)item, "item");
      Language result;
      Assert.IsTrue(Language.TryParse(languageName, out result), string.Format("Cannot parse the language ({0}).", (object)languageName));
      UrlOptions defaultOptions = UrlOptions.DefaultOptions;
      defaultOptions.Language = result;
      defaultOptions.AlwaysIncludeServerUrl = true;
      Item obj = item.Database.GetItem(item.ID, result);
      string message = string.Format("Item not found ({0}, {1}).", (object)item.ID, (object)result);
      Assert.IsNotNull((object)obj, message);
      UrlOptions options = defaultOptions;
      UrlString urlString = new UrlString(LinkManager.GetItemUrl(obj, options));
      foreach (string key in url.Parameters.Keys)
        urlString.Parameters[key] = url.Parameters[key];
      return urlString;
    }
  }
}
