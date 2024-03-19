using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericImporter
{
	// NOTA: Los valores de las enumeraciones a continuación deben tener el tercer bit establecido en 1 para ser considerados como IsKey
	[Flags]
	public enum IsKeyValue
	{
		False = 0b000,
		True = 0b0010,
		TrueAllowNull = 0b0011
	}
}
