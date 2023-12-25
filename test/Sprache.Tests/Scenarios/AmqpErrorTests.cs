// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Sprache.Tests.Scenarios
{
    public class AmqpErrorTests
    {
    }

    public class AmqpErrorItem
    {
    }

    public class AmqpStringItem : AmqpErrorItem
    {
        public string Text { get; }

        public AmqpStringItem(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class KeyValue : AmqpErrorItem
    {
        public Value Value { get; }
        public string Key { get; }

        public KeyValue(string key, Value value)
        {
            Value = value;
            Key = key;
        }

        public override string ToString()
        {
            return string.Format("Key: '{0}', Value: '{1}'", Key, Value);
        }
    }

    public class Value
    {
        
    }

    public class StringValue : Value
    {
        public string Text { get; }

        public StringValue(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class IntValue : Value
    {
        public int Value { get; }

        public IntValue(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}

// ReSharper restore InconsistentNaming