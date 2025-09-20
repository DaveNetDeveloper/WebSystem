using TechTalk.SpecFlow;
using FluentAssertions;
using System.Text.Json;

namespace Test.UI.StepsDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
         
        private List<LoginData> loginList;

        protected class LoginData 
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Expected { get; set; }
        }

        [BeforeScenario] 
        public async Task Setup()
        {

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Args = new[] { "--disable-logging" } // fuerza menos logs
            });

            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();
        } 

        [Given(@"the user enters ""(.*)"" and ""(.*)""")]
        public async Task GivenTheUserEntersAnd(string username, string password)
        {
            TestContext.WriteLine($"Usuario: {username}, Contraseña: {password}");

            await _page.GotoAsync("https://practicetestautomation.com/practice-test-login", new PageGotoOptions { 
                Timeout = 30000
            });

            var json = await File.ReadAllTextAsync("TestData/logins.json");
            loginList = JsonSerializer.Deserialize<List<LoginData>>(json);

            foreach (var login in loginList) {
                var _userName = login.Username;
                var _password = login.Password;
                var _expected = login.Expected;
            } 
        }

        [When(@"they press login")]
        public async Task WhenTheyPressLogin()
        {
            TestContext.WriteLine("Presionando login");

            //FIX, no existen los campos en la web de ejemplo

            await _page.FillAsync("id=username", loginList[0].Username); 
            await _page.FillAsync("id=password", loginList[0].Password);
            await _page.ClickAsync("button[id=submit]");
        }

        [Then(@"they should see the homepage")]
        public async Task ThenTheyShouldSeeTheHomepage()
        {
            TestContext.WriteLine("Login Ok");
              
            _page.Url.Should().Contain("/logged-in-successfully");

            var title = await _page.InnerTextAsync(".post-title");
            title.Should().Be("Logged In Successfully");

            var content = await _page.ContentAsync();
            content.Should().Contain("Congratulations student. You successfully logged in!");

        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await _browser?.CloseAsync();
            _playwright?.Dispose();
        }
    }
}