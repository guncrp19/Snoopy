using System;
using System.Threading;

namespace ConsoleSpReader
{
  class Program
  {
    private static SpoolerWatcher _watcher;
    private static ISpoolFileExtracter _extracter;

    static void Main( string[] args )
    {
      RegisterFileWatcher();
      InitExtracter();
      while( Console.ReadLine() != "x" )
      {
        Thread.Sleep( 100 );
      }
      UnRegisterFileWatcher();
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
      var data = _extracter.ExtractText( filePath );
      Console.WriteLine( data );
    }
  }
}
