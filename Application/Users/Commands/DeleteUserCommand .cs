using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        // 1. ID المستخدم المراد حذفه (يأتي من الرابط/route)
        public int TargetUserId { get; set; }

        // 2. ID المستخدم الذي قام بالطلب (يأتي من التوكن)
        public int CurrentUserId { get; set; }

        // 3. صلاحيات المستخدم الذي قام بالطلب (تأتي من التوكن)
        public string CurrentUserRole { get; set; }
    }
}
