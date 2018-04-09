using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonMark;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

public static class HtmlExtensions
{
    /// <summary>
    /// When viewData is null, we just return null.  Otherwise, we
    ///     convert the viewData collection to a ViewDataDictionary
    /// </summary>
    /// <param name="htmlHelper">HtmlHelper provided by view</param>
    /// <param name="viewData">Anonymous view data object</param>
    /// <returns></returns>
    public static ViewDataDictionary vd<T>(this IHtmlHelper<T> htmlHelper, object viewData)
    {
        if (viewData == null) return null;

        IDictionary<string, object> dict = viewData.ToDictionary();

        //We build the ViewDataDictionary from scratch, because the
        //  object parameter constructor for ViewDataDictionary doesn't
        //  seem to work...
        var vd = new ViewDataDictionary(htmlHelper.ViewData);
        foreach (var item in dict)
        {
            vd[item.Key] = item.Value;
        }

        return vd;
    }

    /// <summary>
    /// Use this extension method to create a dictionary or objects
    ///     keyed by their property name from a given container object
    /// </summary>
    /// <param name="o">Anonymous name value pair object</param>
    /// <returns></returns>
    public static Dictionary<string, object> ToDictionary(this object o)
    {
        var dictionary = new Dictionary<string, object>();

        foreach (var propertyInfo in o.GetType().GetProperties())
        {
            if (propertyInfo.GetIndexParameters().Length == 0)
            {
                dictionary.Add(propertyInfo.Name, propertyInfo.GetValue(o, null));
            }
        }

        return dictionary;
    }
}

namespace Extensions
{
    [HtmlTargetElement("markdown", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement(Attributes = "markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        public ModelExpression Content { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagName == "markdown")
            {
                output.TagName = null;
            }
            output.Attributes.RemoveAll("markdown");

            var content = await GetContent(output);
            var markdown = content;
            var html = CommonMarkConverter.Convert(markdown);
            output.Content.SetHtmlContent(html ?? "");
        }

        private async Task<string> GetContent(TagHelperOutput output)
        {
            if (Content == null)
                return (await output.GetChildContentAsync()).GetContent();

            return Content.Model?.ToString();
        }
    }
}