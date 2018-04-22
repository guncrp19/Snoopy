using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace ConsoleSpReader
{
  class Program
  {
    const string spoolingPath = @"C:\Windows\System32\spool\PRINTERS\";
    private static FileSystemWatcher watcher;
    static void Main( string[] args )
    {
      RegisterFileWatcher();
      while( Console.ReadLine() != "x" )
      {
        Thread.Sleep( 100 );
      }
    }

    private static void RegisterFileWatcher()
    {
      watcher = new FileSystemWatcher()
      {
        Path = spoolingPath,
        Filter = "*.spl"
      };
      watcher.Created += Watcher_Created;
      watcher.EnableRaisingEvents = true;
      watcher.IncludeSubdirectories = true;
    }

    private static void Watcher_Created( object sender, FileSystemEventArgs e )
    {
      var latestFile = GetLatestFile( spoolingPath );
      Open( latestFile );
    }

    // Process all files in the directory passed in, recurse on any directories 
    // that are found, and process the files they contain.
    public static string GetLatestFile( string targetDirectory )
    {
      var directory = new DirectoryInfo( targetDirectory );
      var myFile = ( from f in directory.GetFiles( "*.spl" )
                     orderby f.LastWriteTime descending
                     select f ).First();

      return myFile.FullName;
    }

    public static void ProcessFile( string path )
    {
      Console.WriteLine( "Processed file '{0}'.", path );
    }

    private static void Open( string FilePath )
    {
      string sFile = FilePath;

      if( !string.IsNullOrEmpty( sFile ) )
      {
        var emfReader = new EMFSpoolfileReader.EMFSpoolfileReader();
        try
        {
          emfReader.GetTruePageCount( sFile );
        }
        catch( Exception ex )
        {
          Console.WriteLine( "File is either corrupt or not an EMF format spool file - " + ex.Message, "Error loading " + sFile );
          System.Diagnostics.Trace.TraceError( ex.ToString() );
        }


        if( emfReader.Pages.Count() > 0 )
        {
          //Console.WriteLine( "Page is acquired!" );
        }
      }
    }
  }
}
