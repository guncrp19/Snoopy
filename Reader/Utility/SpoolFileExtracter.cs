using System;
using System.IO;
using System.Reflection;
using EMFSpoolfileReader;
using Utility;

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
        File.Copy( filePath, tempPath, true);
        var emfReader = new SpoolfileReader();
        try
        {
          retVal = emfReader.ExtractText( tempPath );
        }
        catch( Exception ex )
        {
          string errMsg = string.Format( "File is either corrupt or not an EMF format spool file. Error loading : {0}", filePath );
          Logger.LogError( errMsg );
          Logger.LogException(ex);
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
