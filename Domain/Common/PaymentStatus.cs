using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public enum PaymentStatus
    {
        Pending = 1,        // قيد الانتظار
        Completed = 2,      // مكتمل
        Failed = 3,         // فشل
        Canceled = 4        // ملغي
    }
}