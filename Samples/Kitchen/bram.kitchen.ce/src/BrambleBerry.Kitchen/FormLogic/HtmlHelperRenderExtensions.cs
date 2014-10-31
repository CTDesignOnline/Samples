using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.IO;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace BrambleBerry.Kitchen.FormLogic
{
 /// <summary>
        /// HtmlHelper extensions for use in templates
        /// </summary>
        public static class HtmlHelperRenderExtensions
        {
            /// <summary>
            /// Used for rendering out the Form for BeginRoutedUmbracoForm
            /// </summary>
            internal class UmbracoRoutedForm : MvcForm
            {
                /// <summary>
                /// Creates an UmbracoForm
                /// </summary>
                /// <param name="viewContext"></param>
                /// <param name="controllerName"></param>
                /// <param name="controllerAction"></param>
                /// <param name="area"></param>
                /// <param name="method"></param>
                /// <param name="additionalRouteVals"></param>
                public UmbracoRoutedForm(
                    ViewContext viewContext,
                    string controllerName,
                    string controllerAction,
                    string area,
                    FormMethod method,
                    object additionalRouteVals = null)
                    : base(viewContext)
                {
                    _viewContext = viewContext;
                    _method = method;
                    _encryptedString = CreateEncryptedRouteString(controllerName, controllerAction, area, additionalRouteVals);
                }

                private readonly ViewContext _viewContext;
                private readonly FormMethod _method;
                private bool _disposed;
                private readonly string _encryptedString;

                protected override void Dispose(bool disposing)
                {
                    if (this._disposed)
                        return;
                    this._disposed = true;

                    //write out the hidden surface form routes
                    _viewContext.Writer.Write("<input name='ufprt' type='hidden' value='" + _encryptedString + "' />");

                    base.Dispose(disposing);
                }
                internal static string CreateEncryptedRouteString(string controllerName, string controllerAction, string area, object additionalRouteVals = null)
                {
                //    Mandate.ParameterNotNullOrEmpty(controllerName, "controllerName");
                  //  Mandate.ParameterNotNullOrEmpty(controllerAction, "controllerAction");
                 //   Mandate.ParameterNotNull(area, "area");

                    //need to create a params string as Base64 to put into our hidden field to use during the routes
                    var surfaceRouteParams = string.Format("c={0}&a={1}&ar={2}",
                                                              HttpUtility.UrlEncode(controllerName),
                                                              HttpUtility.UrlEncode(controllerAction),
                                                              area);

                    var additionalRouteValsAsQuery = additionalRouteVals != null ? additionalRouteVals.ToDictionary<object>().ToQueryString() : null;

                    if (!string.IsNullOrWhiteSpace(additionalRouteValsAsQuery))
                        surfaceRouteParams += "&" + additionalRouteValsAsQuery;

                    return surfaceRouteParams.EncryptWithMachineKey();
                }
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName, FormMethod method)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, null, new Dictionary<string, object>(), method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, null, new Dictionary<string, object>());
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName, object additionalRouteVals, FormMethod method)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, additionalRouteVals, new Dictionary<string, object>(), method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="additionalRouteVals"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName, object additionalRouteVals)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, additionalRouteVals, new Dictionary<string, object>());
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName,
                                                   object additionalRouteVals,
                                                   object htmlAttributes,
                                                   FormMethod method)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, additionalRouteVals, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName,
                                                   object additionalRouteVals,
                                                   object htmlAttributes)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, additionalRouteVals, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName,
                                                   object additionalRouteVals,
                                                   IDictionary<string, object> htmlAttributes,
                                                   FormMethod method)
            {
            //    Mandate.ParameterNotNullOrEmpty(action, "action");
           //     Mandate.ParameterNotNullOrEmpty(controllerName, "controllerName");

                return html.BeginRoutedUmbracoForm(action, controllerName, "", additionalRouteVals, htmlAttributes, method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline against a locally declared controller
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName,
                                                   object additionalRouteVals,
                                                   IDictionary<string, object> htmlAttributes)
            {
             //   Mandate.ParameterNotNullOrEmpty(action, "action");
             //   Mandate.ParameterNotNullOrEmpty(controllerName, "controllerName");

                return html.BeginRoutedUmbracoForm(action, controllerName, "", additionalRouteVals, htmlAttributes);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType, FormMethod method)
            {
                return html.BeginRoutedUmbracoForm(action, surfaceType, null, new Dictionary<string, object>(), method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType)
            {
                return html.BeginRoutedUmbracoForm(action, surfaceType, null, new Dictionary<string, object>());
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action, FormMethod method)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T), method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T));
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType,
                                                   object additionalRouteVals, FormMethod method)
            {
                return html.BeginRoutedUmbracoForm(action, surfaceType, additionalRouteVals, new Dictionary<string, object>(), method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <param name="additionalRouteVals"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType,
                                                   object additionalRouteVals)
            {
                return html.BeginRoutedUmbracoForm(action, surfaceType, additionalRouteVals, new Dictionary<string, object>());
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action, object additionalRouteVals, FormMethod method)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T), additionalRouteVals, method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="additionalRouteVals"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action, object additionalRouteVals)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T), additionalRouteVals);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType,
                                                   object additionalRouteVals,
                                                   object htmlAttributes,
                                                   FormMethod method)
            {
                return html.BeginRoutedUmbracoForm(action, surfaceType, additionalRouteVals, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), method);
            }
            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType,
                                                   object additionalRouteVals,
                                                   IDictionary<string, object> htmlAttributes,
                                                   FormMethod method)
            {
            //    Mandate.ParameterNotNullOrEmpty(action, "action");
            //    Mandate.ParameterNotNull(surfaceType, "surfaceType");

                return html.BeginRoutedUmbracoForm(action, GetControllerName(surfaceType), "", additionalRouteVals, htmlAttributes, method);
            }
            internal static string GetControllerName(Type controllerType)
            {
                if (!controllerType.Name.EndsWith("Controller"))
                {
                    throw new InvalidOperationException("The controller type " + controllerType + " does not follow conventions, MVC Controller class names must be suffixed with the term 'Controller'");
                }
                return controllerType.Name.Substring(0, controllerType.Name.LastIndexOf("Controller"));
            }
            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType,
                                                   object additionalRouteVals,
                                                   object htmlAttributes)
            {
                return html.BeginRoutedUmbracoForm(action, surfaceType, additionalRouteVals, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action,
                                                      object additionalRouteVals,
                                                      object htmlAttributes,
                                                      FormMethod method)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T), additionalRouteVals, htmlAttributes, method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action,
                                                   object additionalRouteVals,
                                                   object htmlAttributes)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T), additionalRouteVals, htmlAttributes);
            }

           

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="surfaceType">The surface controller to route to</param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, Type surfaceType,
                                                   object additionalRouteVals,
                                                   IDictionary<string, object> htmlAttributes)
            {
                return html.BeginRoutedUmbracoForm(action, surfaceType, additionalRouteVals, htmlAttributes, FormMethod.Post);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action,
                                                      object additionalRouteVals,
                                                      IDictionary<string, object> htmlAttributes,
                                                      FormMethod method)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T), additionalRouteVals, htmlAttributes, method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm<T>(this HtmlHelper html, string action,
                                                   object additionalRouteVals,
                                                   IDictionary<string, object> htmlAttributes)
                where T : SurfaceController
            {
                return html.BeginRoutedUmbracoForm(action, typeof(T), additionalRouteVals, htmlAttributes);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="area"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName, string area, FormMethod method)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, area, null, new Dictionary<string, object>(), method);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="area"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName, string area)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, area, null, new Dictionary<string, object>());
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="area"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="method"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName, string area,
                                                   object additionalRouteVals,
                                                   IDictionary<string, object> htmlAttributes,
                                                   FormMethod method)
            {
               // Mandate.ParameterNotNullOrEmpty(action, "action");
           //     Mandate.ParameterNotNullOrEmpty(controllerName, "controllerName");
                var routVars = HtmlHelper.AnonymousObjectToHtmlAttributes(additionalRouteVals);

                string formAction = UrlHelper.GenerateUrl(null, action, controllerName,routVars, html.RouteCollection, html.ViewContext.RequestContext, false /* includeImplicitMvcValues */);


                return html.RenderForm(formAction, method, htmlAttributes, controllerName, action, area, additionalRouteVals);
            }

            /// <summary>
            /// Helper method to create a new form to execute in the Umbraco request pipeline to a surface controller plugin
            /// </summary>
            /// <param name="html"></param>
            /// <param name="action"></param>
            /// <param name="controllerName"></param>
            /// <param name="area"></param>
            /// <param name="additionalRouteVals"></param>
            /// <param name="htmlAttributes"></param>
            /// <returns></returns>
            public static MvcForm BeginRoutedUmbracoForm(this HtmlHelper html, string action, string controllerName, string area,
                                                   object additionalRouteVals,
                                                   IDictionary<string, object> htmlAttributes)
            {
                return html.BeginRoutedUmbracoForm(action, controllerName, area, additionalRouteVals, htmlAttributes, FormMethod.Post);
            }

            /// <summary>
            /// This renders out the form for us
            /// </summary>
            /// <param name="htmlHelper"></param>
            /// <param name="formAction"></param>
            /// <param name="method"></param>
            /// <param name="htmlAttributes"></param>
            /// <param name="surfaceController"></param>
            /// <param name="surfaceAction"></param>
            /// <param name="area"></param>		
            /// <param name="additionalRouteVals"></param>
            /// <returns></returns>
            /// <remarks>
            /// This code is pretty much the same as the underlying MVC code that writes out the form
            /// </remarks>
            private static MvcForm RenderForm(this HtmlHelper htmlHelper,
                                              string formAction,
                                              FormMethod method,
                                              IDictionary<string, object> htmlAttributes,
                                              string surfaceController,
                                              string surfaceAction,
                                              string area,
                                              object additionalRouteVals = null)
            {

                //ensure that the multipart/form-data is added to the html attributes
                if (htmlAttributes.ContainsKey("enctype") == false)
                {
                    htmlAttributes.Add("enctype", "multipart/form-data");
                }

                var tagBuilder = new TagBuilder("form");
                tagBuilder.MergeAttributes(htmlAttributes);
                // action is implicitly generated, so htmlAttributes take precedence.
                tagBuilder.MergeAttribute("action", formAction);
                // method is an explicit parameter, so it takes precedence over the htmlAttributes. 
                tagBuilder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);
                var traditionalJavascriptEnabled = htmlHelper.ViewContext.ClientValidationEnabled && htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled == false;
                if (traditionalJavascriptEnabled)
                {
                    // forms must have an ID for client validation
                    tagBuilder.GenerateId("form" + Guid.NewGuid().ToString("N"));
                }
                htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

                //new UmbracoForm:
                var theForm = new UmbracoRoutedForm(htmlHelper.ViewContext, surfaceController, surfaceAction, area, method, additionalRouteVals);

                if (traditionalJavascriptEnabled)
                {
                    htmlHelper.ViewContext.FormContext.FormId = tagBuilder.Attributes["id"];
                }
                return theForm;
            }
    }
}
