import IEaglePluginContext from "../sdk/plugin/IEaglePluginContext";
import EagleObject from "../sdk/web/EagleObject";
import IEagleObjectContext from "../sdk/web/IEagleObjectContext";
import IEaglePortApi from "../sdk/web/ports/IEaglePortApi";

export default class FileSourceInstance extends EagleObject {

	constructor(plugin: IEaglePluginContext, context: IEagleObjectContext) {
		super("FileSourceInstance", context);
		this.plugin = plugin;
	}

	private plugin: IEaglePluginContext;

}