﻿using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Gamer.Core
{
    /// <summary>
    /// Stores information about a 2D texture.
    /// </summary>
    public class Texture2DInfo
    {
        public enum RawDataFormat : byte { Img, Raw, DXT }

        public RawDataFormat DataFormat;
        public int Width, Height;
        public TextureFormat Format;
        public bool HasMipmaps;
        public byte[] RawData;

        const int DDS_HEADER_SIZE = 128;
        public Texture2DInfo(RawDataFormat dataFormat, TextureFormat format, byte[] rawData)
        {
            DataFormat = dataFormat;
            switch (dataFormat)
            {
                case RawDataFormat.Img:
                    LoadImage(rawData);
                    return;
                case RawDataFormat.DXT:
                    if (format != TextureFormat.DXT1 && format != TextureFormat.DXT5)
                        throw new ArgumentOutOfRangeException(nameof(format), "Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");
                    Format = format;
                    var ddsSizeCheck = rawData[4];
                    if (ddsSizeCheck != 124)
                        throw new ArgumentOutOfRangeException(nameof(rawData), "Invalid DDS DXTn texture. Unable to read"); // this header byte should be 124 for DDS image files
                    Height = (rawData[13] << 8) | rawData[12];
                    Width = (rawData[17] << 8) | rawData[16];
                    var dxtBytes = new byte[rawData.Length - DDS_HEADER_SIZE];
                    Buffer.BlockCopy(dxtBytes, DDS_HEADER_SIZE, dxtBytes, 0, rawData.Length - DDS_HEADER_SIZE);
                    RawData = dxtBytes;
                    return;
                case RawDataFormat.Raw:
                    RawData = rawData;
                    return;
                default: throw new NotImplementedException($"{dataFormat}");
            }
        }

        public Texture2DInfo(int width, int height, TextureFormat format, bool hasMipmaps, byte[] rawData)
        {
            DataFormat = RawDataFormat.Raw;
            Width = width;
            Height = height;
            Format = format;
            HasMipmaps = hasMipmaps;
            RawData = rawData;
        }

        /// <summary>
        /// Creates a Unity Texture2D from this Texture2DInfo.
        /// </summary>
        public Texture2D ToTexture2D()
        {
            var tex = new Texture2D(Width, Height, Format, HasMipmaps);
            if (RawData != null)
            {
                tex.LoadRawTextureData(RawData);
                tex.Apply();
                tex.Compress(true);
            }
            return tex;
        }

        public void LoadImage(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var r = new EndianBinaryReader(ms, Endian.LittleEndian))
            {
                var magicString = r.ReadBytes(4);
                if (!"IMG ".EqualsASCIIBytes(magicString))
                    throw new FileFormatException($"Invalid IMG file magic string: \"{Encoding.ASCII.GetString(magicString)}\".");
                DataFormat = (RawDataFormat)r.ReadByte();
                HasMipmaps = r.ReadByte() != 0;
                Format = (TextureFormat)r.ReadInt16();
                Width = r.ReadInt32();
                Height = r.ReadInt32();
                var rawDataSize = r.ReadInt32();
                RawData = r.ReadBytes(rawDataSize);
            }
        }

        public byte[] GetImage()
        {
            using (var ms = new MemoryStream())
            using (var r = new EndianBinaryWriter(ms, Endian.LittleEndian))
            {
                r.Write(Encoding.ASCII.GetBytes("IMG "));
                r.Write((byte)DataFormat);
                r.Write((byte)DataFormat);
                r.Write((byte)(HasMipmaps ? 1 : 0));
                r.Write((short)Format);
                r.Write(Width);
                r.Write(Height);
                r.Write(RawData.Length);
                r.Write(RawData);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        #region Transform

        public void Rotate2D(int angle)
        {
            if (DataFormat != RawDataFormat.Raw)
                throw new InvalidOperationException("Invalid DataFormat. Only Raw formats are supported by this method.");
            if (Format == TextureFormat.BGRA32)
                Rotate2DBGRA32(Mathf.Deg2Rad * angle);
        }

        unsafe void Rotate2DBGRA32_(float phi)
        {
            var W = Width; var H = Height;
            var pixels = new byte[W * H * 4];
            fixed (byte* pPixels = pixels, pData = RawData)
            {
                var rPixels = (uint*)pPixels;
                var rData = (uint*)pData;
                for (var j = 0; j < W; ++j)
                    for (var i = 0; i < H; ++i)
                        rPixels[i * W + j] = rData[(W - j - 1) * W + i];
            }
            RawData = pixels;
        }

        unsafe void Rotate2DBGRA32(float phi)
        {
            var sn = Mathf.Sin(phi);
            var cs = Mathf.Cos(phi);

            var W = Width; var H = Height;
            var xc = W / 2; var yc = H / 2;

            var pixels = new byte[W * H * 4];
            fixed (byte* pPixels = pixels, pData = RawData)
            {
                var rPixels = (uint*)pPixels;
                var rData = (uint*)pData;
                int x, y;
                for (var j = 0; j < H; ++j)
                    for (var i = 0; i < W; ++i)
                    {
                        x = (int)(cs * (i - xc) + sn * (j - yc) + xc);
                        y = (int)(-sn * (i - xc) + cs * (j - yc) + yc);
                        if (x > -1 && x < W && y > -1 && y < H)
                            rPixels[j * W + i] = rData[y * W + x];
                    }
                ////
                //x = 0; y = 0;
                //var W2 = W; var H2 = H;
                //for (var j = 0; j < H; ++j)
                //    for (var i = 0; i < W; ++i)
                //        rData[W2 / 2 - W / 2 + x + i + W2 * (H2 / 2 - H / 2 + j + y)] = rPixels[i + j * W];
            }
            RawData = pixels;
        }

        static void Flip2DSubArrayVertically(byte[] arr, int startIndex, int rowCount, int columnCount)
        {
            //Assert(startIndex >= 0 && rowCount >= 0 && columnCount >= 0 && (startIndex + (rowCount * columnCount)) <= arr.Length);
            var tmpRow = new byte[columnCount];
            var lastRowIndex = rowCount - 1;
            for (var rowIndex = 0; rowIndex < (rowCount / 2); rowIndex++)
            {
                var otherRowIndex = lastRowIndex - rowIndex;
                var rowStartIndex = startIndex + (rowIndex * columnCount);
                var otherRowStartIndex = startIndex + (otherRowIndex * columnCount);
                Array.Copy(arr, otherRowStartIndex, tmpRow, 0, columnCount); // other -> tmp
                Array.Copy(arr, rowStartIndex, arr, otherRowStartIndex, columnCount); // row -> other
                Array.Copy(tmpRow, 0, arr, rowStartIndex, columnCount); // tmp -> row
            }
        }

        #endregion
    }
}