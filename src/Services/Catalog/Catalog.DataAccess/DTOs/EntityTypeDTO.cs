
using System;

namespace Catalog.DataAccess.DTOs {
	public abstract class EntityTypeDTO : IEntityTypeDTO {
		private string createdBy;
        private DateTime createdOn;
        private string updatedBy;
        private DateTime? updatedOn;

		public string CreatedBy { 
			get => createdBy; 
            set => createdBy = value;
		}

        public DateTime CreatedOn {
            get => createdOn;
            set => createdOn = value;
        }

        public string UpdatedBy { 
            get => updatedBy;
			set => updatedBy = value;
        }

        public DateTime? UpdatedOn { 
            get => updatedOn;  
			set => updatedOn = (value == null || ((DateTime?) value) != null) ? value : 
								throw new ArgumentException(nameof(updatedOn), $"Value should be able to be casted to {typeof(DateTime?)}");
        }
	}
}
