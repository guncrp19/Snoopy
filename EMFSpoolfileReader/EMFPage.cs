using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

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
		private Image _Thumbnail;
		private Int32 _Scale = 20;
		private System.Collections.Generic.List<EMFRecord> _Records = new System.Collections.Generic.List<EMFRecord>();
		private Metafile _emfFile;

		/// <summary>
		/// The header information for this printed page
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public EMFMETAHEADER Header {
			get { return _Header; }
		}

		/// <summary>
		/// A thumbnail image of the printed page
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>
		/// The size of this thumbnail is controlled by the _Scale private constant
		/// </remarks>
		public Image Thumbnail {
			get { return _emfFile.GetThumbnailImage(Convert.ToInt32(_Header.FrameRect.Width / _Scale), Convert.ToInt32(_Header.FrameRect.Height / _Scale), AbortThumbnail, IntPtr.Zero); }
		}

		public Metafile Metafile {
			get { return _emfFile; }
		}

		public int Scale {
			get { return _Scale; }
			set {
				if ((value < 10)) {
					_Scale = 10;
				} else if ((value > 200)) {
					_Scale = 200;
				} else {
					_Scale = value;
				}

			}
		}

		/// <summary>
		/// Creates a new EMF page record from an open file stream which is proitioned at the 
		/// start of the EMF record 
		/// </summary>
		/// <param name="FileReader">The open file stream</param>
		/// <remarks></remarks>

		public EMFPage(System.IO.BinaryReader FileReader)
		{
			if (EMFSpoolfileReader.ApplicationTracing.TraceVerbose) {
				Trace.WriteLine("New page: " + _Header.RecordCount.ToString() + " EMF records");
			}

			//\\ Get the EMF page header
			long oldPos = FileReader.BaseStream.Position;

			_Header = new EMFMETAHEADER(FileReader);
			FileReader.BaseStream.Seek(oldPos, System.IO.SeekOrigin.Begin);

			if (EMFSpoolfileReader.ApplicationTracing.TraceVerbose)
      {
				Trace.WriteLine("Generating thumbnail");
			}
			_emfFile = new System.Drawing.Imaging.Metafile(FileReader.BaseStream);


			FileReader.BaseStream.Seek(oldPos + _Header.Size, System.IO.SeekOrigin.Begin);

			int Record = 0;
			for (Record = 1; Record <= _Header.RecordCount; Record++)
      {
				var emfRecord = new EMFRecord(FileReader);
				_Records.Add(emfRecord);
        PrintRecord( emfRecord, FileReader, EmfPlusRecordType.EmfSmallTextOut );
				FileReader.BaseStream.Seek(emfRecord.Seek + emfRecord.Size, System.IO.SeekOrigin.Begin);
			}

		}

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

    private void PrintRecord( EMFRecord record, BinaryReader FileReader, EmfPlusRecordType type)
    {
      if( record.Type == type )
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
          return;

        var curPos = FileReader.BaseStream.Position;
        seekSkip = 4; //skip iGraphicMode
        seekSkip += 4; //skip exScale
        seekSkip += 4; //skip eyScale
        FileReader.BaseStream.Seek( curPos + seekSkip, SeekOrigin.Begin );

        var dataChars = FileReader.ReadChars( charCount );
        string data = new string( dataChars );

        //Console.WriteLine( record.Type.ToString() + " :: " + ( record.Size - 8 ) );
        Console.WriteLine( data );

        FileReader.BaseStream.Seek( prevPos, SeekOrigin.Begin );
      }
    }

    private bool AbortThumbnail()
		{
			return false;
		}
	}
}
namespace EMFSpoolfileReader
{
	/// <summary>
	/// A type-safe collection of <see cref="EMFPage">EMFPage</see> objects representing the 
	/// pages of this print job 
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class EMFPages
	{
    private List<EMFPage> emfPages;
    public EMFPages()
    {
      emfPages = new List<EMFPage>();
    }

    public void Add( EMFPage page)
    {
      emfPages.Add( page );
    }

    public int Count()
    {
      return emfPages.Count;
    }

    public List<EMFPage> Pages
    {
      get
      {
        return emfPages;
      }
    }
  }
}
