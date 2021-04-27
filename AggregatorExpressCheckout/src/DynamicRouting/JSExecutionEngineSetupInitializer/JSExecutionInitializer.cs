using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Jint;
using JavaScriptEngineSwitcher.Msie;
using JavaScriptEngineSwitcher.Node;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Vroom;
using Microsoft.ClearScript;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicRouting.JSExecutionEngineSetupInitializer
{
    public class JSExecutionInitializer
    {
        private  IJsEngineSwitcher engineSwitcher = null;
        private ILogger<JSExecutionInitializer> _Logger;
        
        public JSExecutionInitializer(ILogger<JSExecutionInitializer> logger)
        {
            engineSwitcher = JsEngineSwitcher.Current;
            engineSwitcher.EngineFactories
              .AddChakraCore()
              .AddJint()
               .AddMsie(new MsieSettings
               {
                   EngineMode = JsEngineMode.Auto
               })
             .AddNode()
             .AddV8()
             .AddVroom();
            _Logger = logger;
;

        }

       

        public  IJsEngine GetJsEngine()
        {
             IJsEngine engine = null;
            try
            {              
               
                engine = engineSwitcher.CreateEngine(V8JsEngine.EngineName);
                engine.EmbedHostObject("host", new HostFunctions());
                engine.Execute(@"
                    Array.prototype.toClrArray = function () {
                        var clrArray = host.newArr(this.length);
                        for (var i = 0; i < this.length; ++i) {
                            clrArray[i] = this[i];
                        }
                        return clrArray;
                    };
                    ");

                


            }
            catch(Exception ex)
            {
                this._Logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._Logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._Logger.LogError(ex.ToString());

            }
            return engine;


        }

        public static string[] GetGatewayArrayFromJSArray(object JsArrayObject)
        {
            string[] prioritiesList = ((IEnumerable)JsArrayObject).Cast<object>()
                              .Select(x => x.ToString())
                              .ToArray();
            return prioritiesList;
        }

    }
}
