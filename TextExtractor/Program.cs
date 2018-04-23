using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace ConsoleSpReader
{
  class Program
  {
    private static SpoolerWatcher _watcher;
    private static ISpoolFileExtracter _extracter;

    static void Main( string[] args )
    {
      InitConsole();
      RegisterFileWatcher();
      InitExtracter();
      string cmd = "";
      do
      {
        cmd = Console.ReadLine();

        if( cmd == "c" )
          Console.Clear();

        Thread.Sleep( 100 );
      } while( cmd != "x" );

      UnRegisterFileWatcher();
    }

    private static void InitConsole()
    {
      var version = Assembly.GetExecutingAssembly().GetName().Version;
      Console.Title = string.Format("--Text Extractor--({0})", version.ToString());
    }

    private static void RegisterFileWatcher()
    {
      _watcher = new SpoolerWatcher();
      _watcher.WatcherFindNewFileEvent += WatcherFindNewFileEvent;
    }

    private static void InitExtracter()
    {
      _extracter = new SpoolFileExtracter();
    }

    private static void UnRegisterFileWatcher()
    {
      _watcher.WatcherFindNewFileEvent -= WatcherFindNewFileEvent;
    }

    private static void WatcherFindNewFileEvent( string filePath )
    {
      Thread.Sleep( 300 );    //TODO : improve with changed event
      var data = _extracter.ExtractText( filePath );
      Console.WriteLine( data );
    }
  }
}
