using System;

namespace Catalog.Core.Models {
	public abstract class Entity : IEntity {
		private string _createdBy;
        private DateTime _createdOn;
        private string _updatedBy;
        private DateTime _updatedOn;

		public Entity() {
			_createdBy = Environment.UserName;
			_createdOn = DateTime.Now;
			_updatedBy = Environment.UserName;
			_updatedOn= DateTime.Now;
		}

		public string CreatedBy { 
			get => _createdBy;
			set => _createdBy = value ?? throw new ArgumentNullException(nameof(CreatedBy));
		}

        public DateTime CreatedOn {
            get => _createdOn;
			set => _createdOn = value;
        }

        public string UpdatedBy { 
            get => _updatedBy;
			set => _updatedBy = value ?? throw new ArgumentNullException(nameof(UpdatedBy));
		}

        public DateTime UpdatedOn { 
            get => _updatedOn;
			set => _updatedOn = value;
        }
	}
}
