using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nordril.Functional.Results
{
    /// <summary>
    /// Indicates the general class of the result. Roughly equivalent to HTTP status codes.
    /// </summary>
    public enum ResultClass
    {
        /// <summary>
        /// The resource was already present and couldn't be inserted. Roughly equivalent to HTTP 409 (Conflict).
        /// </summary>
        AlreadyPresent,

        /// <summary>
        /// The request cannot be fulfilled because it is syntactically invalid.
        /// </summary>
        BadRequest,

        /// <summary>
        /// There was a conflict in the requested data, such as when multiple results were found but only one was expected. Equivalent to HTTP 409 (Conflict).
        /// </summary>
        DataConflict,

        /// <summary>
        /// There was a conflict with another request when trying to update the resource. Equivalent to HTTP 409 (Conflict).
        /// </summary>
        EditConflict,

        /// <summary>
        /// The request was not allowed because the user wasn't authorized to perform it. Equivalent to HTTP 403.
        /// </summary>
        Forbidden,

        /// <summary>
        /// There was an internal exception. Equivalent to HTTP 500.
        /// </summary>
        InternalException,

        /// <summary>
        /// The request was successful and there were no errors.
        /// </summary>
        Ok,

        /// <summary>
        /// The requested resource was not found. Equivalent to HTTP 404.
        /// </summary>
        NotFound,

        /// <summary>
        /// The method has not been implemented. Equivalent to HTTP 501 (Not implemented).
        /// </summary>
        NotImplemented,

        /// <summary>
        /// The resource is gone. Equivalent to HTTP 410 (Gone) and similar to HTTP 404.
        /// </summary>
        ResourceGone,

        /// <summary>
        /// The request couldn't be processed because it was semantically invalid in a way not described by other <see cref="ResultClass"/>-members. This is the default <see cref="ResultClass"/> for input error.
        /// </summary>
        UnprocessableEntity,

        /// <summary>
        /// The request was cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// Unspecified error result, indicating that there was an error, but that we were not able to specify which kind. Should be avoided.
        /// </summary>
        Unspecified,
    }

    /// <summary>
    /// Extension methods for <see cref="ResultClass"/>.
    /// </summary>
    public static class ResultClassExtensions
    {
        /// <summary>
        /// Converts a <see cref="ResultClass"/> to the nearest approximate HTTP status code.
        /// </summary>
        /// <param name="r">The result class to convert.</param>
        public static HttpStatusCode ToHttpStatusCode(this ResultClass r)
        {
            switch (r)
            {
                case ResultClass.AlreadyPresent:
                    return HttpStatusCode.BadRequest;
                case ResultClass.BadRequest:
                    return HttpStatusCode.BadRequest;
                case ResultClass.DataConflict:
                    return HttpStatusCode.Conflict;
                case ResultClass.EditConflict:
                    return HttpStatusCode.Conflict;
                case ResultClass.Forbidden:
                    return HttpStatusCode.Forbidden;
                case ResultClass.InternalException:
                    return HttpStatusCode.InternalServerError;
                case ResultClass.NotFound:
                    return HttpStatusCode.NotFound;
                case ResultClass.NotImplemented:
                    return HttpStatusCode.NotImplemented;
                case ResultClass.Ok:
                    return HttpStatusCode.OK;
                case ResultClass.ResourceGone:
                    return HttpStatusCode.Gone;
#if NETCORE
                case ResultClass.UnprocessableEntity:
                    return HttpStatusCode.UnprocessableEntity;
#endif
                case ResultClass.Cancelled:
                    return HttpStatusCode.ServiceUnavailable;
                case ResultClass.Unspecified:
                    return HttpStatusCode.InternalServerError;
                default:
                    throw new ArgumentException($"Unknown {nameof(ResultClass)} value {r}.", nameof(r));
            }
        }
    }
}
