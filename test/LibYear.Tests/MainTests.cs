using Xunit;

namespace LibYear.App.Tests
{
    public class MainTests
    {
        [Fact]
        public void AppDoesntCrashWithFunkyData()
        {
            Program.Main(new string[0]);
        }
    }
}