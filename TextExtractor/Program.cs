using System;
using System.Reflection;
using System.Threading;
using Reader.Utility;

namespace ConsoleSpReader
{
  class Program
  {
    private static SpoolerWatcher _watcher;
    private static ISpoolFileExtracter _extracter;
    private static ResultCollector _collector;

    static void Main( string[] args )
    {
      InitConsole();
      InitCollector();
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

    private static void InitCollector()
    {
      _collector = new ResultCollector();
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
      _collector.PrintToText( data );
      Console.WriteLine( "Captured Success!. Path = {0}", _collector.FullPath );
    }
  }
}
