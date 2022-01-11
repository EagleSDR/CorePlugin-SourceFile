import IEaglePluginContext from "../sdk/plugin/IEaglePluginContext";
import EagleObject from "../sdk/web/EagleObject";
import IEagleObjectContext from "../sdk/web/IEagleObjectContext";
import IEaglePortApi from "../sdk/web/ports/IEaglePortApi";
import FileSourceInstance from "./FileSourceInstance";

export default class PluginMain extends EagleObject {

	constructor(plugin: IEaglePluginContext, context: IEagleObjectContext) {
		super("PluginMain", context);
		this.plugin = plugin;

		//Get ports
		this.PortCreateInstance = this.net.GetPortApi("CreateInstance");
	}

	private plugin: IEaglePluginContext;

	private PortCreateInstance: IEaglePortApi;

	async CreateInstance(fileToken: string): Promise<FileSourceInstance> {
		//Send
		var response = await this.PortCreateInstance.SendRequest({
			"file_token": fileToken
		});

		//Resolve
		return this.ResolveNetObject<FileSourceInstance>(response["guid"]);
    }

}