using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CleanArchitecture.WebUiTests
{
    public class AutomatedUITests: IDisposable
    {
        private readonly IWebDriver _driver;

        public AutomatedUITests() => _driver = new ChromeDriver();

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
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
