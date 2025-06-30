using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.ApplicationCore.Services
{
    public class AlbumService(IUnitOfWork unitOfWork, ILogger<GenericService<Album>> logger) : GenericService<Album>(unitOfWork, logger), IAlbumService
    {
    }
}
