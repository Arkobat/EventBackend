namespace Event.Model;

public enum HttpStatus
{
    Ok = 200,
    Created = 201,

    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,

    InternalError = 500,
    NotImplemented = 501,
}