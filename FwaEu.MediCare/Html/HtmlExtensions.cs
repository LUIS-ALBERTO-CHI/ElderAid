using FwaEu.Modules.HtmlRenderer.Razor;
using FwaEu.Modules.Mail.HtmlMailSender;
using FwaEu.MediCare.Html.Mail;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Html
{
	public static class HtmlMailsExtensions
	{
		public static IServiceCollection AddApplicationHtml(this IServiceCollection services)
		{
			services.AddSingleton<IHtmlRazorLayoutPathResolver>(sp => new DefaultHtmlRazorLayoutPathResolver("Html/Mail/_Layout"));
			services.AddTransient<IHtmlMailParametersProvider, DefaultHtmlMailParametersProvider>();
			return services;
		}
	}
}
