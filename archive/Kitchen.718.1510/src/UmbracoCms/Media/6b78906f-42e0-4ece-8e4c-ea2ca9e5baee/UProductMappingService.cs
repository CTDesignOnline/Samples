using System;
using System.Collections.Generic;
using AbleMods.Api;
using AbleMods.Api.Models;
using AbleMods.Dto;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace AbleMods.Services
{
    public static class UProductMappingService
    {
        //Get the Umbraco Database context
        public static UmbracoDatabase _db;
        public static string _dbName = "legacyAcProduct";
        private static acProductDataService _service;

        static UProductMappingService()
        {
            _db = ApplicationContext.Current.DatabaseContext.Database;
            _service = new acProductDataService();

            // make sure table exists
            CreateLegacyTable();
        }

        public static void Save(int acProductId, int acProductVariantId, string optionList, Guid merchelloProductKey, Guid merchelloProductVariantKey, string acUrl, string acSku)
        {
            // make sure table exists
            CreateLegacyTable();

            // create new object and save it
            AcProductDto newDto = new AcProductDto();

            newDto.AcProductId = acProductId;
            newDto.AcProductVariantId = acProductVariantId;
            newDto.AcOptionList = optionList;
            newDto.ProductKey = merchelloProductKey;
            newDto.ProductVariantKey = merchelloProductVariantKey;
            newDto.AcUrl = acUrl;
            newDto.AcSku = acSku;

            _service.AddOrUpdate(newDto);
       }


        public static void CreateLegacyTable()
        {
            // Check if the DB table does NOT exist
            if (!_db.TableExist(_dbName))
            {
                //Create DB table - and set overwrite to false
                _db.CreateTable<AcProductDto>(false);
            }
        }

        public static void DeleteLegacyTable()
        {
            if (_db.TableExist(_dbName))
            {
                // delete the db table
                _db.DropTable<AcProductDto>();
            }
        }

    }
}