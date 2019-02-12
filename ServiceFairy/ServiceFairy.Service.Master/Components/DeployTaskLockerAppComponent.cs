using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Master.Components
{
    /// <summary>
    /// 部署操作锁，用于防止多个部署任务同时运行时引起的混乱
    /// </summary>
    [AppComponent("部署操作锁", "防止多个部署任务同时运行时引起的混乱")]
    class DeployTaskLockerAppComponent : AppComponent
    {
        public DeployTaskLockerAppComponent(Service service)
            : base(service)
        {

        }

        private readonly object _thisLock = new object();

        private readonly Dictionary<object, LockerInfo> _lockers = new Dictionary<object, LockerInfo>();
        private volatile bool _hasGlobalMonopoly = false;

        class LockerInfo
        {
            public object Owner;
            public DeployLockerType LockerType;
            public string Reason;
            public int LockTimes;
        }

        /// <summary>
        /// 获取对象所持有的锁
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        private LockerInfo _GetLockerInfo(object owner)
        {
            return _lockers.GetOrDefault(owner);
        }

        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="reason"></param>
        /// <param name="lockerType"></param>
        public void Lock(object owner, DeployLockerType lockerType, string reason)
        {
            Contract.Requires(owner != null);

            string errorMsg;
            if (!_TryLock(owner, lockerType, reason, out errorMsg))
                throw new ServiceException(ServerErrorCode.ServiceBusy, errorMsg);
        }

        private bool _TryLock(object owner, DeployLockerType lockerType, string reason, out string errorMsg)
        {
            errorMsg = null;

            lock (_thisLock)
            {
                if (_hasGlobalMonopoly)
                {
                    errorMsg = "当前有正在运行的部署任务：" + _lockers.Values.Select(locker => locker.Reason).JoinBy(",");
                    return false;
                }

                if (lockerType == DeployLockerType.GlobalMonopoly)
                {
                    if (_lockers.Count > 0)
                    {
                        errorMsg = "当前有正在运行的部署任务：" + _lockers.Values.Select(locker => locker.Reason).JoinBy(",");
                        return false;
                    }

                    _hasGlobalMonopoly = true;
                }

                LockerInfo lockerInfo = _GetLockerInfo(owner);
                if (lockerInfo == null)
                {
                    _lockers.Add(owner, new LockerInfo() { Owner = owner, LockerType = lockerType, Reason = reason, LockTimes = 1 });
                    errorMsg = null;
                    return true;
                }
                else
                {
                    if (lockerInfo.LockerType == DeployLockerType.Monopoly)
                    {
                        errorMsg = "该任务当前正在运行";
                        return false;
                    }

                    lockerInfo.LockTimes++;
                    return true;
                }
            }
        }

        /// <summary>
        /// 尝试加锁
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="lockerType"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public bool TryLock(object owner, DeployLockerType lockerType, string reason)
        {
            Contract.Requires(owner != null);

            string errMsg;
            return _TryLock(owner, lockerType, reason, out errMsg);
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="owner"></param>
        public bool Unlock(object owner)
        {
            Contract.Requires(owner != null);

            lock (_thisLock)
            {
                LockerInfo lockerInfo = _GetLockerInfo(owner);
                if (lockerInfo == null)
                    return false;

                if (--lockerInfo.LockTimes <= 0)
                    _lockers.Remove(owner);

                if (lockerInfo.LockerType == DeployLockerType.GlobalMonopoly)  // 解除全局独占锁
                    _hasGlobalMonopoly = false;

                return true;
            }
        }
    }

    /// <summary>
    /// 锁类型
    /// </summary>
    enum DeployLockerType
    {
        /// <summary>
        /// 共享锁，允许其它的部署任务运行
        /// </summary>
        [Desc("共享锁")]
        Share,

        /// <summary>
        /// 独占锁，不允许其它的同类任务运行
        /// </summary>
        [Desc("独占锁")]
        Monopoly,

        /// <summary>
        /// 全局独占锁，不允许其它的任何任务运行
        /// </summary>
        [Desc("全局独占锁")]
        GlobalMonopoly,
    }
}
