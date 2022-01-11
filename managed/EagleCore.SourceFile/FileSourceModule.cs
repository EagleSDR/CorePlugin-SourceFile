using EagleWeb.Common;
using EagleWeb.Common.IO;
using EagleWeb.Common.IO.FileSystem;
using EagleWeb.Common.Misc;
using EagleWeb.Common.NetObjects;
using EagleWeb.Common.Plugin;
using EagleWeb.Common.Radio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EagleCore.SourceFile
{
    public class FileSourceModule : EagleObjectPlugin
    {
        public FileSourceModule(IEagleObjectPluginContext context) : base(context)
        {
            
        }

        protected override void ConfigureObject(IEagleObjectConfigureContext context)
        {
            base.ConfigureObject(context);
            context.CreatePortApi("CreateInstance")
                .Bind(HandlerCreateInstance);
        }

        public override void PluginInit()
        {
            
        }

        private JObject HandlerCreateInstance(IEagleClient client, JObject payload)
        {
            //Resolve the file token
            WebFsFileStream stream = Context.ResolveFileToken(payload.GetString("file_token"));

            //Create
            FileSourceInstance source = new FileSourceInstance(this, stream);

            //Respond with GUID
            JObject response = new JObject();
            response["guid"] = source.Guid;
            return response;
        }
    }
}
