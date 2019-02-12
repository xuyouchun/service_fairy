﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Package.Cache
{
    /// <summary>
    /// 缓存值的加载器
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public interface ICacheValueLoader<in TKey, out TValue>
        where TValue : class
    {
        /// <summary>
        /// 获取同步锁
        /// </summary>
        /// <param name="key"></param>
        object GetSyncLocker(TKey key);

        /// <summary>
        /// 根据缓存键加载缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        /// <remarks>如果返回null则表示要删除该缓存项</remarks>
        TValue Load(TKey key);
    }
}
