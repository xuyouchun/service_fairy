using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common.Contracts.Entities
{
    /// <summary>
    /// 坐标
    /// </summary>
    [Serializable, DataContract]
    public class GeoLocation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="desc">描述</param>
        public GeoLocation(float longitude, float latitude, string desc = "")
        {
            Longitude = longitude;
            Latitude = latitude;
            Desc = desc;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public float Longitude { get; private set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public float Latitude { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        /// <summary>
        /// 原点
        /// </summary>
        public static readonly GeoLocation Zero = new GeoLocation(0, 0, "");

        /// <summary>
        /// 地球周长
        /// </summary>
        public const int EarthCircle = 40075700;
    }
}
