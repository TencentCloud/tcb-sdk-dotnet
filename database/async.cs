using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace CloudBase {
	partial class Query {
		public Task<DbQueryResponse> GetAsync() {
			return Task<DbQueryResponse>.Run(() => {
				return this.Get();
			});
		}

		public Task<DbCountResponse> CountAsync() {
			return Task<DbCountResponse>.Run(() => {
				return this.Count();
			});
		}

		public Task<DbUpdateResponse> UpdateAsync(object data) {
			return Task<DbUpdateResponse>.Run(() => {
				return this.Update(data);
			});
		}

		public Task<DbRemoveResponse> RemoveAsync() {
			return Task<DbRemoveResponse>.Run(() => {
				return this.Remove();
			});
		}
	};

	partial class Collection {
		public Task<DbCreateResponse> AddAsync(object data) {
			return Task<DbCreateResponse>.Run(() => {
				return this.Add(data);
			});
		}
	}

	partial class Document {
		public Task<DbQueryResponse> GetAsync() {
			return Task<DbQueryResponse>.Run(() => {
				return this.Get();
			});
		}

		public Task<DbCreateResponse> CreateAsync(object data) {
			return Task<DbCreateResponse>.Run(() => {
				return this.Create(data);
			});
		}

		public Task<DbUpdateResponse> SetAsync(object data) {
			return Task<DbUpdateResponse>.Run(() => {
				return this.Set(data);
			});
		}

		public Task<DbUpdateResponse> UpdateAsync(object data) {
			return Task<DbUpdateResponse>.Run(() => {
				return this.Update(data);
			});
		}

		public Task<DbRemoveResponse> RemoveAsync() {
			return Task<DbRemoveResponse>.Run(() => {
				return this.Remove();
			});
		}
	}
}