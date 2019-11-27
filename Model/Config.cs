using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
public sealed class Config
{
    public IConfigurationRoot configurationManager = null;
    private static readonly Config instance = new Config();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static Config()
    {
    }

    private Config()
    {
    }

    public static Config Instance
    {
        get
        {
            return instance;
        }
    }

    public IDbConnection getDatabaseConnection() {
        if (getSelectedConnection().Equals("Storage")) {
            return new SqlConnection(getConnectionString());
        } else if (getSelectedConnection().Equals("SqlLite")) {
            return new SQLiteConnection(getConnectionString());
        }

        return null;
    }
    public string getConnectionString() {
        return configurationManager.GetConnectionString(getSelectedConnection());
    }

    private string getSelectedConnection() {
        return configurationManager.GetConnectionString("dbServer");
    }
}