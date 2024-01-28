using System;

namespace FwaEu.ElderAid.Initialization
{
	public class ConnectionStringAttribute : Attribute
	{
		public ConnectionStringAttribute(string name)
		{
			this.Name = name;
		}

		public string Name { get; }
	}
}
