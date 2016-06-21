using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ComputerVisionAPIv1.Infrastructure;
using Microsoft.Extensions.Configuration;
using ComputerVisionAPIv1.Infrastructure.Contracts;
using ComputerVisionAPIv1Sample.Web.ViewModels;
using System.Net;

namespace ComputerVisionAPIv1Sample.Web.Controllers
{
    public class CognitiveController : Controller
    {
        private ICognitiveService _cognitiveService { get; set; }
        private IImageRepository _imageRepository { get; set; }

        /// <summary>
        /// Controller Constructore
        /// </summary>
        /// <param name="cognitiveService">Computer Vision API Service Provider</param>
        /// <param name="imageRepository">Image Repository instance</param>
        public CognitiveController(ICognitiveService cognitiveService, IImageRepository imageRepository)
        {
            _cognitiveService = cognitiveService;
            _imageRepository = imageRepository;
        }

        /// <summary>
        /// Used to process an image by Microsoft Cognitive API and store the result into Azure DocumentDB
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task ProcessImage([FromBody]ProcessImagePayload payload)
        {
            if(ModelState.IsValid)
            {
                var image = await _cognitiveService.ProcessImage(payload.Url);
                if (image != null)
                {
                    await _imageRepository.CreateAsync(image);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
