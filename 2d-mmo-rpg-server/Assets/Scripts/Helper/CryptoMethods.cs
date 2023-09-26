using System;
using System.Security.Cryptography;
using System.Text;

public class CryptoMethods
{
    public static string DecodeToBase64String(string toDecodingString)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(toDecodingString));
    }
    
    public static string EncodeFromBase64String(string toEncodingString)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(toEncodingString));
    }
        
    public static string GenerateServerSalt()
    {
        string salt;
        using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
        {
            byte[] clientSaltBytes = new byte[8];
            rng.GetBytes(clientSaltBytes);

            salt = Convert.ToBase64String(clientSaltBytes);
        }

        return salt;
    }

    public static string GetSHA256(string text)
    {
        byte[] message = Encoding.UTF8.GetBytes(text);
        byte[] hashValue;

        using (SHA256Managed hashString = new SHA256Managed())
        {
            hashValue = hashString.ComputeHash(message);
        }

        string hex = "";
        foreach (var x in hashValue)
        {
            hex += string.Format("{0:x2}", x);
        }
        return hex;
    }
}
