using AbleMods.Api.Models;
using AbleMods.Dto;

namespace AbleMods.Services
{
    public static class AcProductReviewService
    {

        private static acProductReviewDataService _service;

        static AcProductReviewService()
        {
            _service = new acProductReviewDataService();
        }

        public static bool SaveReview(UProductReview ableReview)
        {
            AcProductReviewDto newDto = MakeDto(ableReview);
            return _service.AddOrUpdate(newDto);
        }

        private static AcProductReviewDto MakeDto(UProductReview review)
        {
            AcProductReviewDto retVal = new AcProductReviewDto();
            retVal.Email = review.Email;
            retVal.ProductId = review.ProductId;
            retVal.Rating = review.Rating;
            retVal.ReviewBody = review.ReviewBody;
            retVal.ReviewDate = review.ReviewDate;
            retVal.ReviewTitle = review.ReviewTitle;

            return retVal;
        }
    }
}