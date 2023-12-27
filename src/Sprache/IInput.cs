using System;
using System.Collections.Generic;
using System.IO;

namespace Sprache.Binary
{
    /// <summary>
    /// Represents an input for parsing.
    /// </summary>
    public interface IInput : IEquatable<IInput>
    {
        /// <summary>
        /// Advances the input.
        /// </summary>
        /// <returns>A new <see cref="IInput" /> that is advanced.</returns>
        /// <exception cref="System.InvalidOperationException">The input is already at the end of the source.</exception>
        IInput Advance();

        /// <summary>
        /// Gets the whole source.
        /// </summary>
        Stream Source { get; }

        /// <summary>
        /// Gets the current <see cref="System.Char" />.
        /// </summary>
        byte Current { get; }

        /// <summary>
        /// Gets a value indicating whether the end of the source is reached.
        /// </summary>
        bool AtEnd { get; }

        /// <summary>
        /// Gets the current positon.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Seeks to the specified position.
        /// </summary>
        void Seek(int position, SeekOrigin origin = SeekOrigin.Begin);

        /// <summary>
        /// Memos used by this input
        /// </summary>
        IDictionary<object, object> Memos { get; }
    }
}
