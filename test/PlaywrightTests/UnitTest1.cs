using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

public class Tests : PageTest
{
    [SetUp]
    public async Task SetUpAsync()
    {
        await Page.GotoAsync("http://minitwit-web:8080/public");
    }

    [Test]
    public async Task HomePageHasMiniTwitInTitle()
    {
        // find h1 element with text "MiniTwit"
        var title = await Page.TextContentAsync("h1");
        Assert.That(title, Is.EqualTo("MiniTwit"));
    }

    [Test]
    public void TestNavigationPanelVisibility()
    {
        // Wait for the navigation links to appear
        var publicLink = Page.GetByRole(AriaRole.Link, new() { Name = "public timeline" });
        var signUpLink = Page.GetByRole(AriaRole.Link, new() { Name = "sign up" });
        var signInLink = Page.GetByRole(AriaRole.Link, new() { Name = "sign in" });

        // Check if all navigation links are visible
        var publicLinkVisible = publicLink != null;
        var signUpLinkVisible = signUpLink != null;
        var signInLinkVisible = signInLink != null;

        // Assert that all navigation links are visible
        Assert.IsTrue(publicLinkVisible, "Public timeline link should be visible");
        Assert.IsTrue(signUpLinkVisible, "Sign up link should be visible");
        Assert.IsTrue(signInLinkVisible, "Sign in link should be visible");
    }
}