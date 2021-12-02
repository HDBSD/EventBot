using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using IronPython.Hosting;
using IronPython.Modules;
using IronPython.Runtime;
using IronPython;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;

using System.IO;

namespace EventBotCore.PythonScripting
{

    internal class PythonScriptRuntime
    {
        public CompiledCode compiledCode;
        public dynamic scope;
    }

    internal class PythonScriptEngine
    {
        private readonly ScriptEngine m_engine;
        private Dictionary<string, PythonScriptRuntime> script_Scopes = new Dictionary<string, PythonScriptRuntime>();
        private Timer timer = new Timer();

        public PythonScriptEngine()
        {
            m_engine = Python.CreateEngine();

            ICollection<string> searchPaths = m_engine.GetSearchPaths();
            searchPaths.Add(@"C:\Python27\Lib\");
            m_engine.SetSearchPaths(searchPaths);

            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += (s, e) =>
            {
                performTick();
            };
        }

        ~PythonScriptEngine()
        {
            foreach (var script in script_Scopes.Keys)
            {
                try { 
                    var unload = script_Scopes[script].scope.GetVariable("Unload");
                    unload();
                }
                catch (MissingMemberException ex) {}

                script_Scopes[script].scope = null;
                script_Scopes[script].compiledCode = null;
            }

            script_Scopes = null;
        }

        public void loadScript(string path)
        {
            if (script_Scopes.ContainsKey(path))
                throw new ApplicationException("Script already loaded.");

            var psr = new PythonScriptRuntime()
            {
                scope = m_engine.CreateScope(),
                compiledCode = m_engine.CreateScriptSourceFromFile(path).Compile()
            };

            // setup parent object, store script in scope, run init and store the scope in dict
            psr.scope.parent = new BotFramework.BotFunctions();

            // Import sys and set search path

            m_engine.CreateScriptSourceFromString("import sys\r\n" + $@"sys.path.append(r""{Path.GetDirectoryName(path)}"")").Compile().Execute(psr.scope);

            // Load script

            psr.compiledCode.Execute(psr.scope);

            // check if manditory methods exist

            psr.scope.GetVariable("Init");
            psr.scope.GetVariable("Tick");
            psr.scope.GetVariable("Execute");

            // execute init

            psr.scope.GetVariable("Init")();
            script_Scopes.Add(path, psr);
            
        }

        public void unloadScript(string path)
        {
            if (script_Scopes.ContainsKey(path))
                return;

            var unload = script_Scopes[path].scope.GetVariable("Unload");

            if (unload != null)
                unload();

            script_Scopes.Remove(path);
        }

        public void performTick()
        {
            foreach(var script in script_Scopes.Values)
            {
                script.scope.GetVariable("Tick")();
            }
        }

        public void reload(string path)
        {
            if (script_Scopes.ContainsKey(path))
                return;

            var rld = script_Scopes[path].scope.GetVariable("Reload");

            if (rld != null)
                rld();

        }

        public void performExecute(IRC.Data data)
        {
            foreach (var script in script_Scopes.Values)
            {
                script.scope.GetVariable("Execute")((dynamic)data);
            }
        }

    }
}
