using FwaEu.Fwamework;
using FwaEu.Modules.HtmlRenderer.Razor;
using FwaEu.ElderAid.Html.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.ElderAid.Html
{
	public static class HtmlMailsExtensions
	{
		public static IServiceCollection AddApplicationHtml(this IServiceCollection services, ApplicationInitializationContext context)
		{
			services.AddSingleton<IHtmlRazorLayoutPathResolver>(sp => new DefaultHtmlRazorLayoutPathResolver("Html/Mail/_Layout"));
			return services.AddHtmlMailExtensions(context);
		}
	}
}
