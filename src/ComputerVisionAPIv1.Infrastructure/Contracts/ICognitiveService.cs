using ComputerVisionAPIv1.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVisionAPIv1.Infrastructure.Contracts
{
    public interface ICognitiveService
    {
        Task<Image> ProcessImage(string imageUrl);
    }
}
