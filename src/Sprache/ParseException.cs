﻿using System;

namespace Sprache.Binary
{
    /// <summary>
    /// Represents an error that occurs during parsing.
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException" /> class.
        /// </summary>
        public ParseException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ParseException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException" /> class with a specified error message
        /// and the position where the error occured.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="position">The position where the error occured.</param>
        public ParseException(string message, Position position) : base(message)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseException" /> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ParseException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Gets the position of the parsing failure if one is available; otherwise, null.
        /// </summary>
        public Position Position {
            get;
        }
    }
}
