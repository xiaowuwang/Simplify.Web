﻿using System;
using System.Collections.Generic;
using Microsoft.Owin;
using Moq;
using NUnit.Framework;
using Simplify.Web.Modules;

namespace Simplify.Web.Tests.Modules
{
	[TestFixture]
	public class WebContextTests
	{
		private Mock<IOwinContext> _owinContext;

		[SetUp]
		public void Initialize()
		{
			_owinContext = new Mock<IOwinContext>();

			_owinContext.SetupGet(x => x.Response).Returns(new Mock<IOwinResponse>().Object);
			_owinContext.SetupGet(x => x.Request.PathBase).Returns(new PathString("/mywebsite"));
			_owinContext.SetupGet(x => x.Request.Path).Returns(new PathString("/"));
			_owinContext.SetupGet(x => x.Request.Uri).Returns(new Uri("http://localhost/mywebsite/"));
			_owinContext.SetupGet(x => x.Request.Query).Returns(new ReadableStringCollection(new Dictionary<string, string[]>()));
			_owinContext.SetupGet(x => x.Request.Headers)
				.Returns(new HeaderDictionary(new Dictionary<string, string[]>()));
		}

		[Test]
		public void Constructor_NormalContext_SetCorrectly()
		{
			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual(_owinContext.Object, context.Context);
			Assert.AreEqual(_owinContext.Object.Request, context.Request);
			Assert.AreEqual(_owinContext.Object.Response, context.Response);
			Assert.AreEqual(_owinContext.Object.Request.Query, context.Query);
			Assert.AreEqual("http://localhost/mywebsite/", context.SiteUrl);
			Assert.AreEqual("/mywebsite", context.VirtualPath);
			Assert.AreEqual("/", context.Route);
			Assert.IsFalse(context.IsAjax);
		}

		[Test]
		public void Constructor_PathWithPort_SetCorrectly()
		{
			// Assign

			_owinContext.SetupGet(x => x.Request.Uri).Returns(new Uri("http://localhost:8080"));
			_owinContext.SetupGet(x => x.Request.PathBase).Returns(new PathString(""));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual("http://localhost:8080/", context.SiteUrl);
			Assert.AreEqual("", context.VirtualPath);
		}

		[Test]
		public void Constructor_PathWithPortAndQueryString_SetCorrectly()
		{
			// Assign

			_owinContext.SetupGet(x => x.Request.Uri).Returns(new Uri("http://localhost:8080?act=test"));
			_owinContext.SetupGet(x => x.Request.PathBase).Returns(new PathString(""));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual("http://localhost:8080/", context.SiteUrl);
			Assert.AreEqual("", context.VirtualPath);
		}

		[Test]
		public void Constructor_VirtualPathWithPort_SetCorrectly()
		{
			// Assign
			_owinContext.SetupGet(x => x.Request.Uri).Returns(new Uri("http://localhost:8080/mywebsite/"));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual("http://localhost:8080/mywebsite/", context.SiteUrl);
			Assert.AreEqual("/mywebsite", context.VirtualPath);
		}

		[Test]
		public void Constructor_NoVirtualPath_Empty()
		{
			// Assign
			_owinContext.SetupGet(x => x.Request.PathBase).Returns(new PathString(""));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert
			Assert.AreEqual("", context.VirtualPath);
		}

		[Test]
		public void Constructor_LocalhostWithVirtualPathAndSegmentsWithQueryString_ParsedCorrectly()
		{
			// Assign

			_owinContext.SetupGet(x => x.Request.PathBase).Returns(new PathString("/mywebsite"));
			_owinContext.SetupGet(x => x.Request.Uri).Returns(new Uri("http://localhost/mywebsite/test?act=foo"));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual("http://localhost/mywebsite/", context.SiteUrl);
		}

		[Test]
		public void Constructor_NormalPath_ParsedCorrectly()
		{
			// Assign

			_owinContext.SetupGet(x => x.Request.PathBase).Returns(new PathString(""));
			_owinContext.SetupGet(x => x.Request.Uri).Returns(new Uri("http://mywebsite.com"));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual("http://mywebsite.com/", context.SiteUrl);
		}

		[Test]
		public void Constructor_NormalPathAndSegmentsWithQueryString_ParsedCorrectly()
		{
			// Assign

			_owinContext.SetupGet(x => x.Request.PathBase).Returns(new PathString(""));
			_owinContext.SetupGet(x => x.Request.Uri).Returns(new Uri("http://mywebsite.com/test/?act=foo"));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual("http://mywebsite.com/", context.SiteUrl);
		}

		[Test]
		public void Constructor_AjaxRequest_True()
		{
			// Assign
			_owinContext.SetupGet(x => x.Request.Headers)
				.Returns(new HeaderDictionary(new Dictionary<string, string[]> { { "X-Requested-With", new[] { "test" } } }));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.IsTrue(context.IsAjax);
		}

		[Test]
		public void Constructor_SpecificRoute_SetCorrectly()
		{
			// Assign
			_owinContext.SetupGet(x => x.Request.Path).Returns(new PathString("/test"));

			// Act
			var context = new WebContext(_owinContext.Object);

			// Assert

			Assert.AreEqual("/test", context.Route);
		}
	}
}