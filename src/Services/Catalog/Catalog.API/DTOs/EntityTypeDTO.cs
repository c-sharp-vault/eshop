
using System;

namespace Catalog.API.DTOs {
	public abstract class EntityTypeDTO : IEntityTypeDTO {
		private String createdBy;
        private DateTime createdOn;
        private String? updatedBy;
        private DateTime? updatedOn;

		public EntityTypeDTO() {
			createdBy = Environment.UserName;
			createdOn = DateTime.Now;
		}

		public int Id { get; set; }

		public String CreatedBy { 
			get => createdBy; 
		}

        public DateTime CreatedOn {
            get => createdOn;
        }

        public String? UpdatedBy { 
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
