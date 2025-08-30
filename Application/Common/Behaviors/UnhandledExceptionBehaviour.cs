using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                // اسمح للطلب بالمرور إلى الـ Handler التالي في السلسلة
                return await next();
            }
            catch (Exception ex)
            {
                // في حالة حدوث أي خطأ غير متوقع داخل الـ Handler
                var requestName = typeof(TRequest).Name;

                // قم بتسجيل تفاصيل الخطأ الكاملة لمراجعته لاحقًا
                _logger.LogError(ex, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                // أعد إلقاء الخطأ، سيقوم الـ ErrorHandlingMiddleware بالتقاطه وإرسال رد 500 Internal Server Error
                throw;
            }
        }
    }
}