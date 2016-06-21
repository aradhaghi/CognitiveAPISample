using ComputerVisionAPIv1.Infrastructure.Models;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVisionAPIv1.Infrastructure.Contracts
{
    public interface IImageRepository
    {
        Task<Document> CreateAsync(Image image);
    }
}
