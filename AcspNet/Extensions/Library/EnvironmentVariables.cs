﻿using System;
using System.Globalization;
using System.Threading;
using System.Web;

namespace AcspNet.Extensions.Library
{
	/// <summary>
	/// Site environment variables, by default initialized from <see cref="EngineSettings" />
	/// </summary>
	[Priority(-9)]
	[Version("1.0.8")]
	public sealed class EnvironmentVariables : ILibExtension
	{
		/// <summary>
		/// Language field name in user cookies
		/// </summary>
		public const string CookieLanguageFieldName = "language";

		private Manager _manager;
		private string _language = "";
	
		/// <summary>
		/// Initializes the library extension.
		/// </summary>
		/// <param name="manager">The manager.</param>
		public void Initialize(Manager manager)
		{
			_manager = manager;

			var cookieLanguage = manager.Request.Cookies[CookieLanguageFieldName];

			SetCurrentLanguage(cookieLanguage != null && !string.IsNullOrEmpty(cookieLanguage.Value) ? cookieLanguage.Value : EngineSettings.DefaultLanguage);

			//TemplatesPath = EngineSettings.DefaultTemplatesPath;
			//SiteStyle = EngineSettings.DefaultSiteStyle;
		}

		/// <summary>
		/// Site current templates relative path
		/// </summary>
		public string TemplatesPath { get; set; }

		/// <summary>
		/// Site current templates physical path
		/// </summary>
		public string TemplatesPhysicalPath
		{
			get
			{
				return Manager.SitePhysicalPath + TemplatesPath;
			}
		}

		/// <summary>
		/// Site current style
		/// </summary>
		public string SiteStyle { get; set; }

		/// <summary>
		/// Site current language, for example: "en", "ru", "de" etc.
		/// </summary>
		public string Language
		{
			get { return _language; }
		}

		/// <summary>
		/// Set site current and cookie language value
		/// </summary>
		/// <param name="language">language code</param>
		public void SetLanguage(string language)
		{
			if (language == "kz")
				language = "kk";

			SetCurrentLanguage(language);
			SetCookieLanguage(language);
		}

		/// <summary>
		/// Set site cookie language value
		/// </summary>
		/// <param name="language">Language code</param>
		public void SetCookieLanguage(string language)
		{
			if (string.IsNullOrEmpty(language))
				return;

			var cookie = new HttpCookie(CookieLanguageFieldName, language)
			{
				Expires = DateTime.Now.AddYears(5)
			};

			_manager.Response.Cookies.Add(cookie);
		}

		/// <summary>
		/// Set language for current request
		/// </summary>
		/// <param name="language">Language code</param>
		public void SetCurrentLanguage(string language)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			Thread.CurrentThread.CurrentCulture = new CultureInfo(language);

			_language = language;
		}
	}
}