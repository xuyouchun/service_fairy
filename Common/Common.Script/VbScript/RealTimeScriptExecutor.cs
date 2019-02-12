using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 实时代码执行器
    /// </summary>
    public class RealTimeScriptExecutor : ScriptExecutor
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="interval">周期</param>
        /// <param name="runAtOnce">是否立即运行</param>
        public RealTimeScriptExecutor(TimeSpan interval, bool runAtOnce)
        {
            Interval = interval;
            _Timer.Elapsed += new ElapsedEventHandler(_Timer_Elapsed);

            if (runAtOnce)
                Resume();
        }

        volatile bool _Disposed = false;
        readonly object _SyncLocker = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="interval">周期</param>
        public RealTimeScriptExecutor(TimeSpan interval)
            : this(interval, true)
        {

        }

        void _Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _Event.Set();
        }

        /// <summary>
        /// 周期
        /// </summary>
        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_Timer.Interval); }
            set { _Timer.Interval = value.TotalMilliseconds; }
        }

        /// <summary>
        /// 暂停执行
        /// </summary>
        public void Pause()
        {
            lock (_SyncLocker)
            {
                if (!_Disposed)
                {
                    _Timer.Stop();
                    _Event.Reset();
                }
            }
        }

        /// <summary>
        /// 继续执行
        /// </summary>
        public void Resume()
        {
            lock (_SyncLocker)
            {
                if (!_Disposed)
                {
                    _Timer.Start();
                }
            }
        }

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool Running { get { return _Timer.Enabled; } }

        private readonly Timer _Timer = new Timer();

        private readonly System.Threading.AutoResetEvent _Event = new System.Threading.AutoResetEvent(false);

        internal protected override bool OnBeforeExecuteStatement(Statement statement)
        {
            _Event.WaitOne();

            return base.OnBeforeExecuteStatement(statement);
        }

        public override void Dispose()
        {
            lock (_SyncLocker)
            {
                if (!_Disposed)
                {
                    _Disposed = true;
                    _Timer.Dispose();
                }
            }
        }
    }
}
