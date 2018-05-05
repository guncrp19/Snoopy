using System;
using System.Xml;

namespace CaptureViewer
{
  public class ApplicationConfig
  {
    public string Uri;
    public string UserName;
    public string SecurityProtocol;

    public ApplicationConfig()
    {
      Load();
    }

    private void Load()
    {
      try
      {
        var doc = new XmlDocument();
        doc.Load( "Config/Application.config" );
        Uri = GetConfig( doc, "Uri" );
        UserName = GetConfig( doc, "UserName" );
        SecurityProtocol = GetConfig( doc, "SecurityProtocol" );
      }
      catch
      {
        throw new Exception( "Configuration can't be loaded. It might contains wrong configuration" );
      }
    }

    private string GetConfig(XmlDocument doc, string tagName)
    {
      var node = doc.SelectNodes( string.Format( "//configuration/{0}", tagName) );
      return node[0].InnerText;
    }
  }
}
