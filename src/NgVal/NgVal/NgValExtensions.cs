using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace NgVal
{
    public static class NgValExtensions
    {
        public static MvcHtmlString NgValFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return NgValFor(htmlHelper, expression, null /* validationMessage */, new RouteValueDictionary());
        }

        public static MvcHtmlString NgValFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage)
        {
            return NgValFor(htmlHelper, expression, validationMessage, new RouteValueDictionary());
        }

        public static MvcHtmlString NgValFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes)
        {
            return NgValFor(htmlHelper, expression, validationMessage, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        public static MvcHtmlString NgValFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());
            var name = ExpressionHelper.GetExpressionText(expression);

            var validations = ModelValidatorProviders.Providers.GetValidators(metadata ?? ModelMetadata.FromStringExpression(name, new ViewDataDictionary()), new ControllerContext()).SelectMany(v => v.GetClientValidationRules());

            var validatorMessages = validations.ToDictionary(k => k.ValidationType, v => v.ErrorMessage);

            string result = "";

            result += getNgValDirectiveString(validatorMessages);
            result += getValidatorDirectivesString(validatorMessages);

            //string validatorMessagesStr = "{";
            //foreach (var validatorMessage in validatorMessages)
            //{
            //    validatorMessagesStr += validatorMessage.Key + ":'" + validatorMessage.Value + "',";
            //}
            //validatorMessagesStr += "}";

            return new MvcHtmlString(result);
        }

        private static string getValidatorDirectivesString(Dictionary<string, string> validatorMessages)
        {
            var result = "";
            foreach (var val in validatorMessages.Keys)
            {
                result += " " + val;
            }
            return result;
        }

        private static string getNgValDirectiveString(Dictionary<string, string> validatorMessages)
        {
            return "ngval='" + Json.Encode(validatorMessages) + "'";
        }
    }
}
