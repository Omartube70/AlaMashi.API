using MediatR;

namespace Application.Users.Events
{
    public class UserCreatedNotification : INotification
    {
        public string UserEmail { get; }
        public string UserName { get; }

        public UserCreatedNotification(string userEmail, string userName)
        {
            UserEmail = userEmail;
            UserName = userName;
        }
    }
}