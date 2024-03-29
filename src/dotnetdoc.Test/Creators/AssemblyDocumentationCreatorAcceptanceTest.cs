﻿using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading;
using dotnetdoc.Creators;
using dotnetdoc.TestTypes;
using FluentAssertions;
using NUnit.Framework;

namespace dotnetdoc.Test.Creators
{
	[TestFixture]
	public sealed class AssemblyDocumentationCreatorAcceptanceTest
	{
		private InMemoryFilesystem _filesystem;
		private AssemblyDocumentationReader _documentationReader;
		private AssemblyDocumentationCreator _assemblyDocumentationCreator;

		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_documentationReader = new AssemblyDocumentationReader(typeof(EmptyType).Assembly);
		}

		[SetUp]
		public void Setup()
		{
			_filesystem = new InMemoryFilesystem();
			_assemblyDocumentationCreator = new AssemblyDocumentationCreator(typeof(EmptyType).Assembly);
		}

		[TearDown]
		public void TearDown()
		{}

		[Test]
		[Ignore("Not yet fully implemented")]
		[Description("Verifies that an empty index is created when no types are documented")]
		public void TestIndexGenerationNoFiles()
		{
			Render();
			GetIndex().Should().Be("");
		}

		[Pure]
		private string GetIndex()
		{
			return Encoding.UTF8.GetString(_filesystem.ReadAllBytes("Documentation/README.md"));
		}

		private void Render()
		{
			_assemblyDocumentationCreator.RenderTo(_filesystem, ".");
		}
	}
}