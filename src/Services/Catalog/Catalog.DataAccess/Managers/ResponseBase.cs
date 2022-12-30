
using System.Collections;
using System.Collections.Generic;

namespace Catalog.DataAccess.Managers {
	public abstract class ResponseBase {
		public bool Success { get; set; } = true;
		public IList<string> ErrorMessages { get; set; } = new List<string>();

		public void AddErrorMessage(string errorMessage) =>
			ErrorMessages.Add(errorMessage);
	}
}