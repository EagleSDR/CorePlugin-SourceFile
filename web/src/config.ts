import IEaglePluginContext from "../sdk/plugin/IEaglePluginContext";
import { SDK_VERSION } from "../sdk/SdkVersion";
import FileSourceInstance from "./FileSourceInstance";
import PluginMain from "./PluginMain";
declare var plugin: IEaglePluginContext;

plugin.Configure({
//  Should be pretty self-explanatory.
    name: "",
    version: "1.0",

//  This is loaded from the SDK. Just leave it be.
    sdk_version: SDK_VERSION,

//  *** READ ME ***
//  The app is built around the idea that objects on the server exist on the client too. That applies to us, a plugin, because we're just an object too!
//  Any EagleObject types you define on the server MUST be defined here as well. They're defined with a classname (the full name in CSharp) and a JS type.
    web_classes: {
        "EagleCore.SourceFile.FileSourceModule": PluginMain,
        "EagleCore.SourceFile.FileSourceInstance": FileSourceInstance
    },

//  Declare your custom types to the client.
    demodulators: [

    ],
    sources: [

    ]
})