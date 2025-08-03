using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CleanArchitecture.WebUiTests
{
    public class AutomatedUITests: IDisposable
    {
        private readonly ChromeDriver _driver;

        public AutomatedUITests() => _driver = new ChromeDriver();

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void Display_WhenOpenHomePage_ReturnsHomePage()
        {
            // Act
            _driver.Navigate().GoToUrl("https://localhost:7258");

            // Assert
            Assert.Equal("Home", _driver.Title);
            Assert.Contains("Hello, world!", _driver.PageSource);
        }
    }
}
