using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using Common.Contracts.Service;
using Common.Package.Log;

namespace Common.Framework.TrayPlatform
{
    partial class TrayAppServiceManager
    {
        class TrayLogManager : MarshalByRefObjectEx, ITrayLogManager
        {
            public TrayLogManager(TrayAppServiceInfo info, ITrayLogManager innerLogManager)
            {
                _info = info;
                _trayLogManager = innerLogManager;
                _writer = new LogWriterWrapper(((innerLogManager == null) ? EmptyLogWriter<LogItem>.Instance : innerLogManager.Writer) ?? innerLogManager.Writer, info);
                _reader = ((innerLogManager == null) ? EmptyLogReader<LogItem>.Instance : innerLogManager.Reader) ?? innerLogManager.Reader;
            }

            private readonly TrayAppServiceInfo _info;
            private readonly ITrayLogManager _trayLogManager;
            private readonly ILogWriter<LogItem> _writer;
            private readonly ILogReader<LogItem> _reader;

            public ILogWriter<LogItem> Writer
            {
                get { return _writer; }
            }

            public ILogReader<LogItem> Reader
            {
                get { return _reader; }
            }

            class LogWriterWrapper : MarshalByRefObjectEx, ILogWriter<LogItem>
            {
                public LogWriterWrapper(ILogWriter<LogItem> innerLogWriter, TrayAppServiceInfo info)
                {
                    _innerLogWriter = innerLogWriter;
                    _info = info;
                }

                private readonly ILogWriter<LogItem> _innerLogWriter;
                private readonly TrayAppServiceInfo _info;
                private string _source;

                public void Write(IEnumerable<LogItem> items)
                {
                    _innerLogWriter.Write(_Revise(items));
                }

                private IEnumerable<LogItem> _Revise(IEnumerable<LogItem> items)
                {
                    foreach (LogItem logItem in items)
                    {
                        if (string.IsNullOrWhiteSpace(logItem.Source))
                            logItem.Source = (_source ?? (_source = _GetSource()));

                        yield return logItem;
                    }
                }

                private string _GetSource()
                {
                    ServiceDesc sd;
                    if (_info.AppServiceInfo != null && (sd = _info.AppServiceInfo.ServiceDesc) != null)
                        return sd.ToString();

                    return null;
                }
            }
        }
    }
}
