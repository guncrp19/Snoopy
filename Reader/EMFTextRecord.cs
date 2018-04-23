using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing;
namespace EMFSpoolfileReader
{

	/// <summary>
	/// Represents a text element on the printed page
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class EMFTextRecord : EMFRecord
	{
		private Int32 _Top;
		private Int32 _Left;
		private Int32 _Bottom;
		private Int32 _Right;
		private Int32 _GraphicsMode;
		private float _scaleX;
		private float _scaleY;
		//EMRTEXT
		private Int32 _PTx;
		private Int32 _PTy;
		private Int32 _nChars;
		private Int32 _offString;
		private Int32 _Options;
		private Int32 _TxtTop;
		private Int32 _TxtLeft;
		private Int32 _TxtBottom;
		private Int32 _TxtRight;
		private Int32 _offDX;
		private string _Text;

		public enum ExtendedTextOutputFlags
		{
			ETO_OPAQUE = 0x2,
			ETO_CLIPPED = 0x4,
			ETO_GLYPH_INDEX = 0x10,
			ETO_RTLREADING = 0x80,
			ETO_NUMERICSLOCAL = 0x400,
			ETO_NUMERICSLATIN = 0x800,
			ETO_IGNORELANGUAGE = 0x1000,
			ETO_PDY = 0x2000
		}

		/// <summary>
		/// The text of this text element
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public string Text
    {
			get { return _Text; }
		}

		/// <summary>
		/// True if the text is clipped inside the <see cref="EMFTextRecord.BoundaryRectangle">boundary 
		/// rectangle</see>
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>
		/// </remarks>
		public bool Clipped
    {
			get { return (_Options & (int)ExtendedTextOutputFlags.ETO_CLIPPED) == (int)ExtendedTextOutputFlags.ETO_CLIPPED; }
		}

		/// <summary>
		/// True if the text is surrounded by an opaque rectangle
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks>
		/// </remarks>
		public bool Opaque
    {
			get { return (_Options & (int)ExtendedTextOutputFlags.ETO_OPAQUE) == (int)ExtendedTextOutputFlags.ETO_OPAQUE; }
		}

		/// <summary>
		/// The rectangle that bounds this text element
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Rectangle BoundaryRectangle
    {
			get { return new Rectangle(_Left, _Top, _Right - _Left, _Bottom - _Top); }
		}

		public EMFTextRecord(int Length, BinaryReader SpoolBinaryReader) : base(SpoolBinaryReader)
		{
			var _with1 = SpoolBinaryReader;
			_Top = _with1.ReadInt32();
			_Left = _with1.ReadInt32();
			_Bottom = _with1.ReadInt32();
			_Right = _with1.ReadInt32();
			_GraphicsMode = _with1.ReadInt32();
			_scaleX = _with1.ReadSingle();
			_scaleY = _with1.ReadSingle();
			_PTx = _with1.ReadInt32();
			_PTy = _with1.ReadInt32();
			_nChars = _with1.ReadInt32();
			_offString = _with1.ReadInt32();
			_Options = _with1.ReadInt32();
			_TxtTop = _with1.ReadInt32();
			_TxtLeft = _with1.ReadInt32();
			_TxtBottom = _with1.ReadInt32();
			_TxtRight = _with1.ReadInt32();
			_offDX = _with1.ReadInt32();
			if (_offString >= 76)
      {
				_with1.BaseStream.Seek(_offString - 76, SeekOrigin.Current);
				byte[] chars = null;
				chars = _with1.ReadBytes(_nChars * 2);
				_Text = System.Text.Encoding.Unicode.GetString(chars);
			}
		}

	}
}
