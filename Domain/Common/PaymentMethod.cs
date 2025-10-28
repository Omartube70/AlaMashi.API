using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public enum PaymentMethod
    {
        Cash = 1,           // نقدي عند الاستلام
        CreditCard = 2,     // بطاقة ائتمان
        DebitCard = 3,      // بطاقة خصم
        Wallet = 4          // محفظة إلكترونية
    }
}