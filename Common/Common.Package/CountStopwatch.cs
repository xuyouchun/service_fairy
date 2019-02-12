using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Common.Package
{
    /// <summary>
    /// 计数器
    /// </summary>
    public class CountStopwatch : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxTimes"></param>
        public CountStopwatch(int maxTimes)
        {
            _maxTimes = (maxTimes < 1) ? 1 : maxTimes;

            _Register(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CountStopwatch()
            : this(30)
        {

        }

        /// <summary>
        /// 增加计数
        /// </summary>
        public void Increment()
        {
            if (!Running)
                throw new InvalidOperationException("性能计数器未运行");

            Interlocked.Increment(ref _counter.Count);
            Interlocked.Increment(ref _totalAll);
        }

        /// <summary>
        /// 增加计数
        /// </summary>
        /// <param name="count"></param>
        public void Increment(int count)
        {
            if (!Running)
                throw new InvalidOperationException("性能计数器未运行");

            Interlocked.Add(ref _counter.Count, count);
            Interlocked.Add(ref _totalAll, count);
        }

        private int _maxTimes;
        private Counter _counter;
        private readonly Queue<Counter> _queue = new Queue<Counter>();
        private readonly object _syncLocker = new object();
        private DateTime _now;
        private volatile bool _running;
        private long _total, _totalAll;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool Running
        {
            get { return _running; }
            private set { _running = value; }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (Running)
                return;

            lock (_syncLocker)
            {
                if (Running)
                    return;

                lock (_queue)
                {
                    _queue.Clear();
                    _counter = new Counter();
                    _queue.Enqueue(_counter);
                    _total = 0;
                    _totalAll = 0;
                }

                Running = true;
            }
        }

        /// <summary>
        /// 重新启动
        /// </summary>
        public void Restart()
        {
            lock (_syncLocker)
            {
                Stop();
                Start();
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (!Running)
                return;

            lock (_syncLocker)
            {
                if (!Running)
                    return;

                Running = false;
                _now = DateTime.UtcNow;
            }
        }

        class Counter
        {
            public DateTime Time = DateTime.UtcNow;
            public long Count;
        }

        /// <summary>
        /// 获取当前的计数与时间
        /// </summary>
        /// <param name="count"></param>
        /// <param name="timespan"></param>
        public bool GetCurrent(out long count, out TimeSpan timespan)
        {
            if (Running)
                _now = DateTime.UtcNow;

            lock (_queue)
            {
                if (_queue.Count == 0)
                    goto _end;

                count = _total + Interlocked.Read(ref _counter.Count);
                timespan = _now - _queue.Peek().Time;
                return true;
            }

        _end:
            count = 0;
            timespan = default(TimeSpan);
            return false;
        }

        /// <summary>
        /// 获取每秒的平均计数
        /// </summary>
        /// <returns></returns>
        public double GetPreSeconds()
        {
            long count;
            TimeSpan timespan;

            if (!GetCurrent(out count, out timespan) || timespan == TimeSpan.Zero)
                return 0;

            return count / timespan.TotalSeconds;
        }

        /// <summary>
        /// 获取当前的计数
        /// </summary>
        /// <returns></returns>
        public long TotalCount
        {
            get
            {
                return Interlocked.Read(ref _totalAll);
            }
        }

        /// <summary>
        /// 自动显示当前的统计信息
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="waitForExit"></param>
        /// <param name="shower"></param>
        public void AutoTrace(TimeSpan interval, WaitHandle waitForExit = null, Action<string> shower = null)
        {
            if (waitForExit == null)
                waitForExit = new ManualResetEvent(false);

            while (true)
            {
                if (waitForExit.WaitOne(interval))
                    return;

                string msg = ToString();
                if (shower != null)
                    shower(msg);
                else
                    Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// 自动显示当前的统计信息
        /// </summary>
        /// <param name="intervalMillseconds"></param>
        /// <param name="waitForExit"></param>
        /// <param name="shower"></param>
        public void AutoTrace(int intervalMillseconds = 1000, WaitHandle waitForExit = null, Action<string> shower = null)
        {
            AutoTrace(TimeSpan.FromMilliseconds(intervalMillseconds), waitForExit, shower);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _Unregister(this);
        }

        ~CountStopwatch()
        {
            Dispose();
        }

        /// <summary>
        /// 启动新的计数器
        /// </summary>
        /// <param name="maxTimes"></param>
        /// <returns></returns>
        public static CountStopwatch StartNew(int maxTimes)
        {
            CountStopwatch pc = new CountStopwatch(maxTimes);
            pc.Start();
            return pc;
        }

        /// <summary>
        /// 启动新的计数器
        /// </summary>
        /// <returns></returns>
        public static CountStopwatch StartNew()
        {
            return StartNew(30);
        }

        static CountStopwatch()
        {
            GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(1), TimeSpan.Zero, _Compute, false);
        }

        private static void _Register(CountStopwatch obj)
        {
            lock (_hs)
            {
                _hs.Add(obj);
            }
        }

        private static void _Unregister(CountStopwatch obj)
        {
            lock (_hs)
            {
                _hs.Remove(obj);
            }
        }

        private static readonly HashSet<CountStopwatch> _hs = new HashSet<CountStopwatch>();

        private static void _Compute()
        {
            if (_hs.Count == 0)
                return;

            CountStopwatch[] counters;
            lock (_hs)
            {
                counters = _hs.ToArray();
            }

            for (int k = 0; k < counters.Length; k++)
            {
                counters[k]._PrivateCompute();
            }
        }

        private void _PrivateCompute()
        {
            if (!Running)
                return;

            lock (_queue)
            {
                Counter newCounter = new Counter();
                Interlocked.Exchange<Counter>(ref _counter, newCounter);

                while (_queue.Count > _maxTimes - 1)
                {
                    _queue.Dequeue();
                }

                _total = _queue.Sum(q => q.Count);
                _queue.Enqueue(newCounter);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}/s (total:{1}) ", Math.Round(GetPreSeconds(), 2), TotalCount);
        }
    }
}
