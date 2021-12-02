using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Runtime;

namespace EventBotCore.BotFramework
{
    public static class BotFunctions
    {

        internal static 

        public static bool AddPoints(string userid, string username, long amount)
        {
            
            return false;
        }

        public static bool RemovePoints(string userid, string username, long amount)
        {

            return false;
        }

        public static List<string> AddPointsAll(PythonDictionary data)
        { 
            List<string> users = new List<string>();

            return users;
        }

        public static List<string> AddPointsAllAsync(PythonDictionary data, Action callback)
        {
            List<string> users = new List<string>();
            callback();
            return users;
        }

        public static long GetPoints(string userid)
        {
            return 0;
        }

        public static long GetHours(string userid)
        {
            return 0;
        }

        public static long GetRank(string userid)
        {
            return 0;
        }

        public static PythonDictionary GetTopCurrency(int top)
        {
            var dict = new PythonDictionary();

            dict["hdbsd"] = 100000;
            return dict;
        }

        public static PythonDictionary GetTopHours(int top)
        {
            var dict = new PythonDictionary();

            dict["hdbsd"] = 100000;
            return dict;
        }

        public static PythonDictionary GetPointsAll(List<string> userids)
        {
            PythonDictionary dict = new PythonDictionary();

            return dict;
        }

        public static PythonDictionary GetRanksAll(List<string> userids)
        {
            PythonDictionary dict = new PythonDictionary();
            return dict;
        }

        public static PythonDictionary GetHoursAll(List<string> userids)
        {
            PythonDictionary dict = new PythonDictionary();
            return dict;
        }

        public static List<long> GetCurrencyUsers(List<string> userids)
        {
            return new List<long>();
        }

    }
}
