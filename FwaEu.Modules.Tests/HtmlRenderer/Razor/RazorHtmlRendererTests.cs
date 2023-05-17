using FwaEu.Modules.HtmlRenderer.Razor;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.HtmlRenderer.Razor
{
	public class UserModel
	{
		public string Name { get; set; }
	}

	[TestClass]
	public class RazorHtmlRendererTests
	{
		[TestMethod]
		public async Task RenderAsync()
		{
			const string Html =
@"<html>
    Your awesome template goes here, {0}
</html>";

			var razorTemplate =
$@"@model {typeof(UserModel).FullName}
" + String.Format(Html, "@Model.Name");

			var resolverStub = new HtmlTemplateResolverStub<UserModel>(razorTemplate);
			var razorHtmlRenderer = new RazorHtmlRenderer<UserModel>(resolverStub, new FakeHostEnvironment(), null, new MemoryCache(new MemoryCacheOptions()));

			var userName = "Rom1 Rom1";

			Assert.AreEqual(Html.Replace("{0}", userName),
				await razorHtmlRenderer.RenderAsync(new UserModel { Name = userName }, null));
		}
	}
}
