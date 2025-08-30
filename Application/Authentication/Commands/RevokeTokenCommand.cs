using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Commands
{

    public class RevokeTokenCommand : IRequest
    {
        // ID المستخدم المراد إلغاء التوكن الخاص به
        public int TargetUserId { get; set; }

        // بيانات المستخدم الذي يقوم بالطلب (من التوكن)
        public int CurrentUserId { get; set; }
        public string CurrentUserRole { get; set; }
    }
}
