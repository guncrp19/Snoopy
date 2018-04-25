using System.Diagnostics;
using System.Threading;
using Reader.Utility;

namespace CaptureViewer
{
  public class TextExtractorFormController
  {
    private SpoolerWatcher _watcher;
    private ISpoolFileExtracter _extracter;
    private ResultCollector _collector;
    private readonly TextExtractorForm _extractorForm;

    public TextExtractorFormController( TextExtractorForm form )
    {
      _extractorForm = form;
      InitCollector();
      RegisterFileWatcher();
      InitExtracter();
    }

    public void ShowResultDirectory()
    {
      var proc = Process.Start( _collector.GetResultDir() );

      Thread.Sleep( 100 );
    }

    private void WatcherFindNewFileEvent( string filePath )
    {
      Thread.Sleep( 300 );    //TODO : improve with changed event
      var data = _extracter.ExtractText( filePath );
      _collector.PrintToText( data );
      _extractorForm.PrintToLog( data, _collector.FullPath );
    }

    private void RegisterFileWatcher()
    {
      _watcher = new SpoolerWatcher();
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
