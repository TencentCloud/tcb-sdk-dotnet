using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CloudBase {
	public class QueryCommand: LogicCommand {
		public QueryCommand(List < List < dynamic > >actions, List < dynamic > step) : base(actions, step) {}
		public LogicCommand Eq(object val) {
			return this.QueryOp(QueryCommandLiteral.EQ, val);
		}
		public LogicCommand Neq(object val) {
			return this.QueryOp(QueryCommandLiteral.NEQ, val);
		}
		public LogicCommand Gt(double val) {
			return this.QueryOp(QueryCommandLiteral.GT, val);
		}
		public LogicCommand Gte(double val) {
			return this.QueryOp(QueryCommandLiteral.GTE, val);
		}
		public LogicCommand Lt(double val) {
			return this.QueryOp(QueryCommandLiteral.LT, val);
		}
		public LogicCommand Lte(double val) {
			return this.QueryOp(QueryCommandLiteral.LTE, val);
		}
		public LogicCommand In(object vals) {
			return this.QueryOp(QueryCommandLiteral.IN, vals);
		}
		public LogicCommand Nin(object vals) {
			return this.QueryOp(QueryCommandLiteral.NIN, vals);
		}
		private LogicCommand QueryOp(string operation, object val) {
			List < dynamic > step = new List < dynamic > ();
			step.Add("$" + operation);
			step.Add(val);
			List < List < dynamic > >actions = new List < List < dynamic > >();
			QueryCommand command = new QueryCommand(actions, step);
			return this.And(command);
		}
	};
}