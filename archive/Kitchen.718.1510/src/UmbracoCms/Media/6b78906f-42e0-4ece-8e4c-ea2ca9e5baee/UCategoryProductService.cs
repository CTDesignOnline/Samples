using System.Collections;
using System.Collections.Generic;
using AbleMods.Api;
using AbleMods.Api.Models;

namespace AbleMods.Services
{
    public static class UCategoryProductService
    {
        public static IList<UProduct> LoadForProductId(int catId)
        {
            return UCategoryProductRepository.LoadAll(catId);
        }
         
    }
}