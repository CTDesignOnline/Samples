using System;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace AbleMods.Services
{
    public class MediaImportService
    {
        private readonly IMediaService _mediaService;

        public MediaImportService()
            : this(ApplicationContext.Current.Services.MediaService)
        {            
        }

        public MediaImportService(IMediaService mediaService)
        {
            if (mediaService == null) throw new NullReferenceException("MediaService cannot be null");

            _mediaService = mediaService;
        }

        public IMedia ImportMedia(string virtualPath, string name, int parentMediaFolderId)
        {
            var media = _mediaService.CreateMedia(name, parentMediaFolderId, Constants.Conventions.MediaTypes.Image);

            // Save the media so that it has a media Id.  This makes the empty "bucket"
            _mediaService.Save(media);

            var file = new FileImportWrapper(IOHelper.MapPath(virtualPath));

            media.SetValue(Constants.Conventions.Media.File, file);

            _mediaService.Save(media);

            return media;
        }
    }
}