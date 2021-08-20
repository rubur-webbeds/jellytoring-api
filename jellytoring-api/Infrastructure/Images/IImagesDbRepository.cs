﻿using jellytoring_api.Models.Images;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Images
{
    public interface IImagesDbRepository
    {
        Task<uint> CreateAsync(uint userId, Image image);
        Task<Image> GetAsync(uint imageId);
        Task<IEnumerable<Image>> GetUserImagesAsync(uint userId);
    }
}