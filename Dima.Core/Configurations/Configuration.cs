namespace Dima.Core.Configurations;

public static class Configuration
{
    #region Page

    public const int PageNumber = 1;
    public const int DefaultPageSize = 25;

    #endregion


    #region Status Code

    public const int DefaultCode = 200;
    public const int DefaultCodeLast = 299;

    #endregion

    #region Configurations

    public static string ConnectionString { get; set; } = string.Empty;
    public static string FrontendUrl { get; set; } = string.Empty;
    public static string BackendUrl { get; set; } = string.Empty;

    #endregion

}