using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using System.Drawing;
using System.Diagnostics.Contracts;
using System.Drawing.Imaging;
using System.IO;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package.Drawing;

namespace Common.Package.UIObject
{
    public class UIObjectImageLoader : MarshalByRefObjectEx, IUIObjectImageLoader
    {
        public UIObjectImageLoader(Image image)
            : this(new[] { image })
        {
            
        }

        public UIObjectImageLoader(Image[] images)
        {
            Contract.Requires(images != null);
            _images = _ReadAllImages(images);
        }

        public UIObjectImageLoader(Icon ico)
            : this(new[] { ico })
        {

        }

        public UIObjectImageLoader(Icon[] icos)
            : this(_ReadAllImages(icos))
        {

        }

        private static Image[] _ReadAllImages(Icon[] icos)
        {
            if (icos == null)
                return Array<Image>.Empty;

            List<Image> imgs = new List<Image>();
            foreach (Icon ico in icos.WhereNotNull())
            {
                imgs.AddRange(DrawingUtility.ReadAllFromIco(ico));
            }

            return imgs.ToArray();
        }

        private static Image[] _ReadAllImages(Image[] images)
        {
            List<Image> imgs = new List<Image>();
            foreach (Image img in images)
            {
                if (img.RawFormat == ImageFormat.Icon)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, ImageFormat.Icon);
                        ms.Position = 0;
                        imgs.AddRange(DrawingUtility.ReadAllFromIco(ms));
                    }
                }
                else
                {
                    imgs.Add(img);
                }
            }

            return imgs.ToArray();
        }

        private readonly Image[] _images;
        private Item[] _items;

        class Item
        {
            public Size Size { get; set; }
            public Image Image { get; set; }
        }

        private Item[] _GetItems()
        {
            if (_items != null)
                return _items;

            Dictionary<Size, Image> dict = new Dictionary<Size, Image>();

            foreach (Image image in _images.WhereNotNull())
            {
                Guid[] dimensionIds = image.FrameDimensionsList;
                if (dimensionIds.Length > 1)
                {
                    foreach (Guid dimensionId in dimensionIds)
                    {
                        FrameDimension dimension = new FrameDimension(dimensionId);
                        for (int index = 0, length = image.GetFrameCount(dimension); index < length; index++)
                        {
                            image.SelectActiveFrame(dimension, index);
                            MemoryStream ms = new MemoryStream();
                            image.Save(ms, ImageFormat.Png);
                            ms.Seek(0, SeekOrigin.Begin);
                            Image img = Image.FromStream(ms);

                            dict[img.Size] = img;
                        }
                    }
                }
                else
                {
                    dict[image.Size] = image;
                }
            }

            Item[] items = dict.OrderBy(item => _ToInt(item.Key)).Select(item => new Item() { Size = item.Key, Image = item.Value }).ToArray();
            return _items = items;
        }

        private static int _Diff(Size s1, Size s2)
        {
            return Math.Abs(_ToInt(s1) - _ToInt(s2));
        }

        private static int _ToInt(Size size)
        {
            return size.Width + size.Height;
        }

        public UIObjectImage GetImage(Size size)
        {
            Item[] items = _GetItems();

            int d = int.MaxValue;
            Image image = null;
            for (int k = 0, length = items.Length; k < length; k++)
            {
                Item item = items[k];
                int d0 = _Diff(item.Size, size);

                if (d0 <= d)
                {
                    d = d0;
                    image = item.Image;
                }
                else break;
            }

            if (image != null)
                return (UIObjectImage)_ReviseImageSize(image, size);

            return null;
        }

        private Image _ReviseImageSize(Image image, Size size)
        {
            if (image.Size == size)
                return image;

            return _cache.GetOrAddOfRelative(new Tuple<Size, Image>(size, image), TimeSpan.FromMinutes(5), delegate {
                return new Bitmap(image, size);
            });
        }

        private static readonly Cache<Tuple<Size, Image>, Image> _cache = new Cache<Tuple<Size, Image>, Image>();

        /// <summary>
        /// 从字节数组中创建
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static IUIObjectImageLoader FromBytes(byte[] imageBytes, bool throwError = false)
        {
            Contract.Requires(!throwError || imageBytes != null);

            if (imageBytes == null)
                return null;

            return FromStream(new MemoryStream(imageBytes), throwError);
        }

        /// <summary>
        /// 从流中创建
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static IUIObjectImageLoader FromStream(Stream stream, bool throwError = false)
        {
            Contract.Requires(!throwError || stream != null);

            if (stream == null)
                return null;

            try
            {
                Icon icon = new Icon(stream);
                return From(icon);
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw;

                LogManager.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 从文件的系统图标创建
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IUIObjectImageLoader FromFileSystemIcon(string filename)
        {
            Contract.Requires(filename != null);

            string ext = Path.GetExtension(filename).ToLower();
            IUIObjectImageLoader imgLoader = _fsIconLoaders.GetOrAddOfRelative(ext, TimeSpan.FromMinutes(10), (ext0) => {
                Icon large = DrawingUtility.GetLargeIcon(filename), small = DrawingUtility.GetSmallIcon(filename);
                if (large == null && small == null)
                    return EmptyUIObjectImageLoader.Instance;

                return new UIObjectImageLoader(new Icon[] { large, small });
            });

            return (imgLoader == EmptyUIObjectImageLoader.Instance) ? null : imgLoader;
        }

        private static Bitmap _ToBitmap(Icon icon)
        {
            if (icon == null)
                return null;

            return icon.ToBitmap();
        }

        private static readonly Cache<string, IUIObjectImageLoader> _fsIconLoaders = new Cache<string, IUIObjectImageLoader>();

        /// <summary>
        /// 从指定的对象中创建，该对象可以是Icon、Image或Stream
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IUIObjectImageLoader From(object obj)
        {
            if (obj is Stream)
                return FromStream((Stream)obj);

            if (obj is Icon)
                return new UIObjectImageLoader((Icon)obj);

            if (obj is Image)
                return new UIObjectImageLoader((Image)obj);

            if (obj is string)
                return FromFileSystemIcon((string)obj);

            if (obj is byte[])
                return FromBytes((byte[])obj);

            return null;
        }
    }
}
