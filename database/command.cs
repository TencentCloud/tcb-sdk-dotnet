using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CloudBase {
	public class Command {
		public QueryCommand Eq(object val) {
			return this.QueryOp(QueryCommandLiteral.EQ, val);
		}
		public QueryCommand Neq(object val) {
			return this.QueryOp(QueryCommandLiteral.NEQ, val);
		}
		public QueryCommand Lt(double val) {
			return this.QueryOp(QueryCommandLiteral.LT, val);
		}
		public QueryCommand Lte(double val) {
			return this.QueryOp(QueryCommandLiteral.LTE, val);
		}
		public QueryCommand Gt(double val) {
			return this.QueryOp(QueryCommandLiteral.GT, val);
		}
		public QueryCommand Gte(double val) {
			return this.QueryOp(QueryCommandLiteral.GTE, val);
		}
		public QueryCommand In(object vals) {
			return this.QueryOp(QueryCommandLiteral.IN, vals);
		}
		public QueryCommand Nin(object vals) {
			return this.QueryOp(QueryCommandLiteral.NIN, vals);
		}
		public QueryCommand Exists(bool val) {
			return this.QueryOp(QueryCommandLiteral.EXISTS, val);
		}
		public QueryCommand Mod(double divisor, double remainder) {
			List < double > vals = new List < double > ();
			vals.Add(divisor);
			vals.Add(remainder);
			return this.QueryOp(QueryCommandLiteral.MOD, vals);
		}
		public QueryCommand All(object vals) {
			return this.QueryOp(QueryCommandLiteral.ALL, vals);
		}
		public QueryCommand ElemMatch(object val) {
			return this.QueryOp(QueryCommandLiteral.ELEM_MATCH, val);
		}
		public QueryCommand Size(double val) {
			return this.QueryOp(QueryCommandLiteral.SIZE, val);
		}
		public QueryCommand GeoNear(Point geometry, float maxDistance, float minDistance) {
			if (geometry == null) {
				throw new CloudBaseException(
					CloudBaseExceptionCode.INVALID_PARAM,
					"geometry can not be null"
				);
			}

			object param = new {
				geometry = geometry,
				maxDistance = maxDistance,
				minDistance = minDistance
			};

			return this.QueryOp(QueryCommandLiteral.GEO_NEAR, param);
		}
		public QueryCommand GeoWithin(object geometry) {
			if (!(geometry is MultiPolygon || geometry is Polygon)) {
				throw new CloudBaseException(
					CloudBaseExceptionCode.INVALID_PARAM,
					"geometry must be type of Polygon or MultiPolygon"
				);
			}

			object param = new {
				geometry = geometry
			};

			return this.QueryOp(QueryCommandLiteral.GEO_WITHIN, param);
		}

		public QueryCommand GeoIntersects(object geometry) {
			if (!(geometry is Point || geometry is LineString || geometry is Polygon ||
        geometry is MultiPoint || geometry is MultiLineString || geometry is MultiPolygon)) {
				throw new CloudBaseException(
					CloudBaseExceptionCode.INVALID_PARAM,
					"geometry must be be type of Point, LineString, Polygon, MultiPoint, MultiLineString or MultiPolygon"
				);
			}

			object param = new {
				geometry = geometry
			};

			return this.QueryOp(QueryCommandLiteral.GEO_INTERSECTS, param);
		}
		
		public LogicCommand And(LogicCommand expression) {
			List < LogicCommand > expressions = new List < LogicCommand > ();
			expressions.Add(expression);
			return this.LogicOp(LogicCommandLiteral.AND, expressions);
		}
		public LogicCommand And(LogicCommand[] expressions) {
			return this.LogicOp(LogicCommandLiteral.AND, new List<LogicCommand>(expressions));
		}
		public LogicCommand Or(LogicCommand expression) {
			List < LogicCommand > expressions = new List < LogicCommand > ();
			expressions.Add(expression);
			return this.LogicOp(LogicCommandLiteral.OR, expressions);
		}
		public LogicCommand Or(LogicCommand[] expressions) {
			return this.LogicOp(LogicCommandLiteral.OR, new List<LogicCommand>(expressions));
		}
		public LogicCommand Not(LogicCommand expression) {
			List < LogicCommand > expressions = new List < LogicCommand > ();
			expressions.Add(expression);
			return this.LogicOp(LogicCommandLiteral.NOT, expressions);
		}
		public LogicCommand Nor(LogicCommand[] expressions) {
			return this.LogicOp(LogicCommandLiteral.NOR, new List<LogicCommand>(expressions));
		}
		public UpdateCommand Set(object val) {
			return this.UpdateOp(UpdateCommandLiteral.SET, val);
		}
		public UpdateCommand Remove() {
			List < double > vals = new List < double > ();
			return this.UpdateOp(UpdateCommandLiteral.REMOVE, vals);
		}
		public UpdateCommand Inc(double val) {
			return this.UpdateOp(UpdateCommandLiteral.INC, val);
		}
		public UpdateCommand Mul(double val) {
			return this.UpdateOp(UpdateCommandLiteral.MUL, val);
		}
		public UpdateCommand Max(double val) {
			return this.UpdateOp(UpdateCommandLiteral.MAX, val);
		}
		public UpdateCommand Min(double val) {
			return this.UpdateOp(UpdateCommandLiteral.MIN, val);
		}
		public UpdateCommand Rename(string val) {
			return this.UpdateOp(UpdateCommandLiteral.RENAME, val);
		}
		public UpdateCommand Push(object val) {
			return this.UpdateOp(UpdateCommandLiteral.PUSH, val);
		}
		public UpdateCommand Pop() {
			List < double > vals = new List < double > ();
			return this.UpdateOp(UpdateCommandLiteral.POP, vals);
		}
		public UpdateCommand Unshift(object val) {
			return this.UpdateOp(UpdateCommandLiteral.UNSHIFT, val);
		}
		public UpdateCommand Shift() {
			List < double > vals = new List < double > ();
			return this.UpdateOp(UpdateCommandLiteral.SHIFT, vals);
		}
		public UpdateCommand Pull(object val) {
			return this.UpdateOp(UpdateCommandLiteral.PULL, val);
		}
		public UpdateCommand PullAll(object vals) {
			return this.UpdateOp(UpdateCommandLiteral.PULL_ALL, vals);
		}
		public UpdateCommand AddToSet(object val) {
			return this.UpdateOp(UpdateCommandLiteral.ADD_TO_SET, val);
		}
		private QueryCommand QueryOp(string operation, object val) {
			List < dynamic > step = new List < dynamic > ();
			step.Add("$" + operation);
			step.Add(val);
			List < List < dynamic > >actions = new List < List < dynamic > >();
			QueryCommand command = new QueryCommand(actions, step);
			return command;
		}
		private LogicCommand LogicOp(string operation, List < LogicCommand > expressions) {
			List < dynamic > step = new List < dynamic > ();
			step.Add("$" + operation);
			foreach(var expression in expressions) {
				step.Add(expression);
			}
			List < List < dynamic > >actions = new List < List < dynamic > >();
			LogicCommand command = new LogicCommand(actions, step);
			return command;
		}
		private UpdateCommand UpdateOp(string operation, object val) {
			List < dynamic > step = new List < dynamic > ();
			step.Add("$" + operation);
			step.Add(val);
			List < List < dynamic > >actions = new List < List < dynamic > >();
			UpdateCommand command = new UpdateCommand(actions, step);
			return command;
		}
	};
}