using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using DependencyChecker.CheckEvaluators.Helpers;

namespace DependencyChecker.SystemIntegration
{
    public class DatabaseCommands
    {
        private readonly string aliasName;
        private readonly string dbName;
        private const string MasterDb = "master";
        private readonly string CreateDatabaseSqlScriptFileName;
        private readonly string CreateObjectsSqlScriptFileName;

        public DatabaseCommands(string aliasName, string dbName)
        {
            this.aliasName = aliasName;
            this.dbName = dbName;

            CreateDatabaseSqlScriptFileName = "create_" + dbName + ".sql";
            CreateObjectsSqlScriptFileName = "create_objects_" + dbName + ".sql";
        }

        public string GetNetworkServiceUser()
        {
            return @"NT AUTHORITY\NETWORK SERVICE";
        }

        public bool CheckDbPermissions()
        {
            var networkServiceUser = GetNetworkServiceUser();

            return DatabaseUserExists(networkServiceUser);
        }

        protected bool DatabaseUserExists(string user)
        {
            var cmd = string.Format(
                CultureInfo.InvariantCulture,
                @"USE [{0}]
                  SELECT principal_id FROM sys.database_principals WHERE name = N'{1}'",
                dbName,
                user);

            var result = ExecuteSqlCommandWithResults(aliasName, dbName, cmd);

            if (result == DBNull.Value || Convert.ToInt32(result) <= 0)
            {
                return false;
            }

            if (!CheckUserIsInDboRole(aliasName, dbName, user))
            {
                return false;
            }

            return true;
        }

        protected bool CheckUserIsInDboRole(string serverName, string dbName, string user)
        {
            var cmd = string.Format(
                CultureInfo.InvariantCulture,
                @"use [{0}]
                 select  rolename=case when (r.principal_id is null) then 'public'	else r.name end
	             from sys.database_principals u
                        left join (sys.database_role_members m join sys.database_principals r on m.role_principal_id = r.principal_id) on u.principal_id = m.member_principal_id
                        left join sys.server_principals l on u.sid = l.sid
	             where u.name = '{1}' and u.type <> 'R'",
                dbName,
                user);

            var result = ExecuteSqlCommandWithResults(serverName, dbName, cmd);

            return result != DBNull.Value && result.ToString().Trim().ToLower() == "db_owner";
        }

        public void GrantAccessToUser(string user)
        {
            var cmd =
                string.Format(
                    @"USE [master]
                      IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{1}')
                      BEGIN
  	                    CREATE LOGIN [{1}] FROM WINDOWS WITH DEFAULT_DATABASE=[{0}]
                      END
                      USE [{0}]
                      CREATE USER [{1}] FOR LOGIN [{1}]
                      USE [{0}]
                      EXEC sp_addrolemember N'db_owner', N'{1}'",
                    dbName,
                    user);

            ExecuteSqlcommand(cmd, MasterDb);
        }

        protected void CreateSqlAlias(string sqlServerName)
        {
            string aliasValue;
            string propertyPath;

            if (sqlServerName == ".")
            {
                aliasValue = @"DBNMPNTW,\.\PIPE\sql\query";
            }
            else
            {
                aliasValue = @"DBNMPNTW,\\.\PIPE\MSSQL$SQLEXPRESS\sql\query";
            }

            if (RegistryHelper.KeyExists(@"HKLM:SOFTWARE\Wow6432Node\Microsoft\MSSQLServer"))
            {
                propertyPath = @"SOFTWARE\Wow6432Node\Microsoft\MSSQLServer\Client\ConnectTo";
                RegistryHelper.AddValue(propertyPath, aliasName, aliasValue);
            }

            propertyPath = @"SOFTWARE\Microsoft\MSSQLServer\Client\ConnectTo";
            RegistryHelper.AddValue(propertyPath, aliasName, aliasValue);
        }

        protected static SqlCommand CreateSqlCommand(string sqlCommandText)
        {
            var command = new SqlCommand { CommandType = System.Data.CommandType.Text, CommandText = sqlCommandText };
            return command;
        }

        public static string GetSqlConnectionString(string serverName, string databaseName)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Server={0};Database={1};Integrated Security=True;pooling=false", serverName, databaseName);
        }

        public static string GetSqlServerNameInComputer()
        {
            var sqlServerNames = new [] { @".\SQLExpress", "." };
            foreach (var server in sqlServerNames)
            {
                if (ExistsDb(server, MasterDb))
                {
                    return server;
                }
            }

            return null;
        }

        public bool ExistsDb()
        {
            return ExistsDb(aliasName, dbName);
        }

        public static bool ExistsDb(string sqlServerName, string dbName)
        {
            var db = dbName;
            object name;

            try
            {
                name = ExecuteSqlCommandWithResults(sqlServerName, MasterDb,
                                                    string.Format("SELECT name FROM master.dbo.sysdatabases WHERE name = N'{0}'", db)
                                                    );
            }
            catch (Exception)
            {
                return false;
            }

            return name != null;
        }

        protected static object ExecuteSqlCommandWithResults(string serverName, string dataBase, string sqlCommandText)
        {
            var connectionString = GetSqlConnectionString(serverName, dataBase);
            object retValue;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = CreateSqlCommand(sqlCommandText))
                {
                    command.Connection = connection;
                    connection.Open();
                    retValue = command.ExecuteScalar();
                }
            }

            return retValue;
        }

        protected string GetSqlText(string scriptName)
        {
            string sqlScriptFilePath = Path.Combine(@".\SqlScripts", scriptName);
            string cmd;

            using (var scriptReader = new StreamReader(sqlScriptFilePath))
            {
                cmd = scriptReader.ReadToEnd();
            }

            return cmd;
        }

        protected void ExecuteSqlcommand(string sqlCommandText, string targetDb = null)
        {
            var connectionString = GetSqlConnectionString(aliasName, string.IsNullOrEmpty(targetDb) ? dbName : targetDb);

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = CreateSqlCommand(sqlCommandText))
                {
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        protected void CreateDatabaseObjects()
        {
            var cmd = GetSqlText(CreateObjectsSqlScriptFileName);
            ExecuteSqlcommand(cmd);
        }

        protected void CreateDatabase()
        {
            var cmd = GetSqlText(CreateDatabaseSqlScriptFileName);
            ExecuteSqlcommand(cmd, MasterDb);
        }

        public void CreateDb()
        {
            var serverName = GetSqlServerNameInComputer();

            if (serverName == null)
            {
                throw new InvalidOperationException("Your server instance could not be determined. Please check that Sql Server is installed and running.");
            }

            CreateSqlAlias(serverName);

            CreateDatabase();

            CreateDatabaseObjects();
        }
    }
}