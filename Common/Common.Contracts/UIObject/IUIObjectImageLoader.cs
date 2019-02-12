using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common.Contracts.Service;
using Common.Collection;
using Common.Utility;
using System.Diagnostics.Contracts;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.Serialization;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// UIObject图像
    /// </summary>
    [Serializable]
    public class UIObjectImage : ISerializable, IDeserializationCallback
    {
        public UIObjectImage(Size size, byte[] buffer)
        {
            Contract.Requires(buffer != null);

            Size = size;
            _buffer = buffer;
        }

        public UIObjectImage(Image image)
        {
            Contract.Requires(image != null);

            _image = image;
        }

        /// <summary>
        /// 尺寸
        /// </summary>
        public Size Size { get; private set; }

        private byte[] _buffer;

        [NonSerialized]
        private Image _image;

        public Image ToImage()
        {
            return (_image ?? (_image = (_buffer == null ? null : Image.FromStream(new MemoryStream(_buffer)))));
        }

        public static explicit operator Image(UIObjectImage img)
        {
            if (img == null)
                return null;

            return img.ToImage();
        }

        public static explicit operator UIObjectImage(Image img)
        {
            if (img == null)
                return null;

            return new UIObjectImage(img);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (_buffer == null)
            {
                if (_image != null)
                {
                    Size = _image.Size;
                    MemoryStream ms = new MemoryStream();
                    _image.Save(ms, ImageFormat.Png);

                    _buffer = ms.ToArray();
                }
            }

            info.AddValue("buffer", _buffer, typeof(byte[]));
        }

        protected UIObjectImage(SerializationInfo info, StreamingContext context)
        {
            _buffer = (byte[])info.GetValue("buffer", typeof(byte[]));
        }

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            return;
        }
    }

    /// <summary>
    /// 用于提供各种尺寸的图片
    /// </summary>
    public interface IUIObjectImageLoader
    {
        /// <summary>
        /// 获取指定尺寸的图片
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        UIObjectImage GetImage(Size size);
    }

    /// <summary>
    /// 空的图片读取器
    /// </summary>
    public class EmptyUIObjectImageLoader : MarshalByRefObjectEx, IUIObjectImageLoader
    {
        public UIObjectImage GetImage(Size size)
        {
            return null;
        }

        public static readonly EmptyUIObjectImageLoader Instance = new EmptyUIObjectImageLoader();
    }

    /// <summary>
    /// 具有图标的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UIObjectImageLoaderAdapter<T> : IUIObjectImageLoader
    {
        public UIObjectImageLoaderAdapter(T obj, IUIObjectImageLoader imageLoader)
        {
            Obj = obj;
            _imageLoader = imageLoader ?? EmptyUIObjectImageLoader.Instance;
        }

        public T Obj { get; private set; }

        private readonly IUIObjectImageLoader _imageLoader;

        public UIObjectImage GetImage(Size size)
        {
            return _imageLoader.GetImage(size);
        }

        public override string ToString()
        {
            return Obj.ToStringIgnoreNull();
        }

        public static UIObjectImageLoaderAdapter<T>[] Convert(IEnumerable<T> items, IUIObjectImageLoader imageLoader)
        {
            Contract.Requires(items != null);

            return items.ToArray(item => new UIObjectImageLoaderAdapter<T>(item, imageLoader));
        }
    }

    /// <summary>
    /// 透明的图片读取器
    /// </summary>
    public class TransparentUIObjectImageLoader : MarshalByRefObjectEx, IUIObjectImageLoader
    {
        private static readonly ThreadSafeDictionaryWrapper<Size, UIObjectImage> _dict = new ThreadSafeDictionaryWrapper<Size, UIObjectImage>();

        public UIObjectImage GetImage(Size size)
        {
            return _dict.GetOrSet(size, _GetImage);
        }

        private UIObjectImage _GetImage(Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            bmp.MakeTransparent(Color.White);
            return new UIObjectImage(bmp);
        }

        public static readonly TransparentUIObjectImageLoader Instance = new TransparentUIObjectImageLoader();
    }

    /// <summary>
    /// 图片读取器的集合
    /// </summary>
    public class UIObjectImageLoaderCollection : MarshalByRefObjectEx, IUIObjectImageLoader
    {
        public UIObjectImageLoaderCollection(IUIObjectImageLoader[] imageLoaders)
        {
            _imageLoaders = imageLoaders ?? new IUIObjectImageLoader[0];
        }

        private readonly IUIObjectImageLoader[] _imageLoaders;

        public UIObjectImage GetImage(Size size)
        {
            for (int k = 0, length = _imageLoaders.Length; k < length; k++)
            {
                UIObjectImage img = _imageLoaders[k].GetImage(size);
                if (img != null)
                    return img;
            }

            return null;
        }
    }
}
