﻿using AcspNet.Modules;
using Simplify.DI;

namespace AcspNet.Core
{
	/// <summary>
	/// Provides builder for ActionModulesAccessor objects
	/// </summary>
	public class ActionModulesAccessorBuilder : ModulesAccessorBuilder
	{
		/// <summary>
		/// Builds the modules accessor properties.
		/// </summary>
		/// <param name="modulesAccessor">The modules accessor.</param>
		/// <param name="containerProvider">The DI container provider.</param>
		protected void BuildActionModulesAccessorProperties(ActionModulesAccessor modulesAccessor, IDIContainerProvider containerProvider)
		{
			BuildModulesAccessorProperties(modulesAccessor, containerProvider);

			modulesAccessor.Context = containerProvider.Resolve<IAcspNetContextProvider>().Get();
			modulesAccessor.DataCollector = containerProvider.Resolve<IDataCollector>();
			modulesAccessor.Redirector = containerProvider.Resolve<IRedirector>();
			modulesAccessor.LanguageManager = containerProvider.Resolve<ILanguageManagerProvider>().Get();
			modulesAccessor.FileReader = containerProvider.Resolve<IFileReader>();
		}
	}
}