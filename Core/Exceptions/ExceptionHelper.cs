using Core.DTOs;
using FluentValidation;

namespace Core.Exceptions;

public class ExceptionHelper
{
    public static ExceptionDetail GetExceptionDetail(Exception exception)
    {
        var status = 500;

        var type = exception.GetType().Name;

        var detail = exception.Message;

        List<string> errors = new List<string>();

        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.Select(x => x.ErrorMessage).ToList();
        }
        else
        {
            var innerException = exception.InnerException;
            while (innerException != null)
            {
                errors.Add(innerException.Message);
                innerException = innerException.InnerException;
            }
        }

        return new ExceptionDetail
        {
            Code = status,
            type = type,
            Succeeded = false,
            Message = detail,
            Errors = errors
        };
    }
}