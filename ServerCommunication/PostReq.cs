using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using Utility;

namespace ServerCommunication
{
  public class PostReq
  {
    private string _url;
    private SecurityProtocolType _sProtocol;

    public PostReq( PostReqSettings settings)
    {
      _url = settings.Url;
      _sProtocol = (SecurityProtocolType)Enum.Parse( typeof( SecurityProtocolType ), settings.SecurityProtocol, true );
    }

    public string SendPostCommand(PostReqPayload payload)
    {
      Logger.LogInfo("Start Post data");
      var httpWebRequest = (HttpWebRequest)WebRequest.Create( _url );
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Method = "POST";
      ServicePointManager.SecurityProtocol = _sProtocol;

      using( var streamWriter = new StreamWriter( httpWebRequest.GetRequestStream() ) )
      {
        var json = new JavaScriptSerializer().Serialize( payload);
        streamWriter.Write( json );
        streamWriter.Flush();
        streamWriter.Close();
      }

      var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
      string result = "";
      using( var streamReader = new StreamReader( httpResponse.GetResponseStream() ) )
      {
        result = streamReader.ReadToEnd();
      }
      Logger.LogInfo( string.Format("Post data SUCCESS! result={0}", result) );
      return result;
    }

    
  }
}
