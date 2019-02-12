using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace Common.Data.UnionTable.Metadata
{
    /// <summary>
    /// 元数据实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class MtBase
    {
        public MtBase(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        [IgnoreDataMember]
        public IDictionary Tag
        {
            get { return _tag; }
        }

        [IgnoreDataMember, NonSerialized]
        private IDictionary _tag = new Hashtable();
    }

    /// <summary>
    /// 元数据实体的基类
    /// </summary>
    [Serializable, DataContract]
    public class MtBase<TOwner> : MtBase where TOwner : MtBase
    {
        public MtBase(string name, string desc)
            : base(name, desc)
        {

        }

        /// <summary>
        /// 拥有者
        /// </summary>
        [DataMember]
        public TOwner Owner { get; internal set; }
    }
}
