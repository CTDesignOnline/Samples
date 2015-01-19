using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AbleMods.Api;
using AbleMods.Api.Models;
using AbleMods.Models;
using AbleMods.Services;
using Merchello.Core;
using umbraco.NodeFactory;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace AbleMods.Controllers
{
  public class ImportSurfaceController : SurfaceController

  {

      [HttpPost]
      public ActionResult ImportPageView(string submitButton, ImportPageModel model)
      {
          return RedirectToCurrentUmbracoPage();
      }

      [HttpPost]
      public ActionResult ImportPage(string submitButton, ImportPageModel model)
      {
          //model not valid, do not save, but return current umbraco page
          if (!ModelState.IsValid)
          {
              //Perhaps you might want to add a custom message to the ViewBag
              //which will be available on the View when it renders (since we're not 
              //redirecting)          
              return CurrentUmbracoPage();
          }

          // process which button was pressed
          switch (submitButton)
          {
              case "Import Products":
                  ImportProduct(model);
                  break;

              case "Import Category with Products":
                  // make sure catgory is valid
                  if (model.CategoryId == 0 || model.ParentNodeId == 0)
                  {
                      // invalid category selection
                      ViewBag.Status = "You must select a category and parent node!";
                      return CurrentUmbracoPage();
                  }

                  ImportCategory(model);
                  break;

              case "Import Reviews":
                  UProductReviewService.ImportReviews();
                  break;

              case "Delete Reviews Table":
                  UProductReviewService.DeleteLegacyTable();
                  break;

              case "Delete Mappings Table":
                  UProductMappingService.DeleteLegacyTable();
                  break;

              case "Delete Merchello Products":
                  MProductService.DeleteProducts();
                  break;

          }

          //redirect to current page to clear the form
          ViewBag.Status = "Import completed.";
          return RedirectToCurrentUmbracoPage();
      }

      private void ImportProduct(ImportPageModel model)
      {
          // load up hard-coded product
          UProduct testProduct = UProductRepository.Load(2951);
          Guid mGuid = MProductService.MakeMerchelloProduct(testProduct);
          UProductService.MakeProduct(testProduct, model.ParentNodeId, mGuid);
      }

      private void ImportCategory(ImportPageModel model)
      {
          // import the category
          int startingCategoryId = model.CategoryId;
          int startingContentNodeId = model.ParentNodeId;
          UCategoryService.ProcessCategory(startingContentNodeId, startingCategoryId, model.DeepImport, true, model.Test3Only);
      }

      [HttpGet]
      [ChildActionOnly]
      public ActionResult ImportPageView()
      {
          var current = Umbraco.TypedContent(UmbracoContext.PageId);
          var home = current.AncestorOrSelf(1);
          ImportPageModel model = new ImportPageModel();
          model.ContentNodes = new SelectList(home.Descendants("ProductListing"), "Id", "Name");
          model.CategoryId = 12;
          model.Categories = UCategoryService.GetCategoryListItems();
          //model.ContentNodes = GetNodes("ProductListing");

          return this.PartialView("~/Views/Partials/ImportPageView.cshtml", model);
      }

      private IEnumerable<SelectListItem> GetNodes(string docTypeName)
      {

          List<Node> foundNodes = new List<Node>();

          var node = new Node(-1);
          foreach (Node childNode in node.Children)
          {
              var child = childNode;
              if (child.NodeTypeAlias == docTypeName)
              {
                  foundNodes.Add(child);
              }
          }

          return new SelectList(foundNodes, "Id", "Name");
      }

  }
}