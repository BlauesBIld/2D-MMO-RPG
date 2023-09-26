namespace Database
{
    public class DBExperience
    {
        public static bool UpdateLevelAndExpForUserChar(ExperienceObj expObj)
        {
            return UpdateLevelForUserChar(expObj) && UpdateExpForUserChar(expObj) && UpdateMaxExpForUserChar(expObj);
        }

        private static bool UpdateMaxExpForUserChar(ExperienceObj expObj)
        {
            DAOGame dao = DAOGame.GetInstance();

            return dao.UpdateMaxExpForUserChar(expObj);
        }

        public static bool UpdateLevelForUserChar(ExperienceObj expObj)
        {
            DAOGame dao = DAOGame.GetInstance();

            return dao.UpdateLevelForUserChar(expObj);
        }
        
        public static bool UpdateExpForUserChar(ExperienceObj expObj)
        {
            DAOGame dao = DAOGame.GetInstance();

            return dao.UpdateExperienceForUserChar(expObj);
        }

        public static bool InsertNewExperienceEntryForUserChar(ExperienceObj expObj)
        {
            DAOGame dao = DAOGame.GetInstance();

            return dao.InsertNewExperienceForCharEntry(expObj);
        }

        public static int GetLevelForUserChar(ExperienceObj expObj)
        {
            DAOGame dao = DAOGame.GetInstance();

            return dao.SelectCurrentLevelFromUserChar(expObj);
        }
        
        public static int GetExperienceForUserChar(ExperienceObj expObj)
        {
            DAOGame dao = DAOGame.GetInstance();

            return dao.SelectCurrentExpFromUserChar(expObj);
        }

        public static int GetMaxExpForUserChar(ExperienceObj expObj)
        {
            DAOGame dao = DAOGame.GetInstance();

            return dao.SelectMaxExpFromUserChar(expObj);
        }
    }
}