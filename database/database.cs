using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CloudBase {
	public class Database {
		private Core Core;
		public Command Command;

		public Geo Geo;

		public Database(Core core) {
			this.Core = core;
			this.Command = new Command();
			this.Geo = new Geo();
		}
		public Collection Collection(string name) {
			return new Collection(this.Core, name);
		}
		public RegExp RegExp(string regexp, string option) {
			return new RegExp(regexp, option);
		}
		public ServerDate ServerDate(int offset) {
			return new ServerDate(offset);
		}
	};
}