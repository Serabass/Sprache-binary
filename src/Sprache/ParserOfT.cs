﻿using System;
using System.IO;

namespace Sprache.Binary
{
    /// <summary>
    /// Represents a parser.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="input">The input to parse.</param>
    /// <returns>The result of the parser.</returns>
    public delegate IResult<T> Parser<out T>(IInput input);

    /// <summary>
    /// Contains some extension methods for <see cref="Parser&lt;T&gt;" />.
    /// </summary>
    public static class ParserExtensions
    {
        /// <summary>
        /// Tries to parse the input without throwing an exception.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="parser">The parser.</param>
        /// <param name="input">The input.</param>
        /// <returns>The result of the parser</returns>
        public static IResult<T> TryParse<T>(this Parser<T> parser, Stream input)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            if (input == null) throw new ArgumentNullException(nameof(input));

            return parser(new Input(input));
        }

        /// <summary>
        /// Tries to parse the input without throwing an exception.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="parser">The parser.</param>
        /// <param name="input">The input.</param>
        /// <returns>The result of the parser</returns>
        public static IResult<T> TryParse<T>(this Parser<T> parser, byte[] input)
        {
            return TryParse(parser, new MemoryStream(input));
        }

        /// <summary>
        /// Parses the specified input string.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="parser">The parser.</param>
        /// <param name="input">The input.</param>
        /// <returns>The result of the parser.</returns>
        /// <exception cref="ParseException">It contains the details of the parsing error.</exception>
        public static T Parse<T>(this Parser<T> parser, Stream input)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            if (input == null) throw new ArgumentNullException(nameof(input));

            var result = parser.TryParse(input);

            if(result.WasSuccessful)
                return result.Value;

            throw new ParseException(result.ToString(), Position.FromInput(result.Remainder));
        }

        /// <summary>
        /// Parses the specified input string.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="parser">The parser.</param>
        /// <param name="predicate">The input.</param>
        /// <returns>The result of the parser.</returns>
        /// <exception cref="ParseException">It contains the details of the parsing error.</exception>
        public static T Parse<T>(this Parser<T> parser, Action<BinaryWriter> predicate)
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);
            {
                predicate(writer);
                writer.Flush();
            }
            memoryStream.Position = 0;
            return Parse(parser, memoryStream);
        }


        /// <summary>
        /// Parses the specified input string.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="parser">The parser.</param>
        /// <param name="input">The input.</param>
        /// <returns>The result of the parser.</returns>
        /// <exception cref="ParseException">It contains the details of the parsing error.</exception>
        public static T Parse<T>(this Parser<T> parser, byte[] input)
        {
            return Parse(parser, new MemoryStream(input));
        }
    }
}
