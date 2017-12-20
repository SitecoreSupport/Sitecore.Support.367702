using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Globalization;
using Sitecore.Text;
using Sitecore.Web;
using System;
using System.Web;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.ChangeLanguage
{
  public class ChangeLanguageRequest : PipelineProcessorRequest<ValueItemContext>
  {
    private const string LanguageQueryKey = "sc_lang";

    protected UrlString BuidUrl(string initialUrl)
    {
      return new UrlString(WebUtil.GetRequestUri404(initialUrl));
    }

    protected string GetLanguageItem(Database database, string languageItemId)
    {
      Item obj = database.GetItem(HttpUtility.UrlDecode(languageItemId));
      Assert.IsNotNull((object)obj, "Could not find language with id {0}", (object)languageItemId);
      return obj.Name;
    }

    public override PipelineProcessorResponseValue ProcessRequest()
    {
      string[] strArray = this.RequestContext.Value.Split('|');
      PipelineProcessorResponseValue processorResponseValue;
      if (strArray.Length != 2)
      {
        processorResponseValue = new PipelineProcessorResponseValue()
        {
          AbortMessage = "Missing language item id or current url"
        };
      }
      else
      {
        string str = strArray[0];
        UrlString url1 = this.BuidUrl(strArray[1]);
        ItemUri itemUri1 = new ItemUri(ID.Parse(this.RequestContext.ItemId), Language.Parse(this.RequestContext.Language), Sitecore.Data.Version.Parse(this.RequestContext.Version), this.RequestContext.Database);
        ItemUri itemUri2 = itemUri1;
        string languageName1 = str;
        UrlString url2 = SupportWebUtility.ChangeLanguageUrl(url1, itemUri2, languageName1);
        ItemUri itemUri3 = itemUri1;
        string languageName2 = str;
        UrlString urlString = SupportWebUtility.ChangeLanguageUrl(url2, itemUri3, languageName2);
        if (url2 == null)
        {
          processorResponseValue = new PipelineProcessorResponseValue()
          {
            AbortMessage = "Could not build change language URL"
          };
        }
        else
        {
          urlString.ToString();
          string serverUrl = WebUtil.GetServerUrl(new Uri(urlString.ToString()), false);
          processorResponseValue = new PipelineProcessorResponseValue()
          {
            Value = (object)urlString.ToString().Replace(serverUrl, string.Empty)
          };
        }
      }
      return processorResponseValue;
    }
  }
}
