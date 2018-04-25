using System;
using System.IO;
using System.Reflection;
using EMFSpoolfileReader;

namespace Reader.Utility
{
  public class SpoolFileExtracter : ISpoolFileExtracter
  {
    public string ExtractText( string filePath )
    {
      string retVal = "";
      if( !string.IsNullOrEmpty( filePath ) )
      {
        var tempPath = Path.Combine(Path.GetDirectoryName( Assembly.GetEntryAssembly().Location ), Path.GetFileName( filePath ) );
        File.Copy( filePath, tempPath );
        var emfReader = new SpoolfileReader();
        try
        {
          retVal = emfReader.ExtractText( tempPath );
        }
        catch( Exception ex )
        {
          Console.WriteLine( "File is either corrupt or not an EMF format spool file - " + ex.Message, "Error loading " + filePath );
          System.Diagnostics.Trace.TraceError( ex.ToString() );
        }
        finally
        {
          File.Delete( tempPath );
        }
      }
      return retVal;
    }
  }
}
