using Xunit;

namespace LibYear.Tests
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