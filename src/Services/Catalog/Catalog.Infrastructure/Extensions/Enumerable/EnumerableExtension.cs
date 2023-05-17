﻿
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Infrastructure.Extensions.Enumerable {
	public static class EnumerableExtension {
		public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) 
			=> self.Select((item, index) => (item, index));
	}
}
