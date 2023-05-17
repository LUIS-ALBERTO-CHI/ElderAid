﻿using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FwaEu.Fwamework.Tests.Configuration
{
	public class HostEnvironmentFake : IHostEnvironment
	{
		public string ApplicationName
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public IFileProvider ContentRootFileProvider
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public string ContentRootPath
		{
			get => Path.GetFullPath("/");
			set => throw new NotImplementedException();
		}

		public string EnvironmentName
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
	}
}
