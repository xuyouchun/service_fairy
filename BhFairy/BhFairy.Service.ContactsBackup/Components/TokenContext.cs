using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using ServiceFairy.SystemInvoke;

namespace BhFairy.Service.ContactsBackup.Components
{
    class TokenContext
    {
        public TokenContext(int userId, CallingSettings settings)
        {
            UserId = userId;
            Settings = settings;
        }

        public int UserId { get; private set; }
        public CallingSettings Settings { get; private set; }

        private static readonly Cache<int, TokenContext> _tokenContextCache = new Cache<int, TokenContext>();

        public static TokenContext GetTokenContext(int userId, SystemInvoker invoker)
        {
            return _tokenContextCache.GetOrAddOfDynamic(userId, TimeSpan.FromMinutes(1), (key) =>
                new TokenContext(userId, new FileClient(invoker).CreateCallingSettings(Utility.MakeUserPath(userId)))
            );
        }

        public static void SetTokenContext(int userId, TokenContext tokenContext)
        {
            _tokenContextCache.AddOfDynamic(userId, tokenContext, TimeSpan.FromMinutes(1));
        }
    }
}
