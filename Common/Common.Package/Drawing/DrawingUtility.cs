using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Package.Drawing.Ico;
using System.Drawing.Imaging;
using Common.Utility;
using System.Collections.Concurrent;
using FileIconDict = System.Collections.Concurrent.ConcurrentDictionary<System.Tuple<string, uint>, System.Drawing.Icon>;
using FileIconDictKey = System.Tuple<string, uint>;

namespace Common.Package.Drawing
{
    /// <summary>
    /// 绘制工具
    /// </summary>
    public static class DrawingUtility
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };


        class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0;
            public const uint SHGFI_SMALLICON = 0x1;

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref   SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        }

        /// <summary>
        /// 获取指定文件的小图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Icon GetSmallIcon(string fileName)
        {
            return _GetIcon(fileName, Win32.SHGFI_SMALLICON);
        }

        private static readonly AutoLoad<FileIconDict> _fileIconDict = new AutoLoad<FileIconDict>(() => new FileIconDict(), TimeSpan.FromMinutes(1));

        private static Icon _GetIcon(string fileName, uint icoSize)
        {
            bool exist = File.Exists(fileName);
            Icon icon = null;
            FileIconDict dict = null;

            if (!exist)
            {
                string ext = Path.GetExtension(fileName);
                if ((dict = _fileIconDict.Value).TryGetValue(new FileIconDictKey((ext ?? "").ToLower(), icoSize), out icon))
                    return icon;

                fileName = Path.GetTempFileName().Replace(".", "") + ext;
                File.WriteAllBytes(fileName, Array<byte>.Empty);
            }

            try
            {
                IntPtr hImgLarge;
                SHFILEINFO shinfo = new SHFILEINFO();
                hImgLarge = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | icoSize);
                if (shinfo.hIcon != IntPtr.Zero)
                    icon = System.Drawing.Icon.FromHandle(shinfo.hIcon);

                return icon;
            }
            finally
            {
                if (!exist)
                {
                    PathUtility.DeleteIfExists(fileName);
                    dict[new FileIconDictKey((Path.GetExtension(fileName) ?? "").ToLower(), icoSize)] = icon;
                }
            }
        }

        /// <summary>
        /// 获取指定文件的大图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Icon GetLargeIcon(string fileName)
        {
            return _GetIcon(fileName, Win32.SHGFI_LARGEICON);
        }

        /// <summary>
        /// 从ICO图标中读取所有的位图
        /// </summary>
        /// <param name="icoStream"></param>
        /// <returns></returns>
        public static Bitmap[] ReadAllFromIco(Stream icoStream)
        {
            Contract.Requires(icoStream != null);

            EOIcoCurLoader loader = new EOIcoCurLoader(icoStream);

            int count = loader.CountImages();
            Bitmap[] bmpArr = new Bitmap[count];

            for (int k = 0; k < count; k++)
            {
                bmpArr[k] = loader.GetImage(k);
            }

            return bmpArr;
        }

        /// <summary>
        /// 从ICO图标文件中读取所有的位图
        /// </summary>
        /// <param name="icoFile"></param>
        /// <returns></returns>
        public static Bitmap[] ReadAllFromIco(string icoFile)
        {
            Contract.Requires(icoFile != null);

            using (FileStream fs = new FileStream(icoFile, FileMode.Open))
            {
                return ReadAllFromIco(fs);
            }
        }

        /// <summary>
        /// 从ICO图标中读取所有的位图
        /// </summary>
        /// <param name="ico"></param>
        /// <returns></returns>
        public static Bitmap[] ReadAllFromIco(Icon ico)
        {
            Contract.Requires(ico != null);
            using (MemoryStream ms = new MemoryStream())
            {
                ico.Save(ms);
                ms.Position = 0;

                return ReadAllFromIco(ms);
            }
        }
    }
}
