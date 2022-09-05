using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Models {
	public class CatalogType : IEntity {
		private int _id;
		private String _type;

		public int Id {
			get { return _id; }
			set { _id = value; }
		}

		public String Type {
			get { return _type; }
			set { _type = value; }
		}

		public CatalogType(String type) {
			this.Type = Type;
		}
	}
}
