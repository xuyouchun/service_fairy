using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.File.UnionFile;
using Common.Framework.TrayPlatform;
using Common.Utility;

namespace ServiceFairy.Service.File.Components
{
    /// <summary>
    /// 文件读取事务
    /// </summary>
    class ReadFileToken : FileToken
    {
        protected override UnionFileStream OnCreateStream()
        {
            UnionFileStream stream = UnionFile.Open(UnionFileOpenModel.Read);
            return Service.FileCryptoExecutor.CreateDecryptUnionFileStreamAdapter(stream, SecurityUtility.Md5(Path.ToUpper()));
        }
    }
}
