namespace Authentication.Common.Exceptions;

public class UserNameDuplicationException(string message) : Exception(message);