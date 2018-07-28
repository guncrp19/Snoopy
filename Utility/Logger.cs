using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Utility
{
  public static class Logger
  {
    public static readonly ILog Log =
              LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
  }
}
