namespace Database
{
    public class ExperienceObj
    {
        private string username;
        private int charslot;
        private int currentLevel;
        private int currentExp;
        private int maxExp;

        public ExperienceObj(string _username, int _charslot, int _currentLevel, int _currentExp, int _maxExp)
        {
            username = _username;
            charslot = _charslot;
            currentLevel = _currentLevel;
            currentExp = _currentExp;
            maxExp = _maxExp;
        }

        public ExperienceObj(string _username, int _charslot)
        {
            username = _username;
            charslot = _charslot;
        }

        public string Username => username;

        public int Charslot => charslot;

        public int CurrentLevel => currentLevel;

        public int CurrentExp => currentExp;

        public int MaxExp => maxExp;
    }
}