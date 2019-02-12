using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;

namespace ServiceFairy.Deploy
{
    /// <summary>
    /// 部署方式验证
    /// </summary>
    public interface IDeployValidator
    {
        /// <summary>
        /// 验证是否为有效部署
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        DeployValidatorResult Validate(AppClientDeployMap map);
    }

    /// <summary>
    /// 部署方式验证结果
    /// </summary>
    public class DeployValidatorResult
    {
        public DeployValidatorResult(IEnumerable<DeployValidatorResultItem> items)
        {
            Items = items.ToArray();
        }

        /// <summary>
        /// 所有的验证结果
        /// </summary>
        public DeployValidatorResultItem[] Items { get; set; }

        /// <summary>
        /// 是否全部成功
        /// </summary>
        /// <returns></returns>
        public bool AllSucceed()
        {
            return Items.All(item => item.Avaliable);
        }

        /// <summary>
        /// 获取全部成功的验证结果
        /// </summary>
        /// <returns></returns>
        public DeployValidatorResultItem[] GetAllSucceedItems()
        {
            return Items.Where(item => item.Avaliable).ToArray();
        }

        /// <summary>
        /// 获取全部失败的验证结果
        /// </summary>
        /// <returns></returns>
        public DeployValidatorResultItem[] GetAllFieldItems()
        {
            return Items.Where(item => !item.Avaliable).ToArray();
        }
    }

    /// <summary>
    /// 部署方式验证结果的每一项
    /// </summary>
    public class DeployValidatorResultItem
    {
        public DeployValidatorResultItem(bool avaliable, AppClientDeployInfo deployInfo, string message = "")
        {
            Avaliable = avaliable;
            Message = message;
            DeployInfo = deployInfo;
        }

        /// <summary>
        /// 是否为有效部署
        /// </summary>
        public bool Avaliable { get; private set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 出现错误的终端
        /// </summary>
        public AppClientDeployInfo DeployInfo { get; private set; }
    }
}
