using System;
using Xunit;

namespace Sprache.Tests
{
    public class InputTests
    {
        [Fact]
        public void InputsOnTheSameString_AtTheSamePosition_AreEqual()
        {
            var s = new byte[] { 1, 2, 3, 4, 5 };
            var p = 2;
            var i1 = new Input(s, p);
            var i2 = new Input(s, p);
            Assert.Equal(i1, i2);
            Assert.True(i1 == i2);
        }

        [Fact]
        public void InputsOnTheSameString_AtDifferentPositions_AreNotEqual()
        {
            var s = new byte[] { 1, 2, 3, 4, 5 };
            var i1 = new Input(s, 1);
            var i2 = new Input(s, 2);
            Assert.NotEqual(i1, i2);
            Assert.True(i1 != i2);
        }

        [Fact]
        public void InputsOnDifferentStrings_AtTheSamePosition_AreNotEqual()
        {
            var p = 2;
            var i1 = new Input(new byte[] { 1, 2, 3, 4, 5 }, p);
            var i2 = new Input(new byte[] { 1, 2, 3, 4, 5 }, p);
            Assert.NotEqual(i1, i2);
        }

        [Fact]
        public void InputsAtEnd_CannotAdvance()
        {
            var i = new Input(Array.Empty<byte>(), 0);
            Assert.True(i.AtEnd);
            Assert.Throws<InvalidOperationException>(() => i.Advance());
        }

        [Fact]
        public void AdvancingInput_MovesForwardOneCharacter()
        {
            var i = new Input(new byte[] { 1, 2, 3, 4, 5 }, 1);
            var j = i.Advance();
            Assert.Equal(2, j.Position);
        }

        [Fact]
        public void CurrentCharacter_ReflectsPosition()
        {
            var i = new Input(new byte[] { 1, 2, 3, 4, 5 }, 1);
            Assert.Equal(1, i.Current);
        }

        [Fact]
        public void ANewInput_WillBeAtFirstCharacter()
        {
            var i = new Input(new byte[] { 1, 2, 3, 4, 5 });
            Assert.Equal(0, i.Position);
        }
    }
}
