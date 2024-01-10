using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Sprache.Binary.Tests
{
    static class AssertParser
    {
        public static void SucceedsWithOne<T>(Parser<IEnumerable<T>> parser, IEnumerable<byte> input, T expectedResult)
        {
            SucceedsWith(parser, input, t =>
            {
                Assert.Single(t);
                Assert.Equal(expectedResult, t.Single());
            });
        }

        public static void SucceedsWithMany<T>(Parser<IEnumerable<T>> parser, IEnumerable<byte> input, IEnumerable<T> expectedResult)
        {
            SucceedsWith(parser, input, t => Assert.True(t.SequenceEqual(expectedResult)));
        }

        public static void SucceedsWithAll(Parser<IEnumerable<byte>> parser, IEnumerable<byte> input)
        {
            SucceedsWithMany(parser, input, input);
        }

        public static void SucceedsWith<T>(Parser<T> parser, IEnumerable<byte> input, Action<T> resultAssertion)
        {
            parser.TryParse(new MemoryStream(input.ToArray()))
                .IfFailure(f =>
                {
                    Assert.True(false, $"Parsing of \"{nameof(input)}\" failed unexpectedly. f");
                    return f;
                })
                .IfSuccess(s =>
                {
                    resultAssertion(s.Value);
                    return s;
                });
        }

        public static void SucceedsWith<T>(Parser<T> parser, Stream input, Action<T> resultAssertion)
        {
            parser.TryParse(input)
                .IfFailure(f =>
                {
                    Assert.True(false, $"Parsing of \"{nameof(input)}\" failed unexpectedly. f");
                    return f;
                })
                .IfSuccess(s =>
                {
                    resultAssertion(s.Value);
                    return s;
                });
        }

        public static void Fails<T>(Parser<T> parser, IEnumerable<byte> input)
        {
            FailsWith(parser, input, f => { });
        }

        public static void FailsAt<T>(Parser<T> parser, IEnumerable<byte> input, int position)
        {
            FailsWith(parser, input, f => Assert.Equal(position, f.Remainder.Position));
        }

        public static void FailsWith<T>(Parser<T> parser, Stream input, Action<IResult<T>> resultAssertion)
        {
            parser.TryParse(input)
                .IfSuccess(s =>
                {
                    Assert.True(false, $"Expected failure but succeeded with {s.Value}.");
                    return s;
                })
                .IfFailure(f =>
                {
                    resultAssertion(f);
                    return f;
                });
        }

        public static void FailsWith<T>(Parser<T> parser, IEnumerable<byte> input, Action<IResult<T>> resultAssertion)
        {
            parser.TryParse(new MemoryStream(input.ToArray()))
                .IfSuccess(s =>
                {
                    Assert.True(false, $"Expected failure but succeeded with {s.Value}.");
                    return s;
                })
                .IfFailure(f =>
                {
                    resultAssertion(f);
                    return f;
                });
        }
    }
}
