using System;
using EMFSpoolfileReader;

namespace ConsoleSpReader
{
  public class SpoolFileExtracter : ISpoolFileExtracter
  {
    public string ExtractText( string filePath )
    {
      string retVal = "";
      if( !string.IsNullOrEmpty( filePath ) )
      {
        var emfReader = new EmfSpoolfileReader();
        try
        {
          retVal = emfReader.ExtractText( filePath );
        }
        catch( Exception ex )
        {
          Console.WriteLine( "File is either corrupt or not an EMF format spool file - " + ex.Message, "Error loading " + filePath );
          System.Diagnostics.Trace.TraceError( ex.ToString() );
        }
      }
      return retVal;
    }
  }
}
