using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CaptureViewer.Other;
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
    private PostReq _postRequest;
    private PostWorker _postWorker;
    private ApplicationConfig _appConfig;

    public TextExtractorFormController( TextExtractorForm form )
    {
      _appConfig = new ApplicationConfig();
      _extractorForm = form;
      InitServerComm();     
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
      _postWorker.AbortWorker();
      DebugLogger.Instance.LogEvent -= HandleLogMessage;
    }

    private void InitServerComm()
    {
      string progDataPath = Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData );
      string workerPath = Path.Combine( progDataPath, @"Snoopy" );

      //init result collector
      _collector = new ResultCollector( workerPath );

      //init post worker
      var settings = new PostReqSettings()
      {
        Url = _appConfig.Uri,
        SecurityProtocol =_appConfig.SecurityProtocol
      };

      _postRequest = new PostReq(settings);

      var workerSetting = new PostWorkerSettings()
      {
        WorkingDirectory = workerPath,
        UserName = _appConfig.UserName,
        PostReq = _postRequest
      };

      _postWorker = new PostWorker( workerSetting );
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
      _extractorForm.PrintToLog( data );
      var t2 = new Thread(() => SpoolerRemover.RemoveExceededSpoolFile(_appConfig.SpoolerPath));
      t2.Start();
    }

    private void RegisterFileWatcher()
    {
      _watcher = new SpoolerWatcher(_appConfig.SpoolerPath);
      _watcher.WatcherFindNewFileEvent += WatcherFindNewFileEvent;
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
