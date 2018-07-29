using System.IO;
using System.Reflection;
using Utility;

namespace Reader.Utility
{
  public class ResultCollector
  {
    private readonly string _workingDir;

    public ResultCollector( string workingDir )
    {
      _workingDir = workingDir;
    }

    public void PrintToText(string data)
    {
      string path = GenerateFilePath();
      Logger.LogInfo(string.Format("print to ={0}, data={1} ", path, data));
      File.WriteAllText( path, data);
    }

    public string GetResultDir()
    {
      if( !Directory.Exists( _workingDir ) )
        Directory.CreateDirectory( _workingDir );

      return _workingDir;
    }

    private string GenerateFilePath()
    {
      return GenerateIncrementFileName( GetResultDir());
    }

    private string GenerateIncrementFileName(string dir)
    {
      string baseName = "Snoop_{0}.txt";
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
