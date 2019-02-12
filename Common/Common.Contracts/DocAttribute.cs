using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Utility;
using System.Reflection;

namespace Common.Contracts
{
    /// <summary>
    /// 用于标注实体类的文档
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DocAttribute : Attribute, IDocProvider
    {
        public virtual int GetRank()
        {
            return Rank;
        }

        public int Rank { get; set; }
    }

    /// <summary>
    /// 提供文档
    /// </summary>
    public interface IDocProvider
    {
        /// <summary>
        /// 优先级
        /// </summary>
        /// <returns></returns>
        int GetRank();
    }

    /// <summary>
    /// 用于读取摘要
    /// </summary>
    public interface IDocSummaryProvider : IDocProvider
    {
        /// <summary>
        /// 读取摘要
        /// </summary>
        /// <returns></returns>
        string GetSummary();
    }

    /// <summary>
    /// 摘要
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class SummaryAttributeBase : DocAttribute, IDocSummaryProvider, IDescProvider
    {
        /// <summary>
        /// 摘要
        /// </summary>
        public abstract string Summary { get; }

        string IDocSummaryProvider.GetSummary()
        {
            return Summary;
        }

        public static string GetSummary(MemberInfo mInfo, bool alsoSearchType = true)
        {
            return mInfo == null ? null : DocUtility.GetSummary(mInfo, alsoSearchType);
        }

        string IDescProvider.GetDesc()
        {
            return ((IDocSummaryProvider)this).GetSummary();
        }
    }

    /// <summary>
    /// 摘要
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class SummaryAttribute : SummaryAttributeBase
    {
        public SummaryAttribute(string summary)
        {
            _summary = summary;
        }

        private readonly string _summary;

        /// <summary>
        /// 摘要
        /// </summary>
        public override string Summary
        {
            get { return _summary; }
        }
    }

    /// <summary>
    /// 读取详细说明
    /// </summary>
    public interface IDocRemarksProvider : IDocProvider
    {
        /// <summary>
        /// 读取详细说明
        /// </summary>
        /// <returns></returns>
        string GetRemarks();
    }

    /// <summary>
    /// 详细说明
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class RemarksAttributeBase : DocAttribute, IDocRemarksProvider
    {
        /// <summary>
        /// 详细说明
        /// </summary>
        public abstract string Remarks { get; }

        string IDocRemarksProvider.GetRemarks()
        {
            return Remarks;
        }

        public static string GetRemarks(MemberInfo mInfo, bool alsoSearchType = true)
        {
            return mInfo == null ? null : DocUtility.GetRemarks(mInfo, alsoSearchType);
        }
    }

    /// <summary>
    /// 详细说明
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class RemarksAttribute : RemarksAttributeBase
    {
        public RemarksAttribute(string remarks)
        {
            _remarks = remarks ?? string.Empty;
        }

        private readonly string _remarks;

        /// <summary>
        /// 详细说明
        /// </summary>
        public override string Remarks
        {
            get { return _remarks; }
        }
    }

    /// <summary>
    /// 读取示例
    /// </summary>
    public interface IDocExampleProvider : IDocProvider
    {
        /// <summary>
        /// 读取示例
        /// </summary>
        string GetExample();
    }

    /// <summary>
    /// 示例
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class ExampleAttributeBase : DocAttribute, IDocExampleProvider
    {
        /// <summary>
        /// 示例
        /// </summary>
        public abstract string Example { get; }

        string IDocExampleProvider.GetExample()
        {
            return Example;
        }

        public static string GetExample(MemberInfo mInfo, bool alsoSearchType = true)
        {
            return mInfo == null ? null : DocUtility.GetExample(mInfo, alsoSearchType);
        }
    }

    /// <summary>
    /// 示例
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ExampleAttribute : ExampleAttributeBase
    {
        public ExampleAttribute(string example)
        {
            _example = example ?? string.Empty;
        }

        private readonly string _example;

        /// <summary>
        /// 示例内容
        /// </summary>
        public override string Example
        {
            get { return _example; }
        }
    }

    /// <summary>
    /// 自定义的标识
    /// </summary>
    public interface IDocCustomFlagProvider : IDocProvider
    {
        /// <summary>
        /// 获取所有的键
        /// </summary>
        /// <returns></returns>
        string[] GetAllKeys();

        /// <summary>
        /// 获取自定义标识
        /// </summary>
        /// <returns></returns>
        string GetFlag(string key);
    }

    /// <summary>
    /// 自定义的标识
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class CustomFlagAttributeBase : DocAttribute, IDocCustomFlagProvider
    {
        /// <summary>
        /// 获取所有的键
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetAllKeys();

        /// <summary>
        /// 获取自定义标识
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract string GetFlag(string key);

        public static IDictionary<string, string> GetCustomFlags(MemberInfo mInfo, bool alsoSearchType = true)
        {
            return mInfo == null ? null : DocUtility.GetCustomFlags(mInfo, alsoSearchType);
        }

        public static CustomFlag[] GetCustomFlagArray(MemberInfo mInfo, bool alsoSearchType = true)
        {
            return mInfo == null ? null : DocUtility.GetCustomFlagArray(mInfo, alsoSearchType);
        }
    }

