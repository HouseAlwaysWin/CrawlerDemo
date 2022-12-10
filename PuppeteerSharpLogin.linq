<Query Kind="Program">
  <NuGetReference>AngleSharp</NuGetReference>
  <NuGetReference>PuppeteerSharp</NuGetReference>
  <Namespace>PuppeteerSharp</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>AngleSharp.Html.Parser</Namespace>
</Query>

async Task Main()
{
	using var browserFetcher = new BrowserFetcher();
	await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
	var browser = await Puppeteer.LaunchAsync(new LaunchOptions
	{
		Headless = false
	});
	await using var page = await browser.NewPageAsync();
	await page.GoToAsync("http://www.eyny.com/member.php?mod=logging&action=login", null, new[] { PuppeteerSharp.WaitUntilNavigation.Networkidle0 });
	await page.TypeAsync("input[name=username]", "");
	await page.TypeAsync("input[name=password]", "");
	await page.ClickAsync("button[name=loginsubmit]");
	await page.WaitForNavigationAsync();
	var content = await page.GetContentAsync();
	var parser = new HtmlParser();
	var document = await parser.ParseDocumentAsync(content);
	var links = document.QuerySelectorAll("a");
	foreach (var link in links)
	{
		link.Attributes["href"].TextContent.Dump();
	}

	await browser.CloseAsync();

}

// You can define other methods, fields, classes and namespaces here