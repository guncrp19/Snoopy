using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader.Utility
{
  public interface ISpoolFileExtracter
  {
    string ExtractText( string filePath );
  }
}