    /// <summary>
    /// 自定义的标识
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class CustomFlagAttribute : CustomFlagAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flags">以分号与冒号分隔的键值对</param>
        public CustomFlagAttribute(string flags)
        {
            _dict = StringUtility.KeyValuePairsToDictionary(flags);
        }

        private readonly IDictionary<string, string> _dict;

        public override string[] GetAllKeys()
        {
            return _dict.Keys.ToArray();
        }

        public override string GetFlag(string key)
        {
            if (key == null)
                return null;

            return _dict.GetOrDefault(key);
        }
    }

    /// <summary>
    /// 可能取值
    /// </summary>
    [Serializable, DataContract]
    public class PossibleValue
    {
        public PossibleValue(string value, string desc)
        {
            Value = value;
            Desc = desc;
        }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public string Value { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        public override string ToString()
        {
            return Value.ToStringIgnoreNull();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(PossibleValue))
                return false;

            return object.Equals(Value, ((PossibleValue)obj).Value);
        }

        public override int GetHashCode()
        {
            return Value == null ? 0 : Value.GetHashCode();
        }
    }

    /// <summary>
    /// 用于读取可能的取值
    /// </summary>
    public interface IDocPossibleValueProvider : IDocProvider
    {
        /// <summary>
        /// 读取可能的取值
        /// </summary>
        /// <returns></returns>
        PossibleValue[] GetPossibleValues();
    }

    /// <summary>
    /// 可能取值
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class PossibleValuesAttributeBase : DocAttribute, IDocPossibleValueProvider
    {
        /// <summary>
        /// 获取可能的值
        /// </summary>
        /// <returns></returns>
        public abstract PossibleValue[] GetPossibleValues();

        public static PossibleValue[] GetPossableValues(MemberInfo mInfo, bool alsoSearchType = true)
        {
            return mInfo == null ? null : DocUtility.GetPossiableValues(mInfo, alsoSearchType);
        }
    }

    /// <summary>
    /// 可能取值
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class PossibleValuesAttribute : PossibleValuesAttributeBase
    {
        /// <summary>
        /// 以冒号与分号分隔的可能值与描述
        /// </summary>
        /// <param name="possibleValues"></param>
        public PossibleValuesAttribute(string possibleValues)
        {
            _values = new Lazy<PossibleValue[]>(() => {
                return possibleValues.KeyValuePairsToDictionary().ToArray(item => new PossibleValue(item.Key, item.Value));
            });
        }

        /// <summary>
        /// 搜索指定类型中的静态字段
        /// </summary>
        /// <param name="type"></param>
        public PossibleValuesAttribute(Type type)
        {
            _values = new Lazy<PossibleValue[]>(() => {
                List<PossibleValue> lvs = new List<PossibleValue>();
                if (type != null)
                {
                    foreach (FieldInfo fInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
                    {
                        object value = type.IsEnum ? fInfo.Name : fInfo.GetValue(null);
                        string sValue;
                        if (value != null && (sValue=value.ToString())!=null)
                        {
                            lvs.Add(new PossibleValue(sValue, StringUtility.GetFirstNotNullOrEmptyString(fInfo.GetDesc(), DocUtility.GetSummary(fInfo))));
                        }
                    }
                }

                return lvs.ToArray();
            });
        }

        private readonly Lazy<PossibleValue[]> _values;

        public override PossibleValue[] GetPossibleValues()
        {
            return _values.Value;
        }
    }

    /// <summary>
    /// 文档的基类
    /// </summary>
    [Serializable, DataContract]
    public class DocBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 类型短名
        /// </summary>
        [DataMember]
        public string TypeShortName { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        [DataMember]
        public string TypeName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Remarks { get; set; }

        /// <summary>
        /// 示例
        /// </summary>
        [DataMember]
        public string Example { get; set; }

        /// <summary>
        /// 类型的全名
        /// </summary>
        [DataMember]
        public string TypeFullName { get; set; }

        /// <summary>
        /// 自定义标识
        /// </summary>
        [DataMember]
        public CustomFlag[] CustomFlags { get; set; }

        public override string ToString()
        {
            return Name + " <" + TypeShortName + ">";
        }
    }

    /// <summary>
    /// 自定义标识
    /// </summary>
    [Serializable, DataContract]
    public class CustomFlag
    {
        /// <summary>
        /// 键
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// 类型的文档
    /// </summary>
    [Serializable, DataContract]
    public class TypeDoc : DocBase
    {
        /// <summary>
        /// 字段集合
        /// </summary>
        [DataMember]
        public FieldDoc[] FieldDocs { get; set; }
    }

    /// <summary>
    /// 文档树
    /// </summary>
    [Serializable, DataContract]
    public class TypeDocTree
    {
        /// <summary>
        /// 文档主节点
        /// </summary>
        [DataMember]
        public TypeDoc Root { get; set; }

        /// <summary>
        /// 文档子节点
        /// </summary>
        [DataMember]
        public TypeDoc[] SubTypeDocs { get; set; }
    }

    /// <summary>
    /// 字段的文档
    /// </summary>
    [Serializable, DataContract]
    public class FieldDoc : DocBase
    {
        /// <summary>
        /// 可能的值
        /// </summary>
        [DataMember]
        public PossibleValue[] PossibleValues { get; set; }
    }
}
