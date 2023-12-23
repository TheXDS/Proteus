using System.Data.SqlClient;
using System.Security;
using System.Windows.Input;
using TheXDS.Ganymede.Helpers;
using TheXDS.Ganymede.Models;
using TheXDS.Ganymede.Types.Base;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Services.Configuration;
using St = TheXDS.Proteus.Resources.Strings.SqlCommon;

namespace TheXDS.Proteus.ViewModels;

internal class SqlServerConfiguratorViewModel : ViewModel, IValidatableViewModel
{
    private string _connectionString = string.Empty;
    private bool _UseSimpleSettings = true;
    private bool _UseFullConnStr;
    private string _Server = string.Empty;
    private string _Database = string.Empty;
    private bool _UseTrustedConn = true;
    private bool _UseAuth;
    private string _Username = string.Empty;
    private SecureString? _Password;

    /// <summary>
    /// Gets or sets the full connection string to use.
    /// </summary>
    public string ConnectionString
    {
        get => _connectionString;
        set => Change(ref _connectionString, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates if the ViewModel should build a
    /// connection string using a set of common values.
    /// </summary>
    public bool UseSimpleSettings
    {
        get => _UseSimpleSettings;
        set => Change(ref _UseSimpleSettings, value);
    }

    /// <summary>
    /// Gets ors ets a value that indicates if the custom connection string
    /// should be used.
    /// </summary>
    public bool UseFullConnStr
    {
        get => _UseFullConnStr;
        set => Change(ref _UseFullConnStr, value);
    }

    /// <summary>
    /// Gets or sets the server to connect to.
    /// </summary>
    public string Server
    {
        get => _Server;
        set => Change(ref _Server, value);
    }

    /// <summary>
    /// Gets or sets the database name to include in the connection string.
    /// </summary>
    public string Database
    {
        get => _Database;
        set => Change(ref _Database, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates of the connection string should use
    /// integrated security (trusted connection) for authentication.
    /// </summary>
    public bool UseTrustedConn
    {
        get => _UseTrustedConn;
        set => Change(ref _UseTrustedConn, value);
    }

    /// <summary>
    /// Gets or sets a value that indicates if the Connection string should use
    /// SQL server authentication.
    /// </summary>
    public bool UseAuth
    {
        get => _UseAuth;
        set => Change(ref _UseAuth, value);
    }

    /// <summary>
    /// Gets or sets the username to be used for SQL server authentication.
    /// </summary>
    public string Username
    {
        get => _Username;
        set => Change(ref _Username, value);
    }

    /// <summary>
    /// Gets or sets the password to be used for SQL server authentication.
    /// </summary>
    public SecureString? Password
    {
        get => _Password;
        set => Change(ref _Password, value);
    }

    /// <summary>
    /// Gets a reference to a command used to test if the supplied SQL connection is valid.
    /// </summary>
    public ICommand TestConnectionCommand { get; }

    /// <summary>
    /// Gets a value that indicates whether or not the settings have been
    /// tested.
    /// </summary>
    public bool? IsConnectionSucessful { get; private set; }

    /// <inheritdoc/>
    public bool IsStateValid => IsConnectionSucessful == true;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SqlServerConfiguratorViewModel"/> class.
    /// </summary>
    public SqlServerConfiguratorViewModel()
    {
        CommandBuilder<SqlServerConfiguratorViewModel> cb = new(this);
        TestConnectionCommand = cb.BuildBusyOperation(OnTestConnection, St.TestingConnection);
    }

    private string GetConnectionString(bool censor = false)
    {
        if (_UseFullConnStr) return censor ? CensorFullConnStr() : ConnectionString;
        return $"Server={Server};Database={Database};{(UseTrustedConn ? "Trusted_Connection=True" : $"User Id={Username};Password={(censor ? "********" : Password?.Read())}")};";
    }

    private string CensorFullConnStr()
    {
        var s = ConnectionString.Split(';');
        for (int j = 0; j < s.Length; j++)
        {
            if (s[j].StartsWith("password=", StringComparison.CurrentCultureIgnoreCase)) { s[j] = "Password=********"; break; }
        }
        return string.Join(";", s);
    }

    private async Task OnTestConnection(IProgress<ProgressReport> progress)
    {
        if (IsFormNotFilled())
        {
            await (DialogService?.Error(St.InvalidConnStr, St.InvalidConnStrMsg) ?? Task.CompletedTask);
            return;
        }
        using var conn = new SqlConnection(GetConnectionString());
        try
        {
            progress.Report(string.Format(St.TryingConn, GetConnectionString(true)));
            await conn.OpenAsync();
            await conn.CloseAsync();
            await (DialogService?.Message(St.SQLConn, St.ConnSuccess) ?? Task.CompletedTask);
            IsConnectionSucessful = true;
        }
        catch (Exception ex)
        {
            await (DialogService?.Error(St.CouldNotConnect, ex.Message) ?? Task.CompletedTask);
            IsConnectionSucessful = false;
        }
        finally
        {
            conn.Dispose();
        }
        Notify(nameof(IsConnectionSucessful), nameof(IsStateValid));
    }

    private bool IsFormNotFilled()
    {
        if (UseFullConnStr) return ConnectionString.IsEmpty();
        var result = !UseSimpleSettings;
        result |= Server.IsEmpty();
        result |= Database.IsEmpty();
        if (UseAuth)
        {
            result |= Username.IsEmpty();
            result |= Password?.Read().IsEmpty() ?? true;
        }
        else
        {
            result |= !UseTrustedConn;
        }
        return result;
    }
}
