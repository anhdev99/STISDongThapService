using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public abstract class BaseService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly ILogger<BaseService> _logger;
    protected readonly ApplicationDbContext _dbContext;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;
    protected BaseService(IHttpContextAccessor httpContextAccessor, ILogger<BaseService> logger, ApplicationDbContext dbContext, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Lấy userId từ token (claim "userId").
    /// </summary>
    protected string? ProfileCode
    {
        get
        {
            return _httpContextAccessor.HttpContext?.Items["ProfileCode"]?.ToString();
           
        }
    }
    
    protected string? UserName 
    {
        get
        {
            return _httpContextAccessor.HttpContext?.Items["UserName"]?.ToString();
           
        }
    }

    protected List<string> Roles 
    {
        get
        {
            try
            {
                return _httpContextAccessor.HttpContext?.Items["Roles"] as List<string>;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }
    }
    /// <summary>
    /// Phương thức để ghi log thông tin.
    /// </summary>
    protected void LogInformation(string message)
    {
        _logger.LogInformation(message);
    }

    /// <summary>
    /// Phương thức để ghi log lỗi.
    /// </summary>
    protected void LogError(string message, Exception ex)
    {
        _logger.LogError(ex, message);
    }
}