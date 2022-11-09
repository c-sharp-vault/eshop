
using System;

namespace Catalog.API.DTOs {
	public abstract class EntityTypeDTO : IEntityTypeDTO {
		private string createdBy;
        private DateTime createdOn;
        private string? updatedBy;
        private DateTime? updatedOn;

		public EntityTypeDTO() {
			createdBy = Environment.UserName;
			createdOn = DateTime.Now;
		}

		public int ID { get; set; }

		public string CreatedBy { 
			get => createdBy; 
		}

        public DateTime CreatedOn {
            get => createdOn;
        }

        public string? UpdatedBy { 
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
