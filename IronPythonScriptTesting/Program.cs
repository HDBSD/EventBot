using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IronPythonScriptTesting
{
    internal class Program
    {
        static void Main(string[] arguments)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var probingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\libs";
                var assyName = new AssemblyName(args.Name);

                var newPath = Path.Combine(probingPath, assyName.Name);
                if (!newPath.EndsWith(".dll"))
                {
                    newPath = newPath + ".dll";
                }
                if (File.Exists(newPath))
                {
                    var assy = Assembly.LoadFile(newPath);
                    return assy;
                }
                return null;
            };

            BotTesting();            

        }

        static void BotTesting()
        {
            EventBotCore.EventBotCore a = new EventBotCore.EventBotCore();

            a.LoadScript(@"C:\Users\Brad\source\repos\EventBot\Resources\Scripts\Gamble\Gamble.py");
            a.ConnectToChat("arufashifuto", "", "HDBSD");


            Console.Read();
            a.DisconnectFromChat();
        }
    }
}
