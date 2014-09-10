/// <summary>
/// This function adds supporting of array params for RouteValueDictionary
/// </summary>
/// <returns></returns>
public static MvcHtmlString ActionLinkEx(this HtmlHelper helper, string text, string action, string controller, object routeData, object htmlAttributes)
{
	var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
	string href = urlHelper.Action(action, controller);

	if (routeData != null)
	{
		var routeValues = routeData is RouteValueDictionary ? (RouteValueDictionary)routeData : new RouteValueDictionary(routeData);
		var urlParameters = new List<string>();
		foreach (var key in routeValues.Keys)
		{
			var value = routeValues[key];
			if (value != null)
			{
				if (value is IEnumerable && value.GetType() != typeof(string))
				{
					foreach (var val in (IEnumerable)value)
					{
						urlParameters.Add(String.Format("{0}={1}", key, val));
					}
				}
				else
				{
					urlParameters.Add(String.Format("{0}={1}", key, value));
				}
			}
		}
		var paramString = String.Join("&", urlParameters);
		if (!String.IsNullOrEmpty(paramString))
		{
			href += "?" + paramString;
		}
	}

	var builder = new TagBuilder("a");
	builder.Attributes.Add("href", href);
	builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
	builder.SetInnerText(text);
	return new MvcHtmlString(builder.ToString(TagRenderMode.Normal));
}
