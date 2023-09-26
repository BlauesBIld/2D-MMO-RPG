using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Database;
using Mono.Data.Sqlite;
using UnityEngine;

public class DAOGame
{
    private SqliteConnection connection;
    private static string connectionString;

    private static DAOGame instance = new DAOGame();

    private DAOGame()
    {
        connectionString = "URI=file:" + Application.persistentDataPath + "/game_db.db";
        connection = new SqliteConnection(connectionString);
    }

    public static DAOGame GetInstance()
    {
        return instance;
    }

    public bool OpenConnection()
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return true;
        }
        catch (SqliteException ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (SqliteException e)
        {
            Debug.Log(e);
            return false;
        }
    }

    #region experience statements

    
    public int SelectMaxExpFromUserChar(ExperienceObj expObj)
    {
        int maxExp = 0;

        if (OpenConnection())
        {
            string query = $"SELECT maxexp FROM experience WHERE username = '{expObj.Username}' AND charslot = {expObj.Charslot};";
            SqliteCommand statement = new SqliteCommand(query, connection);
            SqliteDataReader dataReader = statement.ExecuteReader();

            if (dataReader.Read())
            {
                maxExp = (int) (long) dataReader.GetValue(0);
            }

            statement.Dispose();
            dataReader.Close();
            CloseConnection();
        }

        return maxExp; 
    }
    
    public int SelectCurrentExpFromUserChar(ExperienceObj expObj)
    {
        int currentExp = 0;

        if (OpenConnection())
        {
            string query = $"SELECT currentexp FROM experience WHERE username = '{expObj.Username}' AND charslot = {expObj.Charslot};";
            SqliteCommand statement = new SqliteCommand(query, connection);
            SqliteDataReader dataReader = statement.ExecuteReader();

            if (dataReader.Read())
            {
                currentExp = (int) (long) dataReader.GetValue(0);
            }

            statement.Dispose();
            dataReader.Close();
            CloseConnection();
        }

        return currentExp;  
    }
    
    public int SelectCurrentLevelFromUserChar(ExperienceObj expObj)
    {
        int level = 0;

        if (OpenConnection())
        {
            string query = $"SELECT level FROM experience WHERE username = '{expObj.Username}' AND charslot = {expObj.Charslot};";
            SqliteCommand statement = new SqliteCommand(query, connection);
            SqliteDataReader dataReader = statement.ExecuteReader();

            if (dataReader.Read())
            {
                level = (int) (long) dataReader.GetValue(0);

            }

            statement.Dispose();
            dataReader.Close();
            CloseConnection();
        }

        return level;    
    }

    public bool UpdateLevelForUserChar(ExperienceObj expObj)
    {
        return ExecuteInsertOrUpdate($"UPDATE experience SET level = {expObj.CurrentLevel} WHERE username = '{expObj.Username}' AND charslot = {expObj.Charslot};");
    }
    
    public bool UpdateMaxExpForUserChar(ExperienceObj expObj)
    {
        return ExecuteInsertOrUpdate($"UPDATE experience SET maxexp = {expObj.MaxExp} WHERE username = '{expObj.Username}' AND charslot = {expObj.Charslot};");
    }
    public bool UpdateExperienceForUserChar(ExperienceObj expObj)
    {
        return ExecuteInsertOrUpdate($"UPDATE experience SET currentexp = {expObj.CurrentExp} WHERE username = '{expObj.Username}' AND charslot = {expObj.Charslot};");
    }

    public bool InsertNewExperienceForCharEntry(ExperienceObj expObj)
    {
        return ExecuteInsertOrUpdate($"INSERT INTO experience VALUES ('{expObj.Username}', {expObj.Charslot}, 1, 0, 100);");
    }

    #endregion

    #region inventory statements

    public string SelectInventoryFromUserChar(InventoryObj invObj)
    {
        string inventory = null;

        if (OpenConnection())
        {
            string query = $"SELECT inventory FROM inventories WHERE username = '{invObj.Username}' AND charslot = {invObj.Charslot};";
            SqliteCommand statement = new SqliteCommand(query, connection);
            SqliteDataReader dataReader = statement.ExecuteReader();

            if (dataReader.Read())
            {
                inventory = (string) dataReader.GetValue(0);
            }

            statement.Dispose();
            dataReader.Close();
            CloseConnection();
        }

        return inventory;
    }

    public bool UpdateInventoryForUserChar(InventoryObj invObj)
    {
        return ExecuteInsertOrUpdate($"UPDATE inventories SET inventory = '{invObj.Base64CodedInventory}' WHERE username = '{invObj.Username}' AND charslot = {invObj.Charslot};");
    }

    public bool InsertNewInventoryForCharEntry(InventoryObj invObj)
    {
        return ExecuteInsertOrUpdate($"INSERT INTO inventories VALUES ('{invObj.Username}', {invObj.Charslot}, '{CryptoMethods.DecodeToBase64String("[null, null, null, null, null, null, null, null, null, null, null, null]")}');");
    }

    #endregion

    #region login statements

    public bool InsertNewUserLogin(int clientId, RegistrationObj registrationObj)
    {
        bool executionSuccesfull = false;

        if (OpenConnection())
        {
            SqliteTransaction trans = connection.BeginTransaction();
            SqliteCommand statement = new SqliteCommand($"INSERT INTO logins VALUES ('{registrationObj.Username}','{registrationObj.ClientSalt}','{registrationObj.ServerSalt}','{registrationObj.HashedAndDoubleSaltedPw}');", connection, trans);
            try
            {
                statement.ExecuteNonQuery();
                trans.Commit();
                ServerSend.RegistrationResult(Server.clients[clientId].id, true, "Registration successful!");
                executionSuccesfull = true;
            }
            catch (SqliteException ex)
            {
                HandleRegistrationErrorForUser(clientId, ex);
                RollBackTransactionAndHandleError(trans);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                statement.Dispose();
                trans.Dispose();
                CloseConnection();
            }
        }

        return executionSuccesfull;
    }

    public string SelectClientSaltFromUsername(string username)
    {
        string clientSalt = null;

        if (OpenConnection())
        {
            string query = $"SELECT clientSalt FROM logins WHERE username = '{username}';";
            SqliteCommand statement = new SqliteCommand(query, connection);
            SqliteDataReader dataReader = statement.ExecuteReader();

            if (dataReader.Read())
            {
                clientSalt = (string) dataReader.GetValue(0);
            }

            statement.Dispose();
            dataReader.Close();
            CloseConnection();
        }

        return clientSalt;
    }

    public List<String> GetServerSaltAndPasswordForUser(string username)
    {
        OpenConnection();

        List<string> serverSaltAndPassword = new List<string>();
        string query = $"SELECT serverSalt, hashedAndSaltedPw FROM logins WHERE username = '{username}';";
        SqliteCommand statement = new SqliteCommand(query, connection);
        SqliteDataReader dataReader = statement.ExecuteReader();

        if (dataReader.Read())
        {
            serverSaltAndPassword.Add((string) dataReader.GetValue(0));
            serverSaltAndPassword.Add((string) dataReader.GetValue(1));
        }

        statement.Dispose();
        dataReader.Close();
        CloseConnection();

        return serverSaltAndPassword;
    }

    #endregion

    #region create tables statements

    public bool CreateAllTablesIfNotExisting()
    {
        bool executionSuccesfull = false;
        if (OpenConnection())
        {
            SqliteTransaction trans = connection.BeginTransaction();
            SqliteCommand statement = new SqliteCommand($"CREATE TABLE IF NOT EXISTS 'inventories' ('username'    TEXT NOT NULL, 'charslot'    INTEGER NOT NULL, 'inventory'    TEXT NOT NULL, FOREIGN KEY('username') REFERENCES 'logins'('username'),PRIMARY KEY('charslot','username'));" +
                                                        $"CREATE TABLE IF NOT EXISTS 'logins' ('username'    TEXT NOT NULL CHECK(LENGTH(username) > 0) UNIQUE,'clientSalt'    TEXT NOT NULL, 'serverSalt'    TEXT NOT NULL, 'hashedAndSaltedPw'    TEXT NOT NULL, PRIMARY KEY('username'));" +
                                                        $"CREATE TABLE IF NOT EXISTS 'experience' ('username'    TEXT NOT NULL, 'charslot'    INTEGER NOT NULL, 'level'    INTEGER NOT NULL, 'currentexp'    INTEGER NOT NULL, 'maxexp'    INTEGER NOT NULL, FOREIGN KEY('username') REFERENCES 'logins'('username'),PRIMARY KEY('username','charslot'));",
                connection, trans);

            try
            {
                statement.ExecuteNonQuery();
                trans.Commit();
                executionSuccesfull = true;
            }
            catch (SqliteException ex)
            {
                RollBackTransactionAndHandleError(trans);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                statement.Dispose();
                trans.Dispose();
                CloseConnection();
            }
        }

        return executionSuccesfull;
    }

    #endregion

    #region generalSqlMethods

    private bool ExecuteInsertOrUpdate(string sqlStatement)
    {
        bool executionSuccesfull = false;

        if (OpenConnection())
        {
            SqliteTransaction trans = connection.BeginTransaction();
            SqliteCommand statement = new SqliteCommand(sqlStatement, connection, trans);
            try
            {
                statement.ExecuteNonQuery();
                trans.Commit();
                executionSuccesfull = true;
            }
            catch (SqliteException ex)
            {
                Debug.LogException(ex);
                RollBackTransactionAndHandleError(trans);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                statement.Dispose();
                trans.Dispose();
                CloseConnection();
            }
        }

        return executionSuccesfull;
    }

    #endregion

    #region static methods

    private static void RollBackTransactionAndHandleError(SqliteTransaction trans)
    {
        try
        {
            trans.Rollback();
        }
        catch (SqliteException ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void HandleRegistrationErrorForUser(int clientId, SqliteException ex)
    {
        switch (ex.ErrorCode)
        {
            case SQLiteErrorCode.Constraint:
                ServerSend.RegistrationResult(Server.clients[clientId].id, false, "The username is already taken!");
                break;
            default:
                Debug.LogException(ex);
                break;
        }
    }

    #endregion
}