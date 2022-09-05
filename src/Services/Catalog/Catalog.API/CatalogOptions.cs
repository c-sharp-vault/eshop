using System;

namespace Catalog.API {
	public class CatalogOptions {

		public const String KEY = "Catalog";

		private String _picBaseUrl;
		private String _eventBusConnection;
		private bool _useCustomizationData;
		private bool _azureStorageEnabled;

		public String PicBaseUrl {
			get { return _picBaseUrl; }
			set { _picBaseUrl = value; }
		}

		public String EventBusConnection {
			get { return _eventBusConnection; }
			set { _eventBusConnection = value; }
		}

		public bool UseCustomizationData {
			get { return _useCustomizationData; }
			set { _useCustomizationData = value; }
		}

		public bool AzureStorageEnabled {
			get { return _azureStorageEnabled; }
			set { _azureStorageEnabled = value; }
		}
	}
}
