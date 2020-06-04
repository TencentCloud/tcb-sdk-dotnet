using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CloudBase {
	public class LogicCommandLiteral {
		public const string AND = "and";
		public const string OR = "or";
		public const string NOT = "not";
		public const string NOR = "nor";
	};
	public class QueryCommandLiteral {
		public const string AND = "and";
		public const string EQ = "eq";
		public const string NEQ = "neq";
		public const string GT = "gt";
		public const string GTE = "gte";
		public const string LT = "lt";
		public const string LTE = "lte";
		public const string IN = "in";
		public const string NIN = "nin";
		public const string ALL = "all";
		public const string ELEM_MATCH = "elemMatch";
		public const string EXISTS = "exists";
		public const string SIZE = "size";
		public const string MOD = "mod";
		public const string GEO_NEAR = "geoNear";
		public const string GEO_WITHIN = "geoWithin";
		public const string GEO_INTERSECTS = "geoIntersects";
	};
	public class UpdateCommandLiteral {
		public const string SET = "set";
		public const string REMOVE = "remove";
		public const string INC = "inc";
		public const string MUL = "mul";
		public const string PUSH = "push";
		public const string PULL = "pull";
		public const string PULL_ALL = "pullAll";
		public const string POP = "pop";
		public const string SHIFT = "shift";
		public const string UNSHIFT = "unshift";
		public const string ADD_TO_SET = "addToSet";
		public const string BIT = "bit";
		public const string RENAME = "rename";
		public const string MAX = "max";
		public const string MIN = "min";
	};
}