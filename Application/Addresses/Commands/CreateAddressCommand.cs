using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using MediatR;
using Application.Addresses.Dtos;

namespace Application.Addresses.Commands
{
    public class CreateAddressCommand : IRequest<AddressDto>
    {
        /// 🔒 ID المستخدم الحالي الذي يقوم بالطلب.
        /// يتم الحصول عليه من التوكن في الـ Controller لضمان الأمان.
        public int CurrentUserId { get; set; }

        /// اسم الشارع من الطلب.
        public string Street { get; set; }

        /// اسم المدينة من الطلب.
        public string City { get; set; }

        /// تفاصيل إضافية اختيارية للعنوان.
        public string? AddressDetails { get; set; }

        /// نوع العنوان (مثل: منزل أو عمل).
        public AddressType AddressType { get; set; }
    }
}
