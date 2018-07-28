using System;
using System.IO;
using System.Linq;
using Reader.Utility;

namespace CaptureViewer.Other
{
  public class SpoolerRemover
  {
    private const int MaxNumber = 10; //keep 5 spool files (.SHD and .SPL)

    public static void RemoveExceededSpoolFile(string spoolerPath)
    {
      try
      {
        var directory = new DirectoryInfo( spoolerPath );
        var list = directory.GetFiles( "*.SPL" ).Concat( directory.GetFiles( "*.SHD" ) );
        if( list.Count() <= MaxNumber )
          return;

        var query = list.OrderByDescending( file => file.CreationTime );
        var i = 0;
        foreach( var file in query )
        {
          if( i++ < MaxNumber )
          {
            continue;
          }
          else
          {
            try
            {
              File.Delete( file.FullName );
            }
            catch( Exception ex )
            {
              DebugLogger.Instance.Log( string.Format( "Remove failed. reason={0}", ex.Message ) );
            }
          }
        }
      }
      catch
      {

      }
    }
  }
}
