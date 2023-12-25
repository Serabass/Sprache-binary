using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sprache.Tests.Scenarios
{
    public static class AssemblerParser
    {
    }

    [Obsolete("Don't use this")]
    public class AssemblerLine
    {
        public readonly string Label;
        public readonly string InstructionName;
        public readonly string[] Operands;
        public readonly string Comment;

        public AssemblerLine(string label, string instructionName, string[] operands, string comment)
        {
            Label = label;
            InstructionName = instructionName;
            Operands = operands;
            Comment = comment;
        }

        protected bool Equals(AssemblerLine other)
        {
            return ToString() == other.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AssemblerLine)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InstructionName != null ? InstructionName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Operands != null ? Operands.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Comment != null ? Comment.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Join(" ",
                Label == null ? "" : (Label + ":"),
                InstructionName == null ? "" : InstructionName + string.Join(", ", Operands),
                Comment == null ? "" : ";" + Comment);
        }
    }
}