using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace FwaEu.Modules.HtmlRenderer.Razor
{
	public class RazorHtmlRenderer<TModel> : IHtmlRenderer<TModel>
	{
		private readonly IRazorTemplateResolver<TModel> _razorTemplateResolver;
		private readonly IHostEnvironment _hostEnvironment;
		private readonly IHtmlRazorLayoutPathResolver _htmlMailRazorLayoutPathResolver;
		private readonly IMemoryCache _memoryCache;
		public static readonly string LayoutPathKey = "LayoutPath";

		public RazorHtmlRenderer(IRazorTemplateResolver<TModel> razorTemplateResolver, IHostEnvironment hostEnvironment,
			IHtmlRazorLayoutPathResolver htmlMailRazorLayoutPathResolver, IMemoryCache memoryCache)
		{
			_razorTemplateResolver = razorTemplateResolver ?? throw new ArgumentNullException(nameof(razorTemplateResolver));
			_hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
			_htmlMailRazorLayoutPathResolver = htmlMailRazorLayoutPathResolver;
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
		}

		protected ExpandoObject ConvertDictionaryToExpandoObject(IDictionary<string, object> dictionary)
		{
			var expandoObject = new ExpandoObject();

			if (dictionary != null)
			{
				foreach (var keyValuePair in dictionary)
				{
					expandoObject.TryAdd(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return expandoObject;
		}

		public async Task<string> RenderAsync(TModel model, IDictionary<string, object> parameters)
		{
			var engine = new RazorLightEngineBuilder().UseFileSystemProject(_hostEnvironment.ContentRootPath).Build();
			var expandoObject = ConvertDictionaryToExpandoObject(parameters);

			if (_htmlMailRazorLayoutPathResolver != null)
			{
				expandoObject.TryAdd(LayoutPathKey, _htmlMailRazorLayoutPathResolver.GetLayoutPath(model));
			}

			var cacheKey = typeof(TModel);
			var template = await _memoryCache.GetOrCreateAsync(cacheKey, async cacheEntry =>
			{
				cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
				return await _razorTemplateResolver.GetTemplateAsync();
			});
			return await engine.CompileRenderStringAsync(nameof(TModel), template, model, expandoObject);
		}
	}
}
