﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common
{
    /// <summary>
    /// 读写锁
    /// </summary>
    public class RwLocker
    {
        public RwLocker()
        {
            _ReadLocker = new ReadLocker(_Locker);
            _WriteLocker = new WriteLocker(_Locker);
            _UpgradeLocker = new UpgradeLocker(_Locker);
        }

        private readonly ReaderWriterLockSlim _Locker = new ReaderWriterLockSlim();

        /// <summary>
        /// 进入读模式
        /// </summary>
        /// <returns></returns>
        public IDisposable Read()
        {
            _Locker.EnterReadLock();
            return _ReadLocker;
        }

        private readonly IDisposable _ReadLocker, _WriteLocker, _UpgradeLocker;

        /// <summary>
        /// 尝试进入读模式
        /// </summary>
        /// <param name="stub"></param>
        /// <param name="millsecondsTimeout"></param>
        /// <returns></returns>
        public bool TryRead(out IDisposable stub, int millsecondsTimeout = 0)
        {
            if (_Locker.TryEnterReadLock(millsecondsTimeout))
            {
                stub = _ReadLocker;
                return true;
            }

            stub = null;
            return false;
        }

        /// <summary>
        /// 进入写模式
        /// </summary>
        /// <returns></returns>
        public IDisposable Write()
        {
            _Locker.EnterWriteLock();
            return _WriteLocker;
        }

        /// <summary>
        /// 尝试进入写模式
        /// </summary>
        /// <param name="stub"></param>
        /// <param name="millsecondsTimeout"></param>
        /// <returns></returns>
        public bool TryWrite(out IDisposable stub, int millsecondsTimeout = 0)
        {
            if (_Locker.TryEnterWriteLock(millsecondsTimeout))
            {
                stub = _WriteLocker;
                return true;
            }

            stub = null;
            return false;
        }

        /// <summary>
        /// 从读模式升级到写模式
        /// </summary>
        /// <returns></returns>
        public IDisposable Upgrade()
        {
            _Locker.EnterUpgradeableReadLock();
            return _UpgradeLocker;
        }

        /// <summary>
        /// 尝试从读模式升级到写模式
        /// </summary>
        /// <param name="stub"></param>
        /// <param name="millsecondsTimeout"></param>
        /// <returns></returns>
        public bool TryUpgrade(out IDisposable stub, int millsecondsTimeout = 0)
        {
            if (_Locker.TryEnterUpgradeableReadLock(millsecondsTimeout))
            {
                stub = _UpgradeLocker;
                return true;
            }

            stub = null;
            return false;
        }

        #region Class ReadLocker ...

        class ReadLocker : IDisposable
        {
            public ReadLocker(ReaderWriterLockSlim locker)
            {
                _Locker = locker;
            }

            private readonly ReaderWriterLockSlim _Locker;

            #region IDisposable Members

            public void Dispose()
            {
                _Locker.ExitReadLock();
            }

            #endregion
        }

        #endregion

        #region Class WriteLocker ...

        class WriteLocker : IDisposable
        {
            public WriteLocker(ReaderWriterLockSlim locker)
            {
                _Locker = locker;
            }

            private readonly ReaderWriterLockSlim _Locker;

            #region IDisposable Members

            public void Dispose()
            {
                _Locker.ExitWriteLock();
            }

            #endregion
        }

        #endregion

        #region Class UpgradeLocker ...

        class UpgradeLocker : IDisposable
        {
            public UpgradeLocker(ReaderWriterLockSlim locker)
            {
                _Locker = locker;
            }

            private readonly ReaderWriterLockSlim _Locker;

            #region IDisposable Members

            public void Dispose()
            {
                _Locker.ExitUpgradeableReadLock();
            }

            #endregion
        }

        #endregion

        #region Class EmptyLocker ...

        class EmptyLocker : IDisposable
        {
            public void Dispose()
            {
                
            }
        }

        #endregion

        public static readonly IDisposable EmptyStub = new EmptyLocker();

    }
}
