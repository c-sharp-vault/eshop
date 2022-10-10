namespace Catalog.Infrastructure.Extensions.Linq {
	public class SelectTryResult<TSource, TResult> {
		private TSource _source;
		private TResult _result;
		private Exception? _exception;

		internal SelectTryResult(TSource source, TResult result, Exception exception) {
			_source = source;
			_result = result;
			_exception = exception;
		}

		public TSource Source {
			get { return _source; }
			private set { _source = value; }
		}

		public TResult Result {
			get { return _result; }
			private set { _result = value; }
		}

		public Exception CaughtException {
			get { return _exception; }
			private set { _exception = value; }
		}
	}
}
