using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class UpdateUserCommand : IRequest
    {
        // --- بيانات لتحديد الهدف والصلاحيات ---
        public int TargetUserId { get; set; }      // ID المستخدم المراد تحديثه
        public int CurrentUserId { get; set; }     // ID المستخدم الذي يقوم بالطلب
        public string CurrentUserRole { get; set; } // صلاحيات المستخدم الحالي

        // --- البيانات الجديدة ---
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
    }
}
