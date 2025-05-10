using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class PermissionService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<BaseService> logger,
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : BaseService(httpContextAccessor, logger, dbContext, unitOfWork, mapper);