using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVisionAPIv1Sample.Web.ViewModels
{
    /// <summary>
    /// Request Payload for Cognitive Image Processing API
    /// </summary>
    public class ProcessImagePayload
    {
        /// <summary>
        /// URL of the image 
        /// </summary>
        public string Url { get; set; }
    }
}
