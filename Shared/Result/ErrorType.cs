namespace Shared.Result
{
    public enum ErrorType
    {
        //دي انواع الايرور اللي ممكن تقابلني في
        //Expected Error in App
        Failure=0,
        Validation=1,
        NotFound=2,
        Unauthorized=3,
        Forbidden=4,
        InvalidCredentials=5,

    }
}