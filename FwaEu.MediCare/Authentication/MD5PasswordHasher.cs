using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Security;

namespace FwaEu.MediCare.Authentication
{
	public class MD5PasswordHasher : IPasswordHasher
	{
		public string Hash(string password)
		{
            return MD5Hasher.ToMD5String(password);
		}
	}
}
