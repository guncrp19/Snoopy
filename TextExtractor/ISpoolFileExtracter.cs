using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleSpReader
{
  public interface ISpoolFileExtracter
  {
    string ExtractText( string filePath );
  }
}
