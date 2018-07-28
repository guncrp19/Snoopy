using System;
using System.Management;
using log4net;
using ServerCommunication;

namespace TestPrj
{
  class Program
  {
    public static readonly ILog Log =
              LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

    static PostWorker _worker;
    static void TestPostWorker()
    {
      string path = @"D:\TestingFolder";
      PostWorkerSettings setting = new PostWorkerSettings()
      {
        WorkingDirectory = path,
        UserName = "test", 
        PostReq = new PostReq(new PostReqSettings(){ SecurityProtocol = "tls", Url="localhost"} )
      };

      _worker = new PostWorker( setting );

    }

    static void Main( string[] args )
    {
      Log.Error("eRROR 1 test!");

      TestPostWorker();

      Console.WriteLine( "Retrieving printer queue information using WMI" );
      Console.WriteLine( "==================================" );
      //Query printer queue
      System.Management.ObjectQuery oq = new System.Management.ObjectQuery
("SELECT * FROM Win32_PrintJob");
      ManagementObjectSearcher query1 = new ManagementObjectSearcher( oq );
      ManagementObjectCollection queryCollection1 = query1.Get();
      foreach( ManagementObject mo in queryCollection1 )
      {
        Console.WriteLine( "Printer Driver : " + mo["DriverName"].ToString() );
        Console.WriteLine( "Document Name : " + mo["Document"].ToString() );
        Console.WriteLine( "Document Owner : " + mo["Owner"].ToString() );
        Console.WriteLine("Status: " + mo["JobStatus"] );
        Console.WriteLine( "Job ID: " + mo["JobId"].ToString() );
        Console.WriteLine( "Parameters: " + mo["Parameters"] );
        Console.WriteLine( "PrintProcessor: " + mo["PrintProcessor"].ToString() );
        Console.WriteLine( "==================================" );
      }

      Console.ReadLine();
    }
  }
}
