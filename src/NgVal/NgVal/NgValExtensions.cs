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
            var validations = ModelValidatorProviders.Providers.GetValidators(
                    metadata ?? ModelMetadata.FromStringExpression(name, new ViewDataDictionary()),
                    new ControllerContext())
                    .SelectMany(v => v.GetClientValidationRules()).ToArray();

            var validatorMessages = validations.ToDictionary(k => k.ValidationType, v => v.ErrorMessage);

            string result = "";

            result += GetNgValDirectiveString(validatorMessages);
            result += GetValidatorDirectivesString(validations);

            //string validatorMessagesStr = "{";
            //foreach (var validatorMessage in validatorMessages)
            //{
            //    validatorMessagesStr += validatorMessage.Key + ":'" + validatorMessage.Value + "',";
            //}
            //validatorMessagesStr += "}";

            return new MvcHtmlString(result);
        }

        private static string GetValidatorDirectivesString(IEnumerable<ModelClientValidationRule> validations)
        {
            var result = "";
            foreach (var val in validations)
            {
                result += " " + ConvertMvcClientValidatorToAngularValidatorString(val);
            }
            return result;
        }

        private static string ConvertMvcClientValidatorToAngularValidatorString(ModelClientValidationRule val)
        {
            switch (val.ValidationType.ToLower())
            {
                case "required":
                    return "required";
                case "range":
                    return string.Format("ng-minlength=\"{0}\" ng-maxlength=\"{1}\"", val.ValidationParameters["min"], val.ValidationParameters["max"]);
                case "regex":
                    return string.Format("ng-pattern=\"{0}\"", val.ValidationParameters["pattern"]);
                case "length":
                    string lengthRes = "";
                    if (val.ValidationParameters.ContainsKey("min"))
                        lengthRes += string.Format("ng-minlength=\"{0}\"", val.ValidationParameters["min"]);
                    if (val.ValidationParameters.ContainsKey("max"))
                        lengthRes += string.Format("ng-maxlength=\"{0}\"", val.ValidationParameters["max"]);
                    return lengthRes;
                default:
                    return string.Format("{0}=\"{1}\"", val.ValidationType, Json.Encode(val.ValidationParameters));
            }
        }

        private static string GetNgValDirectiveString(Dictionary<string, string> validatorMessages)
        {
            return string.Format("ngval='{0}'", Json.Encode(validatorMessages));
        }
    }
}
