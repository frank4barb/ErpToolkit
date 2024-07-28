using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;


//ESTENZIONE PER:   SCRIPTS SECTION IN ASP.NET CORE PARTIAL VIEW 
//                  https://www.devready.co.uk/post/scripts-section-in-asp-net-core-partial-view

namespace ErpToolkit.Extensions //namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class HtmlHelperExtensions
    {
        ///<summary>
        /// Adds a partial view script to the Http context to be rendered in the parent view
        /// </summary>

        public static IHtmlHelper Script(this IHtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return null;
        }

        ///<summary>
        /// Renders any scripts used within the partial views
        /// </summary>

        /// 
        public static IHtmlHelper RenderPartialViewScripts(this IHtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return null;
        }

        //=======================================================================================================================
        //=======================================================================================================================
        //=======================================================================================================================


        // SLITER: fa scorrere circolarmente le immagini

        public static HtmlString ErpCarousel(this IHtmlHelper html, IList<string> imageSources)
        {
            var carouselWrapper = new TagBuilder("div");
            carouselWrapper.AddCssClass("carousel slide");
            carouselWrapper.MergeAttribute("data-ride", "carousel");

            var innerDiv = new TagBuilder("div");
            innerDiv.AddCssClass("carousel-inner");

            for (var i = 0; i < imageSources.Count; i++)
            {
                var itemDiv = new TagBuilder("div");
                itemDiv.AddCssClass("carousel-item");

                if (i == 0)
                {
                    itemDiv.AddCssClass("active");
                }

                var image = new TagBuilder("img");
                image.MergeAttribute("src", imageSources[i]);
                image.AddCssClass("d-block w-100");
                image.MergeAttribute("alt", "Image " + (i + 1));

                itemDiv.InnerHtml.AppendHtml(image);
                innerDiv.InnerHtml.AppendHtml(itemDiv);
            }

            carouselWrapper.InnerHtml.AppendHtml(innerDiv);

            var prevButton = CreateControlButton("carousel-control-prev", "prev", "Previous");
            var nextButton = CreateControlButton("carousel-control-next", "next", "Next");

            carouselWrapper.InnerHtml.AppendHtml(prevButton);
            carouselWrapper.InnerHtml.AppendHtml(nextButton);

            return new HtmlString(carouselWrapper.GetString());
        }
        private static TagBuilder CreateControlButton(string className, string slideDirection, string ariaLabel)
        {
            var button = new TagBuilder("button");
            button.AddCssClass(className);
            button.MergeAttribute("type", "button");
            button.MergeAttribute("data-target", "#carouselExampleAutoplaying");
            button.MergeAttribute("data-slide", slideDirection);

            var iconSpan = new TagBuilder("span");
            iconSpan.AddCssClass("carousel-control-" + slideDirection + "-icon");
            iconSpan.MergeAttribute("aria-hidden", "true");

            var visuallyHiddenSpan = new TagBuilder("span");
            visuallyHiddenSpan.AddCssClass("visually-hidden");
            visuallyHiddenSpan.InnerHtml.AppendHtml(ariaLabel);

            button.InnerHtml.AppendHtml(iconSpan);
            button.InnerHtml.AppendHtml(visuallyHiddenSpan);

            return button;
        }
        private static string GetString(this IHtmlContent content)
        {
            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
		}


        //=======================================================================================================================
        //=======================================================================================================================
        //=======================================================================================================================


        // AUTOCOMPLETE STATICO: 

        //<!-- https://getbootstrap.com/docs/5.1/forms/form-control/#datalists -->
        //<label for="exampleDataList" class="form-label">Datalist example</label>
        //<input class="form-control" list="datalistOptions" id="exampleDataList" placeholder="Type to search...">
        //<datalist id="datalistOptions">
        //	<option value="San Francisco"> San Francisco </option>
        //  <option value="New York"> New York </option>
        //  <option value="Seattle"> Seattle </option >
        //	<option value="Los Angeles"> Los Angeles</option>
        //  <option value="Chicago"> Chicago </option >
        //</datalist>

        //		public static HtmlString ErpDataListFor<TModel, TResult>(this IHtmlHelper htmlHelper, Expression<Func<TModel, TResult>> expression, IList<SelectListItem> selectList, string placeholder="") 

        public static HtmlString ErpDataListFor(this IHtmlHelper htmlHelper, string inputId, IList<string> selectList, string placeholder="") 
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            ArgumentNullException.ThrowIfNull(inputId);

            // @Html.EditorFor(model => model.TestoLibero, new { htmlAttributes = new { @class = "form-control" } })
            //Html.TextBoxFor( p => p.FirstName ) 
            //For "FirstName" from p => p.FirstName
            //string fieldName = ((MemberExpression)expression.Body).Member.Name; //watch out for runtime casting errors -- https://stackoverflow.com/questions/2789504/get-the-property-as-a-string-from-an-expressionfunctmodel-tproperty

			string datalistOptionsId = "_datalistOptions_" + Guid.NewGuid();
			var input = new TagBuilder("input");
			input.AddCssClass("form-control");
			input.MergeAttribute("id", inputId);
			input.MergeAttribute("list", datalistOptionsId);
			input.MergeAttribute("placeholder", placeholder);

			var datalist = new TagBuilder("datalist");
			datalist.MergeAttribute("id", datalistOptionsId);
			for (var i = 0; i < selectList.Count; i++)
			{
				var option = new TagBuilder("option");
				option.MergeAttribute("value", selectList[i]);
                option.InnerHtml.AppendHtml("");
				datalist.InnerHtml.AppendHtml(option);
			}

			var datalistWrapper = new TagBuilder("div");
			datalistWrapper.InnerHtml.AppendHtml(input);
			datalistWrapper.InnerHtml.AppendHtml(datalist);
			return new HtmlString(datalistWrapper.GetString());
		}


        //Un HtmlHelper per sfruttare la DataList di HTML 5 in ASP.NET MVC
        // https://www.aspitalia.com/script/1142/HtmlHelper-Sfruttare-DataList-HTML-ASP.NET-MVC.aspx#

        //<input type = "text"... list="nome_dataList" />;
        //<datalist id = "nome_dataList" >
        //<option value="option1">option1</option>
        //<option value = "option2" > option2 </option >
        //<option value="option3">option3</option>
        //</datalist>

        //usage
        //<div class="col-md-10">
        //  @Html.DataListFor(model => model.Title, this.Model.AvailableTitles, null)
        //</div>

        //public static HtmlString DataListFor<TModel, TProperty>(
        //                                                        this HtmlHelper<TModel> html,
        //                                                        Expression<Func<TModel, TProperty>> expression,
        //                                                        IEnumerable<SelectListItem> selectList, object htmlAttributes)
        //{
        //    var listId = ExpressionHelper.GetExpressionText(expression) + "_dataList";

        //    if (htmlAttributes == null)
        //        htmlAttributes = new object();

        //    RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
        //    dictionary.Add("list", listId);

        //    var input = html.TextBoxFor(expression, dictionary);

        //    var dataList = new TagBuilder("DataList");
        //    dataList.GenerateId(listId);

        //    StringBuilder items = new StringBuilder();
        //    foreach (var item in selectList)
        //    {
        //        items.AppendLine(ItemToOption(item));
        //    }

        //    dataList.InnerHtml = items.ToString();

        //    return new HtmlString(input + dataList.ToString());
        //}
        //private static string ItemToOption(SelectListItem item)
        //{
        //    TagBuilder builder = new TagBuilder("option");
        //    builder.MergeAttribute("value", item.Value);
        //    builder.SetInnerText(item.Text);

        //    return builder.ToString(TagRenderMode.Normal);
        //}


    }
}
