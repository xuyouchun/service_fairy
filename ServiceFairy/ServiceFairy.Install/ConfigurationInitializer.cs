using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ServiceFairy.Install
{
    /// <summary>
    /// 配置文件初始化
    /// </summary>
    class ConfigurationInitializer
    {
        public ConfigurationInitializer(InstallerContext ctx)
        {
            _ctx = ctx;
            _filePaths = ctx.FilePaths;
        }

        private readonly InstallerContext _ctx;
        private FilePaths _filePaths;

        public void Execute()
        {
            string path = Path.Combine(_filePaths.TargetPath, "Configuration.xml");
            if (!File.Exists(path))
            {
                RunningConfiguration cfg = RunningConfiguration.CreateDefault();
                cfg.SaveToFile(path);
            }
        }
    }
}
