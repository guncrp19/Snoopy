using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing;
namespace EMFSpoolfileReader
{

	#region "EMFMETAHEADER"
	/// <summary>
	/// The EMF header record for a spooled print job
	/// </summary>
	/// <remarks></remarks>
	public class EMFMETAHEADER
	{

		#region "Private properties"
		//\\ EMR record header
		private Int32 _iType;
		private Int32 _nSize;
		//\\ Boundary rectangle
		private Int32 _rclBounds_Left;
		private Int32 _rclBounds_Top;
		private Int32 _rclBounds_Right;
		private Int32 _rclBounds_Bottom;
		//\\ Frame rectangle
		private Int32 _rclFrame_Left;
		private Int32 _rclFrame_Top;
		private Int32 _rclFrame_Right;
		private Int32 _rclFrame_Bottom;
		//\\ "Signature"
		private byte _signature_1;
			//E
		private byte _signature_2;
			//M
		private byte _signature_3;
			//F
		private byte _signature_4;
		//\\ nVersion
		private UInt32 _nVersion;
		private Int32 _nBytes;
		private Int32 _nRecords;
		private Int32 _nHandles;
		private Int16 _sReversed;
		private Int16 _nDescription;
		private Int32 _offDescription;
		private Int32 _nPalEntries;
		private Int32 _szlDeviceWidth;
		private Int32 _szlDeviceHeight;
		private Int32 _szlDeviceWidthMilimeters;
		private Int32 _szlDeviceHeightMilimeters;
		private Int32 _cbPixelFormat;
		private Int32 _offPixelFormat;
		private bool _bOpenGL;
		private Int32 _szlMicrometersWidth;
		private Int32 _szlMicrometersHeight;

		private char[] _Description;
		#endregion

		#region "Public properties"
		/// <summary>
		/// The boundary of the printed page (the paper dimensions)
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Rectangle BoundaryRect {
			get { return new Rectangle(_rclBounds_Left, _rclBounds_Top, _rclBounds_Right, _rclBounds_Bottom); }
		}

		/// <summary>
		/// The frame containing the print elements
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>
		/// This can be smaller than the paper size as many printers have a non-printable margin
		/// around the edge of the page
		/// </remarks>
		public Rectangle FrameRect {
			get { return new Rectangle(_rclFrame_Left, _rclFrame_Top, _rclFrame_Right, _rclFrame_Bottom); }
		}

		public int Size {
			get { return _nSize; }
		}

		/// <summary>
		/// The number of EMF records that make up the printed page
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>
		/// These records describe teh text and graphical elements that make up the printed page
		/// </remarks>
		public int RecordCount {
			get { return _nRecords; }
		}

		/// <summary>
		/// The size of the EMR file
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public int FileSize {
			get { return _nBytes; }
		}

		/// <summary>
		/// The number of colours in the page's paletter
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public int PalleteEntries {
			get { return _nPalEntries; }
		}

		/// <summary>
		/// The description text for this EMF spool record
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Description {
			get { return new string( _Description); }
		}
		#endregion

		#region "Public constructor"


		public EMFMETAHEADER(BinaryReader SpoolBinaryReader)
		{
			long nStart = SpoolBinaryReader.BaseStream.Position;

			var _with1 = SpoolBinaryReader;
			_iType = _with1.ReadInt32();
			_nSize = _with1.ReadInt32();
			//\\ Boundary rectangle
			_rclBounds_Left = _with1.ReadInt32();
			_rclBounds_Top = _with1.ReadInt32();
			_rclBounds_Right = _with1.ReadInt32();
			_rclBounds_Bottom = _with1.ReadInt32();
			//\\ Frame rectangle
			_rclFrame_Left = _with1.ReadInt32();
			_rclFrame_Top = _with1.ReadInt32();
			_rclFrame_Right = _with1.ReadInt32();
			_rclFrame_Bottom = _with1.ReadInt32();
			//\\ "Signature"
			_signature_1 = _with1.ReadByte();
			_signature_2 = _with1.ReadByte();
			_signature_3 = _with1.ReadByte();
			_signature_4 = _with1.ReadByte();
			//\\ nVersion
			_nVersion = _with1.ReadUInt32();
			_nBytes = _with1.ReadInt32();
			_nRecords = _with1.ReadInt32();
			_nHandles = _with1.ReadInt16();
			_sReversed = _with1.ReadInt16();
			_nDescription = _with1.ReadInt16();
			short empty = _with1.ReadInt16();
			_offDescription = _with1.ReadInt32();
			_nPalEntries = _with1.ReadInt32();
			_szlDeviceWidth = _with1.ReadInt32();
			_szlDeviceHeight = _with1.ReadInt32();
			_szlDeviceWidthMilimeters = _with1.ReadInt32();
			_szlDeviceHeightMilimeters = _with1.ReadInt32();
			if (_nSize > 88) {
				_cbPixelFormat = _with1.ReadInt32();
				_offPixelFormat = _with1.ReadInt32();
				_bOpenGL = _with1.ReadBoolean();
			}
			if (_nSize > 100) {
				_szlMicrometersWidth = _with1.ReadInt32();
				_szlMicrometersHeight = _with1.ReadInt32();
			}
			if (_nDescription > 0) {
				SpoolBinaryReader.BaseStream.Seek(nStart + _offDescription, SeekOrigin.Begin);
				_Description = _with1.ReadChars(_nDescription);
			}
		}
		#endregion

	}
}
#endregion
