using System.Collections.Generic;
using AbleMods.Api;
using AbleMods.Api.Models;
using AbleMods.Dto;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace AbleMods.Services
{
    public static class UProductReviewService
    {
        //Get the Umbraco Database context
        public static UmbracoDatabase _db;
        public static string _dbName = "legacyacProductReviews";

        static UProductReviewService()
        {
            _db = ApplicationContext.Current.DatabaseContext.Database;

            // make sure table exists
            CreateLegacyTable();
        }

        public static void ImportReviews()
        {
            // pull in reviews for one product
            UProductReviewRepository repo = new UProductReviewRepository();
            //List<UProductReview> ableReviews = repo.LoadforProductId(3037);
            List<UProductReview> ableReviews = repo.LoadAll();

            // write the reviews to the new table
            foreach (UProductReview review in ableReviews)
            {
                AcProductReviewService.SaveReview(review);
            }
        }


        public static void CreateLegacyTable()
        {
            // Check if the DB table does NOT exist
            if (!_db.TableExist(_dbName))
            {
                //Create DB table - and set overwrite to false
                _db.CreateTable<AcProductReviewDto>(false);
            }
        }

        public static void DeleteLegacyTable()
        {
            if (_db.TableExist(_dbName))
            {
                // delete the db table
                _db.DropTable<AcProductReviewDto>();
            }
        }

    }
}