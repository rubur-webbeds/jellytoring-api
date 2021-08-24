﻿using jellytoring_api.Infrastructure.Images;
using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Email;
using jellytoring_api.Models.Email.Template;
using jellytoring_api.Models.Images;
using jellytoring_api.Service.Email;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Images
{
    public class ImagesService : IImagesService
    {
        private readonly IImagesDbRepository _imagesDbRepository;
        private readonly IImagesDiskRepository _imagesDiskRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IEmailService _emailService;

        public ImagesService(
            IImagesDbRepository imagesDbRepository,
            IImagesDiskRepository imagesDiskRepository,
            IUsersRepository usersRepository,
            IEmailService emailService)
        {
            _imagesDbRepository = imagesDbRepository;
            _imagesDiskRepository = imagesDiskRepository;
            _usersRepository = usersRepository;
            _emailService = emailService;
        }

        public async Task<Image> GetAsync(uint imageId)
        {
            var dbImage = await _imagesDbRepository.GetAsync(imageId);
            return dbImage;
        }

        public async Task<Image> CreateAsync(string userEmail, Image image)
        {
            if (!Validate(image))
            {
                return null;
            }

            var user = await _usersRepository.GetAsync(userEmail);

            var newFilename = Guid.NewGuid();
            var extension = ContentTypeToExtension(image.File.ContentType);
            if (extension == "error")
            {
                return null;
            }

            image.Filename = $"{newFilename}.{extension}";

            var imageId = await _imagesDbRepository.CreateAsync(user.Id, image);

            if (imageId != 0)
            {
                await _imagesDiskRepository.SaveAsync(image);
            }

            // TODO: probably move to ImageApprovalService.
            // In the future we may need to send the approvation through a new channel as well. P.e. send a notification in the UI
            var imageApprovalTemplate =
                new TemplateBuilder()
                .SetName("ImageApprovalTemplate.html")
                .Build();

            var templateReq = new EmailTemplateRequest
            {
                // TODO: set admin email from config
                EmailRequest = new EmailRequest { To = "rubur100@gmail.com", Subject = "New upload to approve! How cool is that?" },
                Template = imageApprovalTemplate
            };

            await _emailService.SendEmailTemplateAsync(templateReq);

            return imageId != 0 ? await GetAsync(imageId) : null;
        }

        public async Task<IEnumerable<Image>> GetUserImagesAsync(string userEmail)
        {
            var user = await _usersRepository.GetAsync(userEmail);

            return await _imagesDbRepository.GetUserImagesAsync(user.Id);
        }

        private string ContentTypeToExtension(string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg":
                    return "jpeg";
                case "image/png":
                    return "png";
                default:
                    return "error";
            }
        }

        private bool Validate(Image image)
        {
            // file extension validation
            string[] permittedExtensions = { ".jpeg", ".jpg", ".png" };
            var imgExtension = Path.GetExtension(image.File.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(imgExtension) || !permittedExtensions.Contains(imgExtension))
            {
                return false;
            }

            // file singnature validation
            var _fileSignature = new Dictionary<string, List<byte[]>>
            {
                {
                ".jpeg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    }
                },
                {
                ".jpg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    }
                },
                { ".png", new List<byte[]>
                    {
                    new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                    }
                }
            };

            using (var reader = new BinaryReader(image.File.OpenReadStream()))
            {
                var signatures = _fileSignature[imgExtension];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }
}
