using System.IO;
using System.Reflection;

namespace TextExtractor
{
  public class ResultCollector
  {
    public string FullPath
    {
      get;
      private set;
    }

    public void PrintToText(string data)
    {
      FullPath = GenerateFilePath();
      File.WriteAllText( FullPath, data);
    }

    private string GenerateFilePath()
    {
      var path = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
      path = Path.Combine( path, @"Result" );
      if( !Directory.Exists( path ) )
        Directory.CreateDirectory( path );

      return GenerateIncrementFileName( path );
    }

    private string GenerateIncrementFileName(string dir)
    {
      string baseName = "TextCapture_{0}.txt";
      int index = 0;
      string filePath = "";
      do
      {
        filePath = Path.Combine( dir, string.Format( baseName, index.ToString().PadLeft( 4, '0' ) ) );
        index++;
      }
      while(File.Exists(filePath));

      return filePath;
    }
  }
}
