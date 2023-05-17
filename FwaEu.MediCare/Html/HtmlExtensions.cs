using FwaEu.Fwamework;
using FwaEu.Modules.HtmlRenderer.Razor;
using FwaEu.MediCare.Html.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace FwaEu.MediCare.Html
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
