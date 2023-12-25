﻿using System.Collections.Generic;

namespace Sprache
{
    /// <summary>
    /// Represents a commented result with its leading and trailing comments.
    /// </summary>
    /// <typeparam name="T">Type of the matched result.</typeparam>
    [System.Obsolete("Don't need this anymore")]
    public interface ICommented<T>
    {
        /// <summary>
        /// Gets the leading comments.
        /// </summary>
        [System.Obsolete("Don't need this anymore")]
        IEnumerable<string> LeadingComments { get; }

        /// <summary>
        /// Gets the resulting value.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets the trailing comments.
        /// </summary>
        [System.Obsolete("Don't need this anymore")]
        IEnumerable<string> TrailingComments { get; }
    }
}
