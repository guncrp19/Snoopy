using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reader.Utility
{
  public class SpoolerWatcher
  {
    const string spoolingPath = @"C:\Windows\System32\spool\PRINTERS\";
    private FileSystemWatcher _watcher;

    public delegate void OnWatcherFindNewFile( string filePath );
    public event OnWatcherFindNewFile WatcherFindNewFileEvent;

    public SpoolerWatcher()
    {
      _watcher = new FileSystemWatcher()
      {
        Path = spoolingPath,
        Filter = "*.spl"
      };
      _watcher.Created += WatcherOnFileCreated;
      _watcher.EnableRaisingEvents = true;
      _watcher.IncludeSubdirectories = true;
    }

    private void InvokeWatcherFindNewFileEvent(string filePath )
    {
      if( WatcherFindNewFileEvent != null )
        WatcherFindNewFileEvent( filePath );
    }

    private void WatcherOnFileCreated( object sender, FileSystemEventArgs e )
    {
      var latestFile = GetLatestFile( spoolingPath );
      InvokeWatcherFindNewFileEvent( latestFile );
    }

    private string GetLatestFile( string targetDirectory )
    {
      var directory = new DirectoryInfo( targetDirectory );
      var myFile = ( from f in directory.GetFiles( "*.spl" )
                     orderby f.LastWriteTime descending
                     select f ).First();

      return myFile.FullName;
    }
  }
}
