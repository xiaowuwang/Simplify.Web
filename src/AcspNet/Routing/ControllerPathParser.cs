﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AcspNet.Routing
{
	public class ControllerPathParser : IControllerPathParser
	{
		public IControllerPath Parse(string controllerPath)
		{
			var items = controllerPath.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
			var pathItems = new List<IPathItem>();

			foreach (var item in items)
			{
				if (item.Contains("{") || item.Contains("}") || item.Contains(":"))
				{
					//var matches = Regex.Matches(item, @"^{([.]+)|([.]+:[.]+])}$");
					var matches = Regex.Matches(item, @"^{.+}$");

					if (matches.Count == 0)
						throw new ControllerRouteException("Bad controller path: " + controllerPath);

					var subitem = item.Substring(1, item.Length - 2);

					if (subitem.Contains("{") || subitem.Contains("}"))
						throw new ControllerRouteException("Bad controller path: " + controllerPath);

					if (subitem.Contains(":"))
					{
						var parameterData = subitem.Split(':');
						var type = ParseParameterType(parameterData[1]);

						if (type == null)
							throw new ControllerRouteException(string.Format("Undefined controller parameter type '{0}', path: {1}",
								parameterData[1], controllerPath));

						pathItems.Add(new PathParameter(parameterData[0], type));
					}
					else
						pathItems.Add(new PathParameter(subitem, typeof (string)));
				}
				else
					pathItems.Add(new PathSegment(item));
			}

			return new ControllerPath(pathItems);
		}

		private Type ParseParameterType(string typeData)
		{
			if (typeData == "int")
				return typeof (int);

			return null;
		}
	}
}