using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PagedList;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Mvc.Ajax;
using Wardship.Models;

namespace Wardship.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Set Focus on a web page control at page load
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="Control">Control to set focus on at page load</param>
        /// <returns></returns>
        public static MvcHtmlString SetFocus<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> Control)
        {
            var propertyName = ExpressionHelper.GetExpressionText(Control);
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\">document.getElementById('{0}').focus()</script>", propertyName));
        }

        /// <summary>
        /// Set Focus on one of two controls, used primarily for logon and password.
        /// If the primary object has a value, focus is set on the secondary object
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="primary">First object to focus on</param>
        /// <param name="secondary">Object to focus on if primary control has data</param>
        /// <returns></returns>
        public static MvcHtmlString SetFocus<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> primary, Expression<Func<TModel, TProperty>> secondary)
        {
            var value = ModelMetadata.FromLambdaExpression(primary, helper.ViewData).Model;

            var propertyName = "";
            if (value == null)
            {
                propertyName = ExpressionHelper.GetExpressionText(primary);
            }
            else
            {
                propertyName = ExpressionHelper.GetExpressionText(secondary);
            }

            //var propertyName = ExpressionHelper.GetExpressionText(primary);
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\">document.getElementById('{0}').focus()</script>", propertyName));
        }

        public static MvcHtmlString SortHeader(this HtmlHelper htmlHelper, ListViewModel Model, string sortValue, string sortButtonText)
        {
            TagBuilder headerCell = new TagBuilder("th");
            TagBuilder form = new TagBuilder("form");
            //action="/Warrant/IndexNew" method="post"//
            form.Attributes.Add("method", "post");
            form.Attributes.Add("action", HttpContext.Current.Request.CurrentExecutionFilePath);
            TagBuilder button = new TagBuilder("input");
            button.MergeAttribute("type", "Submit");
            button.MergeAttribute("value", sortButtonText);
            button.AddCssClass("sortButton");

            TagBuilder includeFinal = new TagBuilder("input");
            includeFinal.Attributes.Add("id", "includeCheck");
            includeFinal.Attributes.Add("name", "includeCheck");
            includeFinal.MergeAttribute("type", "hidden");
            includeFinal.MergeAttribute("value", Model.includeCheck.ToString());

            TagBuilder sortOrder = new TagBuilder("input");
            sortOrder.Attributes.Add("id", "sortOrder");
            sortOrder.Attributes.Add("name", "sortOrder");
            sortOrder.MergeAttribute("type", "hidden");

            if (Model.sortOrder != null && Model.sortOrder.Contains(sortValue))
            {
                if (Model.sortOrder.EndsWith("asc"))
                {
                    sortOrder.MergeAttribute("value", sortValue + " desc");
                    button.AddCssClass("asc");
                }
                else
                {
                    sortOrder.MergeAttribute("value", sortValue + " asc");
                    button.AddCssClass("desc");
                }
            }
            else
            {
                sortOrder.MergeAttribute("value", sortValue + " asc");
            }

            form.InnerHtml += sortOrder;
            form.InnerHtml += includeFinal;
            form.InnerHtml += button;
            headerCell.InnerHtml = form.ToString();
            return MvcHtmlString.Create(headerCell.ToString());
        }
        #region CBHelpers
        public static MvcHtmlString DropDownList(this HtmlHelper helper,
            string name, Dictionary<int, string> dictionary)
        {
            var selectListItems = new SelectList(dictionary, "Key", "Value");
            return helper.DropDownList(name, selectListItems);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, Dictionary<int, string> dictionary)
        {
            var selectListItems = new SelectList(dictionary, "Key", "Value");
            return helper.DropDownListFor(expression, selectListItems);
        }
        public static MvcHtmlString DropDownListWithSubmit(this HtmlHelper htmlHelper, string name, string optionLabel)
        {
            MvcHtmlString DDL = htmlHelper.DropDownList(name, optionLabel);
            return MvcHtmlString.Create(DDL.ToString().Replace("id=", "onchange=\"this.form.submit()\" id="));
        }
        public static MvcHtmlString DropDownListForWithSubmit<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, string defaultValue)
        {
            MvcHtmlString DDLF = htmlHelper.DropDownListFor(expression, selectList, optionLabel, null);
            return MvcHtmlString.Create(DDLF.ToString().Replace("id=", "onchange=\"this.form.submit()\" id=").Replace("alue=\"\"", string.Format("alue=\"{0}\"", defaultValue)));
        }


        /// <summary>
        /// Adds red star to end of field name to indicate mandatory field if TestForRequired is set to true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="TestForRequired"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool TestForRequired)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }

            if (TestForRequired && metadata.IsRequired)
            {
                var spanTag = new TagBuilder("span");
                spanTag.Attributes.Add("alt", "Mandatory field");
                spanTag.AddCssClass("required-star");
                spanTag.InnerHtml = "*";

                resolvedLabelText = string.Concat(resolvedLabelText, spanTag.ToString(TagRenderMode.Normal));
            }

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tag.InnerHtml = resolvedLabelText;
            tag.AddCssClass("text_label");

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// HiddenFor 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression">Something</param>
        /// <param name="value">Default value to be added</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes)
        {
            var propertyName = ExpressionHelper.GetExpressionText(expression);

            var input = new TagBuilder("input");
            input.MergeAttribute("id", helper.AttributeEncode(helper.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)));
            input.MergeAttribute("name", helper.AttributeEncode(helper.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName)));
            input.MergeAttribute("value", value.ToString());
            input.MergeAttribute("type", "hidden");
            input.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(input.ToString());
        }

        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, string optionLabel, string defaultValue)
        {
            MvcHtmlString DDL = htmlHelper.DropDownList(name, optionLabel);
            string find = string.Format("=\"{0}\"", defaultValue);
            return MvcHtmlString.Create(DDL.ToString().Replace(find, string.Format("{0} selected", find)));
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes, string defaultValue)
        {
            MvcHtmlString DDLF = htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
            string find = string.Format("=\"{0}\"", defaultValue);
            return MvcHtmlString.Create(DDLF.ToString().Replace(find, string.Format("{0} selected", find)));
        }

        public static MvcHtmlString ImageAndTextLink(this HtmlHelper htmlHelper, string imgSrc, string displayText, string alt, string actionName
                                                        , string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes, string cssClass)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            string imgtag = htmlHelper.Image(imgSrc, alt, imgHtmlAttributes);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            if (cssClass != null) { imglink.AddCssClass(cssClass.ToString()); }
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgtag + "&nbsp;" + displayText;
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            string result = imglink.ToString();
            return MvcHtmlString.Create(result);
        }
        public static MvcHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string titleText, string actionName, string controller, object routeValues
                                            , AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imageUrl);
            imgTag.MergeAttribute("title", titleText);
            var link = helper.ActionLink("[replaceme] " + titleText, actionName, controller, routeValues, ajaxOptions, htmlAttributes).ToHtmlString();   //update

            return new MvcHtmlString(link.Replace("[replaceme]", imgTag.ToString(TagRenderMode.SelfClosing)));
        }

        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName
                                                , object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            string imgtag = htmlHelper.Image(imgSrc, alt, imgHtmlAttributes);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgtag;
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            string result = imglink.ToString();
            return MvcHtmlString.Create(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="imgSrc"></param>
        /// <param name="alt"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="imgHtmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName
                                                , RouteValueDictionary routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            string imgtag = htmlHelper.Image(imgSrc, alt, imgHtmlAttributes);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgtag;
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            string result = imglink.ToString();
            return MvcHtmlString.Create(result);
        }
        public static string Image(this HtmlHelper helper, string url, string altText, object htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("img");
            builder.Attributes.Add("src", url);
            builder.Attributes.Add("alt", altText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }

        /// <summary>
        /// Paging helper to show first, prev, pages, next, last buttons at foot of paged list
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pagedList">PagedList from the Model</param>
        /// <param name="actionName">Action to link buttons to</param>
        /// <param name="controllerName">Controller that hosts the action</param>
        /// <returns></returns>
        public static MvcHtmlString SSGPaging(this HtmlHelper htmlHelper, IPagedList pagedList, string actionName, string controllerName)
        {
            if (pagedList.PageCount > 1)
            {
                string imagePath = VirtualPathUtility.ToAbsolute("~/Images/");
                MvcHtmlString liFirstStr;
                MvcHtmlString liPrevStr;
                MvcHtmlString liNextStr;
                MvcHtmlString liLastStr;
                TagBuilder divTag = new TagBuilder("div");
                divTag.AddCssClass("PagedList-pager");
                TagBuilder list = new TagBuilder("ul");
                TagBuilder liPage = new TagBuilder("li");
                liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                if (pagedList.HasPreviousPage)
                {
                    liFirstStr = htmlHelper.ImageLink(imagePath + "page_first_enabled.png", "<< First Page", actionName, controllerName, new { page = 1 }, null, null);
                    liPrevStr = htmlHelper.ImageLink(imagePath + "page_prev_enabled.png", "< Previous Page", actionName, controllerName, new { page = pagedList.PageNumber - 1 }, null, null);
                }
                else
                {
                    liFirstStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_first_disabled.png", "<< First Page", null));
                    liPrevStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_prev_disabled.png", "< Previous Page", null));
                }
                //build next buttons
                if (pagedList.HasNextPage)
                {
                    liLastStr = htmlHelper.ImageLink(imagePath + "page_last_enabled.png", "Last Page >>", actionName, controllerName, new { page = pagedList.PageCount }, null, null);
                    liNextStr = htmlHelper.ImageLink(imagePath + "page_next_enabled.png", "Next Page >", actionName, controllerName, new { page = pagedList.PageNumber + 1 }, null, null);
                }
                else
                {
                    liLastStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_last_disabled.png", "Last Page >>", null));
                    liNextStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_next_disabled.png", "Next Page >", null));
                }
                //build list items
                TagBuilder liFirst = new TagBuilder("li");
                liFirst.InnerHtml = liFirstStr.ToString();
                TagBuilder liPrev = new TagBuilder("li");
                liPrev.InnerHtml = liPrevStr.ToString();
                TagBuilder liNext = new TagBuilder("li");
                liNext.InnerHtml = liNextStr.ToString();
                TagBuilder liLast = new TagBuilder("li");
                liLast.InnerHtml = liLastStr.ToString();

                //add LIs to UL
                list.InnerHtml += liPage.ToString();
                list.InnerHtml += liFirst.ToString();
                list.InnerHtml += liPrev.ToString();

                List<string> pages = pageDisplay.pagingDisplay(pagedList.PageCount, pagedList.PageNumber);

                foreach (var page in pages)
                {
                    TagBuilder pageLoop = new TagBuilder("li");
                    if (page == "...")
                    {
                        pageLoop.AddCssClass("PageEllipsis");
                        pageLoop.InnerHtml = page.ToString();
                    }
                    else if (page == pagedList.PageNumber.ToString())
                    {
                        pageLoop.AddCssClass("PageNonSelectable");
                        pageLoop.InnerHtml = page.ToString();
                    }
                    else
                    {
                        UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
                        string url = urlHelper.Action(actionName, controllerName, new { page = page });
                        TagBuilder pageA = new TagBuilder("a");
                        pageA.MergeAttribute("href", url);
                        pageA.InnerHtml = page.ToString();
                        pageLoop.AddCssClass("PageSelectable");
                        pageLoop.InnerHtml = pageA.ToString();
                    }
                    list.InnerHtml += pageLoop.ToString();
                }
                list.InnerHtml += liNext.ToString();
                list.InnerHtml += liLast.ToString();
                //Add UL to Div
                divTag.InnerHtml = list.ToString();
                return MvcHtmlString.Create(divTag.ToString());
            }
            else
            {
                //TagBuilder divTag = new TagBuilder("div");
                //divTag.AddCssClass("PagedList-pager");
                //TagBuilder list = new TagBuilder("ul");
                //TagBuilder liPage = new TagBuilder("li");
                //liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                //list.InnerHtml += liPage.ToString();
                //divTag.InnerHtml = list.ToString();
                //return MvcHtmlString.Create(divTag.ToString());
                return MvcHtmlString.Create("");
            }
        }
        /// <summary>
        /// Paging helper to show first, prev, pages, next, last buttons at foot of paged list
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pagedList">PagedList from the Model</param>
        /// <param name="actionName">Action to link buttons to</param>
        /// <param name="controllerName">Controller that hosts the action</param>
        /// <param name="routeValues">a RouteValueDictionary collection</param>
        /// <returns>An MVCHTMLString</returns>
        public static MvcHtmlString SSGPaging(this HtmlHelper htmlHelper, IPagedList pagedList, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            if (pagedList.PageCount > 1)
            {
                string imagePath = VirtualPathUtility.ToAbsolute("~/Images/");
                RouteValueDictionary RouteValues;
                MvcHtmlString liFirstStr;
                MvcHtmlString liPrevStr;
                MvcHtmlString liNextStr;
                MvcHtmlString liLastStr;
                TagBuilder divTag = new TagBuilder("div");
                divTag.AddCssClass("PagedList-pager");
                TagBuilder list = new TagBuilder("ul");
                TagBuilder liPage = new TagBuilder("li");
                liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                if (pagedList.HasPreviousPage)
                {
                    RouteValues = new RouteValueDictionary(routeValues);
                    RouteValues.Add("Page", "1");
                    liFirstStr = htmlHelper.ImageLink(imagePath + "page_first_enabled.png", "<< First Page", actionName, controllerName, RouteValues, null, null);
                    RouteValues = new RouteValueDictionary(routeValues);
                    RouteValues.Add("Page", pagedList.PageNumber - 1);
                    liPrevStr = htmlHelper.ImageLink(imagePath + "page_prev_enabled.png", "< Previous Page", actionName, controllerName, RouteValues, null, null);
                }
                else
                {
                    liFirstStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_first_disabled.png", "<< First Page", null));
                    liPrevStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_prev_disabled.png", "< Previous Page", null));
                }
                //build next buttons
                if (pagedList.HasNextPage)
                {
                    RouteValues = new RouteValueDictionary(routeValues);
                    RouteValues.Add("Page", pagedList.PageCount);
                    liLastStr = htmlHelper.ImageLink(imagePath + "page_last_enabled.png", "Last Page >>", actionName, controllerName, RouteValues, null, null);
                    RouteValues = new RouteValueDictionary(routeValues);
                    RouteValues.Add("Page", pagedList.PageNumber + 1);
                    liNextStr = htmlHelper.ImageLink(imagePath + "page_next_enabled.png", "Next Page >", actionName, controllerName, RouteValues, null, null);
                }
                else
                {
                    liLastStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_last_disabled.png", "Last Page >>", null));
                    liNextStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_next_disabled.png", "Next Page >", null));
                }
                //build list items
                TagBuilder liFirst = new TagBuilder("li");
                liFirst.InnerHtml = liFirstStr.ToString();
                TagBuilder liPrev = new TagBuilder("li");
                liPrev.InnerHtml = liPrevStr.ToString();
                TagBuilder liNext = new TagBuilder("li");
                liNext.InnerHtml = liNextStr.ToString();
                TagBuilder liLast = new TagBuilder("li");
                liLast.InnerHtml = liLastStr.ToString();

                //add LIs to UL
                list.InnerHtml += liPage.ToString();
                list.InnerHtml += liFirst.ToString();
                list.InnerHtml += liPrev.ToString();

                List<string> pages = pageDisplay.pagingDisplay(pagedList.PageCount, pagedList.PageNumber);

                foreach (var page in pages)
                {
                    TagBuilder pageLoop = new TagBuilder("li");
                    if (page == "...")
                    {
                        pageLoop.AddCssClass("PageEllipsis");
                        pageLoop.InnerHtml = page.ToString();
                    }
                    else if (page == pagedList.PageNumber.ToString())
                    {
                        pageLoop.AddCssClass("PageNonSelectable");
                        pageLoop.InnerHtml = page.ToString();
                    }
                    else
                    {
                        UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
                        RouteValues = new RouteValueDictionary(routeValues);
                        RouteValues.Add("Page", page);
                        string url = urlHelper.Action(actionName, controllerName, RouteValues);
                        TagBuilder pageA = new TagBuilder("a");
                        pageA.MergeAttribute("href", url);
                        pageA.InnerHtml = page.ToString();
                        pageLoop.AddCssClass("PageSelectable");
                        pageLoop.InnerHtml = pageA.ToString();
                    }
                    list.InnerHtml += pageLoop.ToString();
                }
                list.InnerHtml += liNext.ToString();
                list.InnerHtml += liLast.ToString();
                //Add UL to Div
                divTag.InnerHtml = list.ToString();
                return MvcHtmlString.Create(divTag.ToString());
            }
            else
            {
                //TagBuilder divTag = new TagBuilder("div");
                //divTag.AddCssClass("PagedList-pager");
                //TagBuilder list = new TagBuilder("ul");
                //TagBuilder liPage = new TagBuilder("li");
                //liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                //list.InnerHtml += liPage.ToString();
                //divTag.InnerHtml = list.ToString();
                //return MvcHtmlString.Create(divTag.ToString());
                return MvcHtmlString.Create("");
            }
        }
        /// <summary>
        /// Paging helper to show first, prev, pages, next, last buttons at foot of paged list
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pagedList">PagedList from the Model</param>
        /// <param name="actionName">Action to link buttons to</param>
        /// <param name="controllerName">Controller that hosts the action</param>
        /// <returns></returns>
        public static MvcHtmlString SSGPaging(this HtmlHelper htmlHelper, IPagedList pagedList, string actionName, string controllerName, object routeValues)
        {
            if (pagedList.PageCount > 1)
            {
                string imagePath = VirtualPathUtility.ToAbsolute("~/Images/");
                MvcHtmlString liFirstStr;
                MvcHtmlString liPrevStr;
                MvcHtmlString liNextStr;
                MvcHtmlString liLastStr;
                TagBuilder divTag = new TagBuilder("div");
                divTag.AddCssClass("PagedList-pager");
                TagBuilder list = new TagBuilder("ul");
                TagBuilder liPage = new TagBuilder("li");
                TagBuilder liFirst = new TagBuilder("li");
                liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                if (pagedList.HasPreviousPage)
                {

                    var firstLI = new TagBuilder("li");
                    firstLI.SetInnerText("<< FirstPage");
                    RouteValueDictionary newRoutevalues = new RouteValueDictionary(routeValues);
                    newRoutevalues.Add("Page", "1");
                    liFirstStr = htmlHelper.ImageLink(imagePath + "page_first_enabled.png", "<< First Page", actionName, controllerName, routeValues, null, null);
                    liFirst.InnerHtml = liFirstStr.ToString();

                    liPrevStr = htmlHelper.ImageLink(imagePath + "page_prev_enabled.png", "< Previous Page", actionName, controllerName, new { page = pagedList.PageNumber - 1 }, null, null);
                }
                else
                {
                    liFirstStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_first_disabled.png", "<< First Page", null));
                    liPrevStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_prev_disabled.png", "< Previous Page", null));
                }
                //build next buttons
                if (pagedList.HasNextPage)
                {
                    liLastStr = htmlHelper.ImageLink(imagePath + "page_last_enabled.png", "Last Page >>", actionName, controllerName, new { page = pagedList.PageCount }, null, null);
                    liNextStr = htmlHelper.ImageLink(imagePath + "page_next_enabled.png", "Next Page >", actionName, controllerName, new { page = pagedList.PageNumber + 1 }, null, null);
                }
                else
                {
                    liLastStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_last_disabled.png", "Last Page >>", null));
                    liNextStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_next_disabled.png", "Next Page >", null));
                }
                //build list items
                //TagBuilder liFirst = new TagBuilder("li");
                liFirst.InnerHtml = liFirstStr.ToString();
                TagBuilder liPrev = new TagBuilder("li");
                liPrev.InnerHtml = liPrevStr.ToString();
                TagBuilder liNext = new TagBuilder("li");
                liNext.InnerHtml = liNextStr.ToString();
                TagBuilder liLast = new TagBuilder("li");
                liLast.InnerHtml = liLastStr.ToString();

                //add LIs to UL
                list.InnerHtml += liPage.ToString();
                list.InnerHtml += liFirst.ToString();
                list.InnerHtml += liPrev.ToString();

                List<string> pages = pageDisplay.pagingDisplay(pagedList.PageCount, pagedList.PageNumber);

                foreach (var page in pages)
                {
                    TagBuilder pageLoop = new TagBuilder("li");
                    if (page == "...")
                    {
                        pageLoop.AddCssClass("PageEllipsis");
                        pageLoop.InnerHtml = page.ToString();
                    }
                    else if (page == pagedList.PageNumber.ToString())
                    {
                        pageLoop.AddCssClass("PageNonSelectable");
                        pageLoop.InnerHtml = page.ToString();
                    }
                    else
                    {
                        UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
                        string url = urlHelper.Action(actionName, controllerName, new { page = page });
                        TagBuilder pageA = new TagBuilder("a");
                        pageA.MergeAttribute("href", url);
                        pageA.InnerHtml = page.ToString();
                        pageLoop.AddCssClass("PageSelectable");
                        pageLoop.InnerHtml = pageA.ToString();
                    }
                    list.InnerHtml += pageLoop.ToString();
                }
                list.InnerHtml += liNext.ToString();
                list.InnerHtml += liLast.ToString();
                //Add UL to Div
                divTag.InnerHtml = list.ToString();
                return MvcHtmlString.Create(divTag.ToString());
            }
            else
            {
                //TagBuilder divTag = new TagBuilder("div");
                //divTag.AddCssClass("PagedList-pager");
                //TagBuilder list = new TagBuilder("ul");
                //TagBuilder liPage = new TagBuilder("li");
                //liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                //list.InnerHtml += liPage.ToString();
                //divTag.InnerHtml = list.ToString();
                //return MvcHtmlString.Create(divTag.ToString());
                return MvcHtmlString.Create("");
            }
        }


        /// <summary>
        /// Display a from that contains buttons based on a PagedList from a *ListViewModel NOTE- only use on from based pages!
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName">The action that will be called when button pushed</param>
        /// <param name="controllerName">The controller conatining the action</param>
        /// <param name="model">A ListViewModel containing sort and filter criteria</param>
        /// <param name="pagedList">A paged list of objects</param>
        /// <returns></returns>
        public static MvcHtmlString SSGPaging(this HtmlHelper htmlHelper, string actionName, string controllerName, ListViewModel model, IPagedList pagedList, string token="")
        {
            if (pagedList.PageCount > 1)
            {
                string imagePath = VirtualPathUtility.ToAbsolute("~/Images/");
                MvcHtmlString firstBtn;
                MvcHtmlString prevBtn;
                MvcHtmlString nextBtn;
                MvcHtmlString lastBtn;
                MvcHtmlString pageBtn;
                TagBuilder divTag = new TagBuilder("div");
                TagBuilder form = new TagBuilder("form");
                form.MergeAttribute("actionName", actionName);
                form.MergeAttribute("controllerName", controllerName);
                form.MergeAttribute("Method", FormMethod.Post.ToString());

                //Add CSRF token
                if (token != "")
                {
                    TagBuilder csrfToken = new TagBuilder("input");
                    csrfToken.Attributes.Add("id", "__RequestVerificationToken");
                    csrfToken.Attributes.Add("name", "__RequestVerificationToken");
                    csrfToken.MergeAttribute("type", "hidden");
                    csrfToken.MergeAttribute("value", token);
                    form.InnerHtml += csrfToken;
                }

                string pageText = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                pageBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" value=\"{0}\" class=\"pageButton text\" disabled=\"disabled\" style=\"width:100px !important;\">{0}</button>", pageText));
                if (pagedList.HasPreviousPage)
                {
                    firstBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img first enabled\" value=\"{0}\">{0}</button>", 1));
                    prevBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img prev enabled\" value=\"{0}\">{0}</button>", pagedList.PageNumber - 1));
                }
                else
                {
                    firstBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img first disabled\" disabled=\"disabled\" value=\"{0}\">{0}</button>", 1));
                    prevBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img prev disabled\" disabled=\"disabled\" value=\"\">&nbsp;</button>", ""));
                }
                //build next buttons
                if (pagedList.HasNextPage)
                {
                    lastBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img last enabled\" value=\"{0}\">{0}</button>", pagedList.PageCount));
                    nextBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img next enabled\" value=\"{0}\">{0}</button>", pagedList.PageNumber + 1));
                }
                else
                {
                    lastBtn = MvcHtmlString.Create("<button type=\"submit\" name=\"page\" class=\"pageButton img last disabled\" disabled=\"disabled\" value=\"x\">x</button>");
                    nextBtn = MvcHtmlString.Create("<button type=\"submit\" name=\"page\" class=\"pageButton img next disabled\" disabled=\"disabled\" value=\"x\">x</button>");
                }

                //Add leading fields to DIV
                form.InnerHtml += pageBtn;
                form.InnerHtml += firstBtn;
                form.InnerHtml += prevBtn;

                if (model is QuickSearch)
                {
                    QuickSearch temp = model as QuickSearch;

                    TagBuilder FileNumber = new TagBuilder("input");
                    FileNumber.Attributes.Add("id", "FileNumber");
                    FileNumber.Attributes.Add("name", "FileNumber");
                    FileNumber.MergeAttribute("type", "hidden");
                    FileNumber.MergeAttribute("value", temp.FileNumber == null ? "" : temp.FileNumber.ToString());
                    form.InnerHtml += FileNumber.ToString();
                    TagBuilder ChildSurname = new TagBuilder("input");
                    ChildSurname.Attributes.Add("id", "ChildSurname");
                    ChildSurname.Attributes.Add("name", "ChildSurname");
                    ChildSurname.MergeAttribute("type", "hidden");
                    ChildSurname.MergeAttribute("value", temp.ChildSurname == null ? "" : temp.ChildSurname.ToString());
                    form.InnerHtml += ChildSurname.ToString();
                    TagBuilder ChildForenames = new TagBuilder("input");
                    ChildForenames.Attributes.Add("id", "ChildForenames");
                    ChildForenames.Attributes.Add("name", "ChildForenames");
                    ChildForenames.MergeAttribute("type", "hidden");
                    ChildForenames.MergeAttribute("value", temp.ChildForenames == null ? "" : temp.ChildForenames.ToString());
                    form.InnerHtml += ChildForenames.ToString();
                    TagBuilder ChildDateofBirth = new TagBuilder("input");
                    ChildDateofBirth.Attributes.Add("id", "ChildDateofBirth");
                    ChildDateofBirth.Attributes.Add("name", "ChildDateofBirth");
                    ChildDateofBirth.MergeAttribute("type", "hidden");
                    ChildDateofBirth.MergeAttribute("value", temp.ChildDateofBirth == null ? "" : temp.ChildDateofBirth.ToString());
                    form.InnerHtml += ChildDateofBirth.ToString();
 

                }

                if (model is WardshipRecordVMlistView)
                {
                    WardshipRecordVMlistView temp = model as WardshipRecordVMlistView;

                    TagBuilder StartDate = new TagBuilder("input");
                    StartDate.Attributes.Add("id", "StartDate");
                    StartDate.Attributes.Add("name", "StartDate");
                    StartDate.MergeAttribute("type", "hidden");
                    StartDate.MergeAttribute("value", temp.StartDate == null ? "" : temp.StartDate.ToString());
                    form.InnerHtml += StartDate.ToString();

                    TagBuilder EndDate = new TagBuilder("input");
                    EndDate.Attributes.Add("id", "EndDate");
                    EndDate.Attributes.Add("name", "EndDate");
                    EndDate.MergeAttribute("type", "hidden");
                    EndDate.MergeAttribute("value", temp.EndDate == null ? "" : temp.EndDate.ToString());
                    form.InnerHtml += EndDate.ToString();

                }





                List<string> pages = pageDisplay.pagingDisplay(pagedList.PageCount, pagedList.PageNumber);

                foreach (var page in pages)
                {
                    MvcHtmlString pageLoopBtn;
                    if (page == "..." || page == pagedList.PageNumber.ToString())
                    {
                        pageLoopBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton text number\" disabled=\"disabled\" value=\"{0}\">{0}</button>", page.ToString()));
                    }
                    else
                    {
                        pageLoopBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton\" value=\"{0}\">{0}</button>", page.ToString()));
                    }
                    form.InnerHtml += pageLoopBtn.ToString();
                }
                //Add closing fields to DIV
                form.InnerHtml += nextBtn;
                form.InnerHtml += lastBtn;
                divTag.InnerHtml = form.ToString();
                return MvcHtmlString.Create(form.ToString());
            }
            else
            {
                return MvcHtmlString.Create("");
            }
        }
        /// <summary>
        /// Paging helper to show first, prev, pages, next, last buttons at foot of paged list
        /// This version is used on Audit pages and requires auditType htmlAttribute to be added to the paging buttons
        /// e.g. new { auditType=ABC }
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="pagedList">PagedList from the Model</param>
        /// <param name="actionName">Action to link buttons to</param>
        /// <param name="controllerName">Controller that hosts the action</param>
        /// <param name="auditType">String that details which audit type to display</param>
        /// <returns></returns>
        public static MvcHtmlString Paging(this HtmlHelper htmlHelper, IPagedList pagedList, string actionName, string controllerName, string auditType)
        {
            if (pagedList.PageCount > 1)
            {
                string imagePath = VirtualPathUtility.ToAbsolute("~/Images/");
                MvcHtmlString liFirstStr;
                MvcHtmlString liPrevStr;
                MvcHtmlString liNextStr;
                MvcHtmlString liLastStr;
                TagBuilder divTag = new TagBuilder("div");
                divTag.AddCssClass("PagedList-pager");
                TagBuilder list = new TagBuilder("ul");
                TagBuilder liPage = new TagBuilder("li");
                liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                if (pagedList.HasPreviousPage)
                {
                    liFirstStr = htmlHelper.ImageLink(imagePath + "page_first_enabled.png", "<< First Page", actionName, controllerName, new { page = 1, auditType = auditType }, null, null);
                    liPrevStr = htmlHelper.ImageLink(imagePath + "page_prev_enabled.png", "< Previous Page", actionName, controllerName, new { page = pagedList.PageNumber - 1, auditType = auditType }, null, null);
                }
                else
                {
                    liFirstStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_first_disabled.png", "<< First Page", null));
                    liPrevStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_prev_disabled.png", "< Previous Page", null));
                }
                //build next buttons
                if (pagedList.HasNextPage)
                {
                    liLastStr = htmlHelper.ImageLink(imagePath + "page_last_enabled.png", "Last Page >>", actionName, controllerName, new { page = pagedList.PageCount, auditType = auditType }, null, null);
                    liNextStr = htmlHelper.ImageLink(imagePath + "page_next_enabled.png", "Next Page >", actionName, controllerName, new { page = pagedList.PageNumber + 1, auditType = auditType }, null, null);
                }
                else
                {
                    liLastStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_last_disabled.png", "Last Page >>", null));
                    liNextStr = MvcHtmlString.Create(htmlHelper.Image(imagePath + "page_next_disabled.png", "Next Page >", null));
                }
                //build list items
                TagBuilder liFirst = new TagBuilder("li");
                liFirst.InnerHtml = liFirstStr.ToString();
                TagBuilder liPrev = new TagBuilder("li");
                liPrev.InnerHtml = liPrevStr.ToString();
                TagBuilder liNext = new TagBuilder("li");
                liNext.InnerHtml = liNextStr.ToString();
                TagBuilder liLast = new TagBuilder("li");
                liLast.InnerHtml = liLastStr.ToString();

                //add LIs to UL
                list.InnerHtml += liPage.ToString();
                list.InnerHtml += liFirst.ToString();
                list.InnerHtml += liPrev.ToString();
                for (int page = 1; page <= pagedList.PageCount; page++)
                {
                    TagBuilder pageLoop = new TagBuilder("li");

                    if (page == pagedList.PageNumber)
                    {
                        pageLoop.AddCssClass("PageNonSelectable");
                        pageLoop.InnerHtml = page.ToString();
                    }
                    else
                    {
                        UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
                        string url = urlHelper.Action(actionName, controllerName, new { page = page, auditType = auditType });
                        TagBuilder pageA = new TagBuilder("a");
                        pageA.MergeAttribute("href", url);
                        pageA.InnerHtml = page.ToString();
                        pageLoop.AddCssClass("PageSelectable");
                        pageLoop.InnerHtml = pageA.ToString();
                    }
                    list.InnerHtml += pageLoop.ToString();
                }
                list.InnerHtml += liNext.ToString();
                list.InnerHtml += liLast.ToString();
                //Add UL to Div
                divTag.InnerHtml = list.ToString();
                return MvcHtmlString.Create(divTag.ToString());
            }
            else
            {
                //TagBuilder divTag = new TagBuilder("div");
                //divTag.AddCssClass("PagedList-pager");
                //TagBuilder list = new TagBuilder("ul");
                //TagBuilder liPage = new TagBuilder("li");
                //liPage.InnerHtml = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                //list.InnerHtml += liPage.ToString();
                //divTag.InnerHtml = list.ToString();
                return MvcHtmlString.Create("");
            }
        }
        /// <summary>
        /// Display a sortHeader column button
        /// </summary>
        /// <param name="DisplayText">Text to display on screen</param>
        /// <param name="currentSortOrder">asc, desc or blank</param>
        /// <param name="sortColumn">model field name to sort by</param>
        /// <returns></returns>
        public static MvcHtmlString SortHeaderButton(this HtmlHelper htmlHelper, string DisplayText, string sortOrder, string sortColumn)
        {
            string nextSortOrder = "asc";
            string dispSortOrder = null;
            if (sortOrder != null && sortOrder.Contains(sortColumn))
            {
                sortOrder = sortOrder.Replace(sortColumn, "").Trim();
                switch (sortOrder)
                {
                    case "asc":
                        nextSortOrder = "desc";
                        dispSortOrder = "asc";
                        break;
                    case "desc":
                        nextSortOrder = "asc";
                        dispSortOrder = "desc";
                        break;
                    default:
                        nextSortOrder = null;
                        dispSortOrder = null;
                        break;
                }
            }
            TagBuilder button = new TagBuilder("button");
            button.Attributes.Add("type", "submit");
            button.Attributes.Add("value", string.Format("{0} {1}", sortColumn, nextSortOrder).Trim());
            button.Attributes.Add("name", "sortOrder");
            button.InnerHtml = DisplayText;


            button.AddCssClass(string.Format("sortButton {0}", dispSortOrder).Trim());

            return MvcHtmlString.Create(button.ToString());
        }


        /// <summary>
        /// Display a from that contains buttons based on a PagedList from a *ListViewModel NOTE- only use on form based pages!
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName">The action that will be called when button pushed</param>
        /// <param name="controllerName">The controller conatining the action</param>
        /// <param name="model">A ListViewModel containing sort and filter criteria</param>
        /// <param name="pagedList">A paged list of objects</param>
        /// <returns></returns>
        public static MvcHtmlString SSGPaging(this HtmlHelper htmlHelper, string actionName, string controllerName, AdminListView model, IPagedList pagedList)
        {
            if (pagedList.PageCount > 1)
            {
                string imagePath = VirtualPathUtility.ToAbsolute("~/Images/");

                MvcHtmlString firstBtn;
                MvcHtmlString prevBtn;
                MvcHtmlString nextBtn;
                MvcHtmlString lastBtn;
                MvcHtmlString pageBtn;
                MvcHtmlString recBtn;
                TagBuilder divTag = new TagBuilder("div");
                TagBuilder form = new TagBuilder("form");
                form.MergeAttribute("actionName", actionName);
                form.MergeAttribute("controllerName", controllerName);
                form.MergeAttribute("Method", FormMethod.Post.ToString());

                string pageText = string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount);
                pageBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" value=\"{0}\" class=\"pageButton text\" disabled=\"disabled\" style=\"width:100px !important;\">{0}</button>", pageText));
                if (pagedList.HasPreviousPage)
                {
                    firstBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img first enabled\" value=\"{0}\">{0}</button>", 1));
                    prevBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img prev enabled\" value=\"{0}\">{0}</button>", pagedList.PageNumber - 1));
                }
                else
                {
                    firstBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img first disabled\" disabled=\"disabled\" value=\"{0}\">{0}</button>", 1));
                    prevBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img prev disabled\" disabled=\"disabled\" value=\"\">&nbsp;</button>", ""));
                }
                //build next buttons
                if (pagedList.HasNextPage)
                {
                    lastBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img last enabled\" value=\"{0}\">{0}</button>", pagedList.PageCount));
                    nextBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img next enabled\" value=\"{0}\">{0}</button>", pagedList.PageNumber + 1));
                }
                else
                {
                    lastBtn = MvcHtmlString.Create("<button type=\"submit\" name=\"page\" class=\"pageButton img last disabled\" disabled=\"disabled\" value=\"x\">x</button>");
                    nextBtn = MvcHtmlString.Create("<button type=\"submit\" name=\"page\" class=\"pageButton img next disabled\" disabled=\"disabled\" value=\"x\">x</button>");
                }
                //Add Hidden fields from AdminListView
                TagBuilder onlyActive = new TagBuilder("input");
                onlyActive.Attributes.Add("id", "onlyActive");
                onlyActive.Attributes.Add("name", "onlyActive");
                onlyActive.MergeAttribute("type", "hidden");
                onlyActive.MergeAttribute("value", model.onlyActive.ToString());
                if (model.detailContains != null)
                {
                    TagBuilder detailContains = new TagBuilder("input");
                    detailContains.Attributes.Add("id", "detailContains");
                    detailContains.Attributes.Add("name", "detailContains");
                    detailContains.MergeAttribute("type", "hidden");
                    detailContains.MergeAttribute("value", model.detailContains.ToString());
                }
                //Add leading fields to DIV
                form.InnerHtml += onlyActive.ToString();
                form.InnerHtml += pageBtn;
                form.InnerHtml += firstBtn;
                form.InnerHtml += prevBtn;

                List<string> pages = generic.pagingDisplay(pagedList.PageCount, pagedList.PageNumber);

                foreach (var page in pages)
                {
                    MvcHtmlString pageLoopBtn;
                    if (page == "..." || page == pagedList.PageNumber.ToString())
                    {
                        pageLoopBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton text number\" disabled=\"disabled\" value=\"{0}\">{0}</button>", page.ToString()));
                    }
                    else
                    {
                        pageLoopBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton\" value=\"{0}\">{0}</button>", page.ToString()));
                    }
                    form.InnerHtml += pageLoopBtn.ToString();
                }
                //record count
                string recCount = string.Format("{0} record{1}", pagedList.TotalItemCount, pagedList.TotalItemCount == 1 ? "" : "s");
                recBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" value=\"{0}\" class=\"pageButton text\" disabled=\"disabled\" style=\"width:100px !important;\">{0}</button>", recCount));

                //Add closing fields to DIV
                form.InnerHtml += nextBtn;
                form.InnerHtml += lastBtn;
                form.InnerHtml += recBtn;
                divTag.InnerHtml = form.ToString();
                return MvcHtmlString.Create(form.ToString());
            }
            else
            {
                return MvcHtmlString.Create("");
            }
        }

        public static MvcHtmlString ReportPaging(this HtmlHelper htmlHelper, string actionName, string controllerName, Report model, IPagedList<WardshipRecord> pagedList)
        {
            if (pagedList == null || pagedList.Count == 0 || pagedList.PageCount == 0)
            {
                return MvcHtmlString.Empty;
            }

            string imagePath = VirtualPathUtility.ToAbsolute("~/Images/");
            TagBuilder form = new TagBuilder("form");
            form.MergeAttribute("action", htmlHelper.ViewContext.HttpContext.Request.Url.PathAndQuery);
            form.MergeAttribute("method", "post");

            MvcHtmlString pageBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" value=\"{0}\" class=\"pageButton text\" disabled=\"disabled\" style=\"width:100px !important;\">{0}</button>", 
                string.Format("Page {0} of {1}", pagedList.PageCount < pagedList.PageNumber ? 0 : pagedList.PageNumber, pagedList.PageCount)));

            MvcHtmlString firstBtn, prevBtn, nextBtn, lastBtn;

            if (pagedList.HasPreviousPage)
            {
                firstBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img first enabled\" value=\"{0}\"><img src=\"{1}\" alt=\"<< First Page\" /></button>", 1, imagePath + "page_first_enabled.png"));
                prevBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img prev enabled\" value=\"{0}\"><img src=\"{1}\" alt=\"< Previous Page\" /></button>", pagedList.PageNumber - 1, imagePath + "page_prev_enabled.png"));
            }
            else
            {
                firstBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img first disabled\" disabled=\"disabled\"><img src=\"{0}\" alt=\"<< First Page\" /></button>", imagePath + "page_first_disabled.png"));
                prevBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img prev disabled\" disabled=\"disabled\"><img src=\"{0}\" alt=\"< Previous Page\" /></button>", imagePath + "page_prev_disabled.png"));
            }

            if (pagedList.HasNextPage)
            {
                nextBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img next enabled\" value=\"{0}\"><img src=\"{1}\" alt=\"Next Page >\" /></button>", pagedList.PageNumber + 1, imagePath + "page_next_enabled.png"));
                lastBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img last enabled\" value=\"{0}\"><img src=\"{1}\" alt=\"Last Page >>\" /></button>", pagedList.PageCount, imagePath + "page_last_enabled.png"));
            }
            else
            {
                nextBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img next disabled\" disabled=\"disabled\"><img src=\"{0}\" alt=\"Next Page >\" /></button>", imagePath + "page_next_disabled.png"));
                lastBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton img last disabled\" disabled=\"disabled\"><img src=\"{0}\" alt=\"Last Page >>\" /></button>", imagePath + "page_last_disabled.png"));
            }

            form.InnerHtml += pageBtn.ToString();
            form.InnerHtml += firstBtn.ToString();
            form.InnerHtml += prevBtn.ToString();

            List<string> pages = generic.pagingDisplay(pagedList.PageCount, pagedList.PageNumber);
            foreach (var page in pages)
            {
                MvcHtmlString pageLoopBtn;
                if (page == "...")
                {
                    pageLoopBtn = MvcHtmlString.Create(string.Format("<span class=\"pageButton text\">{0}</span>", page));
                }
                else if (page == pagedList.PageNumber.ToString())
                {
                    pageLoopBtn = MvcHtmlString.Create(string.Format("<span class=\"pageButton text current\">{0}</span>", page));
                }
                else
                {
                    pageLoopBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" name=\"page\" class=\"pageButton\" value=\"{0}\">{0}</button>", page));
                }
                form.InnerHtml += pageLoopBtn.ToString();
            }

            form.InnerHtml += nextBtn.ToString();
            form.InnerHtml += lastBtn.ToString();

            int totalRecords = (int)(htmlHelper.ViewContext.ViewData["TotalRecords"] ?? 0);
            MvcHtmlString recBtn = MvcHtmlString.Create(string.Format("<button type=\"submit\" value=\"{0}\" class=\"pageButton text\" disabled=\"disabled\" style=\"width:100px !important;\">{0}</button>", 
                string.Format("{0} record{1}", totalRecords, totalRecords == 1 ? "" : "s")));
            form.InnerHtml += recBtn.ToString();

            return MvcHtmlString.Create(form.ToString());
        }

        #endregion

    }
}    