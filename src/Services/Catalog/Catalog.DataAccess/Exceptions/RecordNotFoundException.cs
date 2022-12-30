using System;

namespace Catalog.DataAccess.Exceptions {
	public class RecordNotFoundException : Exception {
		public RecordNotFoundException(string message) : base(message) {

		}
	}
}