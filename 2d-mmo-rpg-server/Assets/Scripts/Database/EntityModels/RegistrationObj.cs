namespace Database
{
    public class RegistrationObj
    {
        private string username;
        private string clientSalt;
        private string serverSalt;
        private string hashedAndDoubleSaltedPw;

        public RegistrationObj(string username, string clientSalt, string serverSalt, string hashedAndDoubleSaltedPw)
        {
            this.username = username;
            this.clientSalt = clientSalt;
            this.serverSalt = serverSalt;
            this.hashedAndDoubleSaltedPw = hashedAndDoubleSaltedPw;
        }

        public string Username => username;

        public string ClientSalt => clientSalt;

        public string ServerSalt => serverSalt;

        public string HashedAndDoubleSaltedPw => hashedAndDoubleSaltedPw;
        
        
    }
}