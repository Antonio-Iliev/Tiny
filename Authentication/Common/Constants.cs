namespace Authentication.Common;

internal class Constants
{
    internal const string UsernameValidationCriteria = "^[a-zA-Z0-9]{3,20}$";

    internal const string PasswordValidationCriteria =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{3,20}$";
}