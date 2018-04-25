using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Reader.Utility;

namespace CaptureViewer
{
  public partial class TextExtractorForm : Form
  {
    private static SpoolerWatcher _watcher;
    private static ISpoolFileExtracter _extracter;
    private static ResultCollector _collector;

    public TextExtractorForm()
    {
      InitializeComponent();

      InitTitle();
      InitCollector();
      RegisterFileWatcher();
      InitExtracter();
    }

    private void InitCollector()
    {
      _collector = new ResultCollector();
    }

    private void InitTitle()
    {
      var version = Assembly.GetExecutingAssembly().GetName().Version;
      Text= string.Format( "Text Extractor({0})", version.ToString() );
    }

    private void RegisterFileWatcher()
    {
      _watcher = new SpoolerWatcher();
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

    private void WatcherFindNewFileEvent( string filePath )
    {
      Thread.Sleep( 300 );    //TODO : improve with changed event
      var data = _extracter.ExtractText( filePath );
      _collector.PrintToText( data );
      PrintToLog( data );
    }

    private void PrintToLog(string message)
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => PrintToLog( message ) ) );
        return;
      }
      TextBoxLogger.AppendText( message );
    }

    private void ClearLog()
    {
      if( InvokeRequired )
      {
        BeginInvoke( (Action)( () => ClearLog() ) );
        return;
      }
      TextBoxLogger.Clear(); ;
    }

    private void clearToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ClearLog();
    }

    private void openResultFolderToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ShowResultDirectory();
    }

    private void ShowResultDirectory()
    {
      var proc = Process.Start( _collector.GetResultDir() );

      Thread.Sleep( 100 );
    }
  }
}
