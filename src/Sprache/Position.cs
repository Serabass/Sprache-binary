using System;
// ReSharper disable MemberCanBePrivate.Global

namespace SpracheBinary
{
    /// <summary>
    /// Represents a position in the input.
    /// </summary>
    public class Position : IEquatable<Position>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Position" /> class.
        /// </summary>
        /// <param name="pos">The position.</param>
        public Position(int pos)
        {
            Pos = pos;
        }

        /// <summary>
        /// Creates an new <see cref="Position"/> instance from a given <see cref="IInput"/> object.
        /// </summary>
        /// <param name="input">The current input.</param>
        /// <returns>A new <see cref="Position"/> instance.</returns>
        public static Position FromInput(IInput input)
        {
            return new Position(input.Position);
        }

        /// <summary>
        /// Gets the current positon.
        /// </summary>
        public int Pos
        {
            get;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="Position" />.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="Position" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as Position);
        }

        /// <summary>
        /// Indicates whether the current <see cref="Position" /> is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Position other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Pos == other.Pos;
        }

        /// <summary>
        /// Indicates whether the left <see cref="Position" /> is equal to the right <see cref="Position" />.
        /// </summary>
        /// <param name="left">The left <see cref="Position" />.</param>
        /// <param name="right">The right <see cref="Position" />.</param>
        /// <returns>true if both objects are equal.</returns>
        public static bool operator ==(Position left, Position right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Indicates whether the left <see cref="Position" /> is not equal to the right <see cref="Position" />.
        /// </summary>
        /// <param name="left">The left <see cref="Position" />.</param>
        /// <param name="right">The right <see cref="Position" />.</param>
        /// <returns>true if the objects are not equal.</returns>
        public static bool operator !=(Position left, Position right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="Position" />.
        /// </returns>
        public override int GetHashCode()
        {
            var h = 31;
            h = h * 13 + Pos;
            return h;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Position {0}", Pos);
        }
    }
}
