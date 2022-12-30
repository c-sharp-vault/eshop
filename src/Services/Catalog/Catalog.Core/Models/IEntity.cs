
using System;

namespace Catalog.Core.Models {
	public interface IEntity {
		public string CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }
	}
}
