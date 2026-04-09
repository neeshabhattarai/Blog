namespace Blog.Infastructure.Service;

public static class Policy
{
    public const string IsAdmin = "IsAdmin";
    public const string IsUser = "IsUser";
    public const string IsManager = "IsManager";
    public const string IsAuthor = "IsAuthor";
    public const string IsAdminOrUser = "IsAdminOrUser";
}