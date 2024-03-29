﻿namespace Catalog.Infrastructure.Options {
	public class CatalogOptions {

		private string _picBaseUrl = string.Empty;
		private string _eventBusConnection = string.Empty;
		private bool _useCustomizationData;
		private bool _azureStorageEnabled;

		public string PicBaseUrl {
			get { return _picBaseUrl; }
			set { _picBaseUrl = value; }
		}

		public string EventBusConnection {
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
