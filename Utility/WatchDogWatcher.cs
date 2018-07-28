using System.IO;
using System.Threading;

namespace Utility
{
  public class WatchDogWatcher:AppWatcherBase
  {
    public WatchDogWatcher(string appName) : base(appName)
    {

    }

    protected override bool RuleCheck()
    {
      return !File.Exists( spFp );
    }

    protected override void ActionIfRuleCheck()
    {
      CreateWdFile();
    }

    private void CreateWdFile()
    {
      while( !File.Exists( spFp ) )
      {
        try
        {
          using( var file = new FileStream( spFp, FileMode.Create, FileAccess.ReadWrite, FileShare.Delete ) )
          {
            file.Close();
          }
        }
        catch{ }
        Thread.Sleep(100);
      }
    }
  }
}
