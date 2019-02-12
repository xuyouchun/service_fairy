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
    /// 文件写入事务
    /// </summary>
    class WriteFileToken : FileToken
    {
        protected override UnionFileStream OnCreateStream()
        {
            UnionFileStream stream = UnionFile.Open(UnionFileOpenModel.Write);
            return Service.FileCryptoExecutor.CreateEncryptUnionFileStreamAdapter(stream, SecurityUtility.Md5(Path.ToUpper()));
        }
    }
}
