using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpracheBinary
{
    /// <summary>
    /// Represents an input for parsing.
    /// </summary>
    public class Input : IInput
    {
        private readonly Stream _source;
        private readonly int _position;

        /// <summary>
        /// Gets the list of memos assigned to the <see cref="Input" /> instance.
        /// </summary>
        public IDictionary<object, object> Memos { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Input" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public Input(Stream source)
            : this(source, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Input" /> class.
        /// </summary>
        /// <param name="bytes">The source.</param>
        public Input(byte[] bytes)
            : this(new MemoryStream(bytes), 0)
        {
        }

        internal Input(Stream source, int position)
        {
            _source = source;
            _position = position;

            Memos = new Dictionary<object, object>();
        }

        internal Input(byte[] bytes, int position)
            : this(new MemoryStream(bytes), position) { }

        /// <summary>
        /// Advances the input.
        /// </summary>
        /// <returns>A new <see cref="IInput" /> that is advanced.</returns>
        /// <exception cref="System.InvalidOperationException">The input is already at the end of the source.</exception>
        public IInput Advance()
        {
            if (AtEnd)
                throw new InvalidOperationException("The input is already at the end of the source.");

            return new Input(_source, _position + 1);
        }

        /// <summary>
        /// Gets the whole source.
        /// </summary>
        public Stream Source { get { return _source; } }

        /// <summary>
        /// Gets the current <see cref="System.Byte" />.
        /// </summary>
        public byte Current
        {
            get
            {
                // костыль
                _source.Seek(_position, SeekOrigin.Begin);
                return (byte)_source.ReadByte();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the end of the source is reached.
        /// </summary>
        public bool AtEnd { get { return _position == _source.Length; } }

        /// <summary>
        /// Gets the current positon.
        /// </summary>
        public int Position { get { return _position; } }

        /// <summary>
        /// Seeks to the specified position.
        /// </summary>
        public void Seek(int position, SeekOrigin origin = SeekOrigin.Begin)
        {
            _source.Seek(position, origin);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Pos {0}", _position);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="Input" />.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_source != null ? _source.GetHashCode() : 0) * 397) ^ _position;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="Input" />.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="Input" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as IInput);
        }

        /// <summary>
        /// Indicates whether the current <see cref="Input" /> is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IInput other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            // TODO Это костыль, надо сделать нормально
            return _position == other.Position && _source.Length == other.Source.Length && _source.Position == other.Source.Position;
        }

        /// <summary>
        /// Indicates whether the left <see cref="Input" /> is equal to the right <see cref="Input" />.
        /// </summary>
        /// <param name="left">The left <see cref="Input" />.</param>
        /// <param name="right">The right <see cref="Input" />.</param>
        /// <returns>true if both objects are equal.</returns>
        public static bool operator ==(Input left, Input right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Indicates whether the left <see cref="Input" /> is not equal to the right <see cref="Input" />.
        /// </summary>
        /// <param name="left">The left <see cref="Input" />.</param>
        /// <param name="right">The right <see cref="Input" />.</param>
        /// <returns>true if the objects are not equal.</returns>
        public static bool operator !=(Input left, Input right)
        {
            return !Equals(left, right);
        }
    }
}
