using System;
using System.Management;

namespace TestPrj
{
  class Program
  {
    static void Main( string[] args )
    {
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
