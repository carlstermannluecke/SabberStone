﻿using System;
using System.Collections.Generic;

namespace SabberStoneCore.Collections
{
	/// <summary>
	/// Parent interface for all collections which inhibit the unique item constraint.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="System.Collections.Generic.IReadOnlyCollection{T}" />
	/// <autogeneratedoc />
	public interface IReadOnlySet<T>: IReadOnlyCollection<T>
    {
		/// <summary>
		/// Determines whether this set contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if the set contains thie specified item; otherwise, <c>false</c>.</returns>
		bool Contains(T item);

		/// <summary>Executes the specified lambda on each of the elements of the set.</summary>
		/// <param name="lambda">The lambda.</param>
		void ForEach(Action<T> lambda);

		/// <summary>Checks if there is an item in the set for which the lambda returns true.</summary>
		/// <param name="lambda">The lambda.</param>
		/// <returns></returns>
		bool Exists(Func<T, bool> lambda);
	}
}