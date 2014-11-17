using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Common
{
    public static class HtmlHelpers
    {
        public static IHtmlString RadioList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> list,
            string separator = null, IDictionary<string, object> htmlAttributes = null)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            var fullname = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullname))
                throw new ArgumentException("The argument must have a value", "fullname");

            var sb = new StringBuilder();
            var i = 0;

            foreach (var item in list)
            {
                if (string.IsNullOrWhiteSpace(item.Value) || string.IsNullOrWhiteSpace(item.Text))
                    continue; //blank radio box?

                var radio = new TagBuilder("input");
                radio.MergeAttribute("type", "radio");
                radio.MergeAttribute("id", fullname + "_" + i);
                radio.MergeAttribute("value", item.Value);
                radio.MergeAttribute("name", fullname, true);

                if (item.Selected)
                    radio.MergeAttribute("checked", "checked");

                var label = new TagBuilder("label");
                label.MergeAttributes(htmlAttributes);
                label.InnerHtml = radio.ToString(TagRenderMode.SelfClosing) + " " + item.Text;

                sb.Append(label.ToString(TagRenderMode.Normal) + separator);
                i++;
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString MultiSelect(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes = null)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var sb = new StringBuilder();
            foreach (var item in items)
            {
                var label = new TagBuilder("label");
                //label.MergeAttributes(htmlAttributes);

                var checkbox = new TagBuilder("input");
                checkbox.MergeAttribute("name", fullName, true);
                checkbox.MergeAttribute("type", "checkbox");
                checkbox.MergeAttribute("value", item.Value);
                //checkbox.MergeAttributes(htmlAttributes);

                if (item.Selected)
                    checkbox.MergeAttribute("checked", "checked");

                label.InnerHtml = checkbox.ToString(TagRenderMode.SelfClosing) + " " + item.Text;
                sb.AppendLine("<div class='checkbox'>" + label.ToString(TagRenderMode.Normal) + "</div>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString SingleSelect(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes = null)
        {
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var sb = new StringBuilder();
            foreach (var item in items)
            {
                var label = new TagBuilder("label");
                //label.MergeAttributes(htmlAttributes);

                var checkbox = new TagBuilder("input");
                checkbox.MergeAttribute("name", fullName, true);
                checkbox.MergeAttribute("type", "radio");
                checkbox.MergeAttribute("value", item.Value);
                //checkbox.MergeAttributes(htmlAttributes);

                if (item.Selected)
                    checkbox.MergeAttribute("checked", "checked");

                label.InnerHtml = checkbox.ToString(TagRenderMode.SelfClosing) + " " + item.Text;
                sb.AppendLine("<div class='radio'>" + label.ToString(TagRenderMode.Normal) + "</div>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<string> items, bool blank = false, params string[] selected)
        {
            var list = new List<SelectListItem>();

            if (items == null)
                return list;

            list.AddRange(items.Where(i => i != null).Select(item => new SelectListItem
                {
                    Value = item,
                    Text = item,
                    Selected = selected.Contains(item)
                }));

            if (blank)
                list.Insert(0, new SelectListItem());

            return list;
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IDictionary<string, string> items, bool blank = false, params string[] selected)
        {
            var list = new List<SelectListItem>();

            if (items == null)
                return list;

            list.AddRange(items.Select(item => new SelectListItem
                {
                    Value = item.Key,
                    Text = item.Value,
                    Selected = selected.Contains(item.Key),
                }));

            if (blank)
                list.Insert(0, new SelectListItem());

            return list;
        }

        public static IDictionary<string, object> AddClass(this IDictionary<string, object> dict, params string[] classes)
        {
            if (dict == null)
                return null;

            var existing = dict.ContainsKey("class")
                               ? (dict["class"] ?? string.Empty).ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                               : new List<string>();

            existing.AddRange(classes);

            dict["class"] = String.Join(" ", existing.Distinct());

            return dict;
        }

        public static IDictionary<string, object> RemoveClass(this IDictionary<string, object> dict, params string[] classes)
        {
            if (dict == null)
                return null;

            var existing = dict.ContainsKey("class")
                               ? (dict["class"] ?? string.Empty).ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                               : new List<string>();

            existing.RemoveAll(classes.Contains);

            dict["class"] = String.Join(" ", existing.Distinct());

            return dict;
        }
    };
}
