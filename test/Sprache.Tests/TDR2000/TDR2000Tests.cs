using Xunit;

namespace SpracheBinary.Tests.TDR2000
{
  public class TDR2000Tests
  {
    [Theory]
    public void ReadDirTest()
    {
      using DirParser p = new DirParser(@"C:\Program Files (x86)\Новый Диск\Кармагеддон. Колеса смерти\Assets\Sound\COMMON")
        .Init();

      var blocks = p.Init();
      Assert.Equal(1, 1);
    }
  }
}
