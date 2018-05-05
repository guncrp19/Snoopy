using System;
using System.Diagnostics;
using System.Threading;
using Reader.Utility;
using ServerCommunication;

namespace CaptureViewer
{
  public class TextExtractorFormController
  {
    private SpoolerWatcher _watcher;
    private ISpoolFileExtracter _extracter;
    private ResultCollector _collector;
    private readonly TextExtractorForm _extractorForm;
    private DebugForm _debugForm;
    private PostReq _serverComm;
    private ApplicationConfig _appConfig;

    public TextExtractorFormController( TextExtractorForm form )
    {
      _appConfig = new ApplicationConfig();
      InitServerComm();

      _extractorForm = form;
      InitCollector();
      RegisterFileWatcher();
      InitExtracter();

      DebugLogger.Instance.LogEvent += HandleLogMessage;
    }

    public void ShowResultDirectory()
    {
      var proc = Process.Start( _collector.GetResultDir() );

      Thread.Sleep( 100 );
    }

    public void ShowDebugDialog()
    {
      _debugForm = new DebugForm();
      _debugForm.ShowDialog();

      _debugForm = null;
    }

    public void CleanUp()
    {
      DebugLogger.Instance.LogEvent -= HandleLogMessage;
    }

    private void InitServerComm()
    {
      var settings = new PostReqSettings()
      {
        Url = _appConfig.Uri,
        SecurityProtocol =_appConfig.SecurityProtocol
      };

      _serverComm = new PostReq(settings);
    }

    private void HandleLogMessage( string message )
    {
      if( _debugForm == null )
        return;

      _debugForm.PrintToDebug( message );
    }

    private void WatcherFindNewFileEvent( string filePath )
    {
      DebugLogger.Instance.Log(string.Format("File Found = {0}", filePath));
      Thread.Sleep( 300 );    //TODO : improve with changed event
      var data = _extracter.ExtractText( filePath );
      _collector.PrintToText( data );
      _extractorForm.PrintToLog( data, _collector.FullPath );
      var t = new Thread( () => SendDataToServer( data ) );
      t.Start();
    }

    private void SendDataToServer(string data)
    {
      try
      {
        var payload = new PostReqPayload()
        {
          PostingTime = DateTime.Now.ToString(),
          UserName = _appConfig.UserName,
          Content  = data,
        };

        _serverComm.SendPostCommand( payload );
      }
      catch(Exception ex)
      {
        var errMsg = string.Format( "Failed to send data to server. Reason={0}", ex.Message );
        _extractorForm.PrintToLog( errMsg );
      }
    }

    private void RegisterFileWatcher()
    {
      _watcher = new SpoolerWatcher(_appConfig.SpoolerPath);
      _watcher.WatcherFindNewFileEvent += WatcherFindNewFileEvent;
    }

    private void InitCollector()
    {
      _collector = new ResultCollector();
    }

    private void InitExtracter()
    {
      _extracter = new SpoolFileExtracter();
    }

    private void UnRegisterFileWatcher()
    {
      _watcher.WatcherFindNewFileEvent -= WatcherFindNewFileEvent;
    }
  }
}
