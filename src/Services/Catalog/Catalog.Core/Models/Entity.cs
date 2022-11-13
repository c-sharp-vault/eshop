using System;

namespace Catalog.Core.Models {
	public abstract class Entity : IEntity {
		private String createdBy;
        private DateTime createdOn;
        private String? updatedBy;
        private DateTime? updatedOn;

		public Entity() {
			createdBy = Environment.UserName;
			createdOn = DateTime.Now;
		}

		public String CreatedBy { 
			get => createdBy;
			set => createdBy = value;
		}

        public DateTime CreatedOn {
            get => createdOn;
			set => createdOn = (value == null || ((DateTime?) value) != null) ? value : 
								throw new ArgumentException(nameof(createdOn), $"Value should be able to be casted to {typeof(DateTime?)}");
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
