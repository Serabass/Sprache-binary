using System;
using System.Globalization;
using Xunit;

namespace Sprache.Tests
{
    [Obsolete("Don't use this")]
    public class DecimalTests : IDisposable
    {
        private CultureInfo _previousCulture;

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _previousCulture;
        }
    }
}