using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Database
{
    public class DBUserLogins
    {
        public static void RegisterNewLogin(int clientId, RegistrationObj registrationObj)
        {
            DAOGame dao = DAOGame.GetInstance();
            if(dao.InsertNewUserLogin(clientId, registrationObj))
            {
                dao.InsertNewInventoryForCharEntry(new InventoryObj(registrationObj.Username, 1));
                dao.InsertNewExperienceForCharEntry(new ExperienceObj(registrationObj.Username, 1));
            }
        }
        
        public static string GetClientSaltForUser(string username)
        {
            DAOGame dao = DAOGame.GetInstance();
            return dao.SelectClientSaltFromUsername(username);
        }

        public static bool CheckPassword(string username, string clientHashedAndSaltedPassword)
        {
            DAOGame dao = DAOGame.GetInstance();
            List<String> serverSaltAndPassword = dao.GetServerSaltAndPasswordForUser(username);
            return serverSaltAndPassword[1] == CryptoMethods.GetSHA256(clientHashedAndSaltedPassword + serverSaltAndPassword[0]);
        }

        #region static Methods
        
        public static RegistrationObj PrepareNewRegistration(string username, string clientSalt, string hashedAndClientSaltedPassword)
        {
            string serverSalt = CryptoMethods.GenerateServerSalt();
            string hashedAndDoubleSaltedPassword = CryptoMethods.GetSHA256(hashedAndClientSaltedPassword + serverSalt);
            return new RegistrationObj(username, clientSalt, serverSalt, hashedAndDoubleSaltedPassword);
        }
        
        #endregion

    }
}