using System.Net;

namespace Shared.Api.Exceptions;

public sealed record ExceptionResponse(object Response, HttpStatusCode StatusCode);


