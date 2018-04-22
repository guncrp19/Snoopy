using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace EMFSpoolfileReader
{

	/// <summary>
	/// Class the represents the properties of a single page of the printed document
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class EMFPage
	{
		private EMFMETAHEADER _Header;

		/// <summary>
		/// The header information for this printed page
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public EMFMETAHEADER Header
    {
			get { return _Header; }
		}

		/// <summary>
		/// Creates a new EMF page record from an open file stream which is proitioned at the 
		/// start of the EMF record 
		/// </summary>
		/// <param name="FileReader">The open file stream</param>
		/// <remarks></remarks>

		public EMFPage(BinaryReader FileReader)
		{
			if (EmfSpoolfileReader.ApplicationTracing.TraceVerbose)
      {
				Trace.WriteLine("New page: " + _Header.RecordCount.ToString() + " EMF records");
			}

			//\\ Get the EMF page header
			long oldPos = FileReader.BaseStream.Position;

			_Header = new EMFMETAHEADER(FileReader);
			FileReader.BaseStream.Seek(oldPos, System.IO.SeekOrigin.Begin);
			FileReader.BaseStream.Seek(oldPos + _Header.Size, SeekOrigin.Begin);

			int Record = 0;
      textBuffer = new List<string>();

      for (Record = 1; Record <= _Header.RecordCount; Record++)
      {
        var emfRecord = new EMFRecord( FileReader );
        var txtData = ParseRecord( emfRecord, FileReader );
        if(!string.IsNullOrEmpty( txtData ) )
         textBuffer.Add( txtData );
				FileReader.BaseStream.Seek(emfRecord.Seek + emfRecord.Size, SeekOrigin.Begin);
			}
		}

    public List<string> GetTextList()
    {
      return textBuffer;
    }

    public string GetText()
    {
      var strBuilder = new StringBuilder();
      foreach(string str in textBuffer)
      {
        strBuilder.AppendLine( str );
      }
      return strBuilder.ToString();
    }

    private List<string> textBuffer;

    private void PrintRecord( EMFRecord record, BinaryReader FileReader )
    {
      var prevPos = FileReader.BaseStream.Position;
      FileReader.BaseStream.Seek( record.Seek, SeekOrigin.Begin );
      var recordData = FileReader.ReadChars( record.Size + 8 );
      Console.WriteLine( "[RECORD DATA TYPE] {0} {1} {2} {3}", recordData [0], recordData[1], recordData[2], recordData[3] );
      Console.WriteLine( "[RECORD DATA] {0} {1} {2} {3}", recordData[4], recordData[5], recordData[6], recordData[7] );
      Console.WriteLine( "[RECORD DATA] {0}", new string( recordData ) );
      FileReader.BaseStream.Seek( prevPos, SeekOrigin.Begin );
    }

    private void PrintRecordByte( EMFRecord record, BinaryReader FileReader )
    {
      var prevPos = FileReader.BaseStream.Position;
      FileReader.BaseStream.Seek( record.Seek, SeekOrigin.Begin );
      var recordData = FileReader.ReadBytes( record.Size + 8 );
      Console.WriteLine( "Record data:" );
      for( int i = 0; i < recordData.Length; i++ )
      {
        Console.Write("{0} ", recordData[i].ToString("x"));
      }
      Console.WriteLine( "" );
      FileReader.BaseStream.Seek( prevPos, SeekOrigin.Begin );
    }

    private string ParseRecord( EMFRecord record, BinaryReader FileReader)
    {
      string retVal = "";
      if( record.Type == EmfPlusRecordType.EmfSmallTextOut )
      {
        var prevPos = FileReader.BaseStream.Position;

        //lets parse it
        int seekSkip = 0;
        seekSkip += 8; //skip EMR
        seekSkip += 8; //skip ptlref

        FileReader.BaseStream.Seek( record.Seek + seekSkip, SeekOrigin.Begin );

        int charCount = FileReader.ReadInt32(); //read nchars
        int fuOptions = FileReader.ReadInt32(); //read fuOptions
        if( fuOptions != 0x1300 )
          return retVal;

        var curPos = FileReader.BaseStream.Position;
        seekSkip = 4; //skip iGraphicMode
        seekSkip += 4; //skip exScale
        seekSkip += 4; //skip eyScale
        FileReader.BaseStream.Seek( curPos + seekSkip, SeekOrigin.Begin );

        var dataChars = FileReader.ReadChars( charCount );
        string data = new string( dataChars );

        FileReader.BaseStream.Seek( prevPos, SeekOrigin.Begin );
        return data;
      }
      return retVal;
    }
	}
}
