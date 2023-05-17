using FwaEu.Modules.AssemblyProvider;
using System.Reflection;

namespace FwaEu.MediCare.AssemblyProvider
{
	public class FwaEuMediCareAssemblyProvider : IAssemblyProvider
	{
		public Assembly GetAssembly()
		{
			return typeof(FwaEuMediCareAssemblyProvider).Assembly;
		}
	}
}
