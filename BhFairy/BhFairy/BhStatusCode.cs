using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace BhFairy
{
    public enum BhStatusCode
    {
        BusinessError = ServiceStatusCode.BusinessError,

        NameCardSharing = BusinessError | (1024 << 28),

        ContactsBackup = BusinessError | (1025 << 28),
    }
}
