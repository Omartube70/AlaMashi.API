using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Exceptions;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;
using Microsoft.Extensions.Logging;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    public ErrorHandlingMiddleware(RequestDelegate next , ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex , _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception , ILogger logger)
    {
        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            // 400 Bad Request - يُستخدم عندما تكون البيانات المُرسلة من العميل غير صالحة
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                // يجمع كل أخطاء الـ validation في رسالة واحدة
                message = string.Join(", ", validationException.Errors.Select(e => e.ErrorMessage));
                break;

            // 400 Bad Request - استخدام عام للبيانات غير الصحيحة
            case ArgumentException argumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = argumentException.Message;
                break;

            // 401 Unauthorized - عندما يحاول المستخدم الوصول لشيء يتطلب تسجيل الدخول وهو غير مسجل
            case UnauthorizedAccessException unauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                message = unauthorizedAccessException.Message; // <--- يقرأ الرسالة التي أرسلتها من الـ Controller
                break;

            // 403 Forbidden - عندما يكون المستخدم مسجل دخوله ولكن ليس لديه صلاحية للوصول لهذه الجزئية تحديداً
            case ForbiddenAccessException _:
                statusCode = HttpStatusCode.Forbidden;
                message = "You do not have permission to perform this action.";
                break;

            // 404 Not Found - عند البحث عن عنصر (مثل منتج أو مستخدم) برقم ID غير موجود
            case KeyNotFoundException keyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = keyNotFoundException.Message;
                break;

            // 409 Conflict - عند محاولة إنشاء عنصر موجود بالفعل (مثل مستخدم بنفس البريد الإلكتروني)
            case ConflictException conflictException:
                statusCode = HttpStatusCode.Conflict;
                message = conflictException.Message;
                break;

            // 501 Not Implemented - عندما يكون هناك endpoint لم يتم تنفيذ الكود الخاص به بعد
            case NotImplementedException _:
                statusCode = HttpStatusCode.NotImplemented;
                message = "This feature is not implemented yet.";
                break;

            // 500 Internal Server Error - لأي خطأ آخر غير متوقع يحدث في السيرفر
            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = exception.Message;
                logger.LogError(exception, "An unhandled exception has occurred: {ErrorMessage}", exception.Message);
                break;
        }

        var response = new { statusCode = (int)statusCode, message };
        var jsonResponse = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(jsonResponse);
    }
}
