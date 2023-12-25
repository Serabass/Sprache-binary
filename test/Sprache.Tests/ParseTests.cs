using System;
using System.Linq;
using Xunit;

namespace SpracheBinary.Tests
{
    public class ParseTests
    {
        [Fact]
        public void Parser_OfByte_AcceptsThatByte()
        {
            AssertParser.SucceedsWithOne(Parse.Byte(0).Once(), new byte[] { 0 } , (byte)0);
        }

        [Fact]
        public void Parser_OfChar_AcceptsOnlyOneChar()
        {
            AssertParser.SucceedsWithOne(Parse.Byte(0).Once(), new byte[] { 0, 0, 0 }, (byte)0);
        }

        [Fact]
        public void Parser_OfChar_DoesNotAcceptNonMatchingChar()
        {
            AssertParser.FailsAt(Parse.Byte(0).Once(), new byte[] { 1, 0, 0 }, 0);
        }

        [Fact]
        public void Parser_OfChar_DoesNotAcceptEmptyInput()
        {
            AssertParser.Fails(Parse.Byte(0).Once(), Array.Empty<byte>());
        }

        [Fact]
        public void Parser_OfChars_AcceptsAnyOfThoseChars()
        {
            var parser = Parse.Bytes(0, 1, 2).Once();
            AssertParser.SucceedsWithOne(parser, new byte[] {0}, (byte)0);
            AssertParser.SucceedsWithOne(parser, new byte[] {1}, (byte)1);
            AssertParser.SucceedsWithOne(parser, new byte[] {2}, (byte)2);
        }

        [Fact]
        public void Parser_OfChars_UsingString_AcceptsAnyOfThoseChars()
        {
            var parser = Parse.Bytes(new byte[] {0, 1, 2}).Once();
            AssertParser.SucceedsWithOne(parser, new byte[] {0}, (byte)0);
            AssertParser.SucceedsWithOne(parser, new byte[] {1}, (byte)1);
            AssertParser.SucceedsWithOne(parser, new byte[] {2}, (byte)2);
        }

        [Fact]
        public void Parser_OfManyChars_AcceptsEmptyInput()
        {
            AssertParser.SucceedsWithAll(Parse.Byte(0).Many(), Array.Empty<byte>());
        }

        [Fact]
        public void Parser_OfManyChars_AcceptsManyChars()
        {
            AssertParser.SucceedsWithAll(Parse.Byte(0).Many(), new byte[] {0, 0, 0});
        }

        [Fact]
        public void Parser_OfAtLeastOneChar_DoesNotAcceptEmptyInput()
        {
            AssertParser.Fails(Parse.Byte(0).AtLeastOnce(), Array.Empty<byte>());
        }

        [Fact]
        public void Parser_OfAtLeastOneChar_AcceptsOneChar()
        {
            AssertParser.SucceedsWithAll(Parse.Byte(0).AtLeastOnce(), new byte[] {0});
        }

        [Fact]
        public void Parser_OfAtLeastOneChar_AcceptsManyChars()
        {
            AssertParser.SucceedsWithAll(Parse.Byte(0).AtLeastOnce(), new byte[] {0, 0, 0});
        }

        [Fact]
        public void ConcatenatingParsers_ConcatenatesResults()
        {
            var p = Parse.Byte(0).Once().Then(a =>
                Parse.Byte(1).Once().Select(b => a.Concat(b)));
            AssertParser.SucceedsWithAll(p, new byte[] {0, 1});
        }

        [Fact]
        public void ReturningValue_DoesNotAdvanceInput()
        {
            var p = Parse.Return(1);
            AssertParser.SucceedsWith(p, new byte[] {0, 1, 2}, n => Assert.Equal(1, n));
        }

        [Fact]
        public void ReturningValue_ReturnsValueAsResult()
        {
            var p = Parse.Return(1);
            var r = (Result<int>)p.TryParse(new byte[] {0, 1, 2});
            Assert.Equal(0, r.Remainder.Position);
        }

        [Fact]
        public void CanSpecifyParsersUsingQueryComprehensions()
        {
            var p = from a in Parse.Byte(0).Once()
                    from bs in Parse.Byte(1).Many()
                    from cs in Parse.Byte(2).AtLeastOnce()
                    select a.Concat(bs).Concat(cs);

            AssertParser.SucceedsWithAll(p, new byte[] {0, 1, 1, 1, 2,});
        }

        [Fact]
        public void WhenFirstOptionSucceedsButConsumesNothing_SecondOptionTried()
        {
            var p = Parse.Byte(0).Many().XOr(Parse.Byte(1).Many());
            AssertParser.SucceedsWithAll(p, new byte[] {1, 1, 1});
        }

        [Fact]
        public void WithXOr_WhenFirstOptionFailsAndConsumesInput_SecondOptionNotTried()
        {
            var first = Parse.Byte(0).Once().Concat(Parse.Byte(1).Once());
            var second = Parse.Byte(0).Once();
            var p = first.XOr(second);
            AssertParser.FailsAt(p, new byte[] { 0 }, 1);
        }

        [Fact]
        public void WithOr_WhenFirstOptionFailsAndConsumesInput_SecondOptionTried()
        {
            var first = Parse.Byte(0).Once().Concat(Parse.Byte(1).Once());
            var second = Parse.Byte(0).Once();
            var p = first.Or(second);
            AssertParser.SucceedsWithAll(p, new byte[] { 0 });
        }

        [Fact]
        public void ParsesString_AsSequenceOfChars()
        {
            var p = Parse.ByteSequence(new byte[] { 0, 1, 2 });
            AssertParser.SucceedsWithAll(p, new byte[] { 0, 1, 2 });
        }

        [Fact]
        public void WithMany_WhenLastElementFails_FailureReportedAtLastElement()
        {
            var ab = from a in Parse.Byte(0)
                     from b in Parse.Byte(1)
                     select new byte[] { 0, 1 };

            var p = ab.Many().End();

            AssertParser.FailsAt(p, new byte[] { 0, 1, 0, 1, 0, 2 }, 4);
        }
    
        [Fact]
        public void WithXMany_WhenLastElementFails_FailureReportedAtLastElement()
        {
            var ab = from a in Parse.Byte(0)
                     from b in Parse.Byte(1)
                     select new byte[] { 0, 1 };

            var p = ab.XMany().End();

            AssertParser.FailsAt(p, new byte[] { 0, 1, 0, 1, 0, 2 }, 5);
        }
    }
}
