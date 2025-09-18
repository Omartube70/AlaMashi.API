using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public enum OrderStatus
    {
        /// <summary>
        /// تم تقديم الطلب وينتظر التأكيد أو البدء في التحضير.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// تم تأكيد الطلب وهو قيد التحضير.
        /// </summary>
        InPreparation = 2,

        /// <summary>
        /// الطلب جاهز ومع شركة التوصيل في طريقه للعميل.
        /// </summary>
        OutForDelivery = 3,

        /// <summary>
        /// تم توصيل الطلب بنجاح للعميل.
        /// </summary>
        Delivered = 4,

        /// <summary>
        /// تم إلغاء الطلب من قبل العميل أو المسؤول.
        /// </summary>
        Canceled = 5
    }
}
