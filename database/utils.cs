using System.Collections.Generic;

namespace CloudBase {
	class DbUtils {
		public static bool IsNullObject(object str) {
			return str == null;
		}
		public static bool IsNullOrEmptyString(string str) {
			return (string.IsNullOrEmpty(str));
		}
		public static bool IsNullOrEmptyMap(Dictionary<string, int> map) {
			return (map == null || map.Count <= 0);
		}
		public static bool IsNullOrEmptyArray(List<dynamic> list) {
			return (list == null && list.Count <= 0);
		}
	};
}