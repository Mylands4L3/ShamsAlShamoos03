using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShamsAlShamoos02.Infrastructure.Persistence.Contexts;
using ShamsAlShamoos02.Infrastructure.Persistence.Repositories;
using ShamsAlShamoos02.Infrastructure.Persistence.UnitOfWork;
using ShamsAlShamoos02.Server.Services;
using ShamsAlShamoos02.Shared.Entities;
using ShamsAlShamoos02.Shared.Models;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Globalization;
using System.Linq.Expressions;

namespace ShamsAlShamoos02.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HistoryRegisterKala01Controller : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ILogger<HistoryRegisterKala01Controller> _logger;
        private readonly IDapperGenericRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly ApplicationDbContext _contextDB;
        private readonly APIDataService01 _dataService;
        private readonly PersianCalendar _persianCalendar = new();

        public HistoryRegisterKala01Controller(
            ApplicationDbContext contextDB,
            ILogger<HistoryRegisterKala01Controller> logger,
            IUnitOfWork context,
            IMapper mapper,
            APIDataService01 dataService,
            UserManager<ApplicationUsers> userManager,
            IDapperGenericRepository repository)
        {
            _logger = logger;
            _context = context;
            _repository = repository;
            _contextDB = contextDB;
            _dataService = dataService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Test([FromBody] TestRequest request)
        {
            return Ok(new { message = $"Hello {request.Name}" });
        }

        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody] LoadDataRequest request)
        {
            try
            {
                const string whereClause = "1=1";
                const string viewName = "dbo.View_HistoryRegisterKala03_Tbl";

                var parameters = new { ViewSelect = viewName, WHERE = whereClause };

                var guardActivity = await _repository.ListAsync<HistoryRegisterKala01ViewModel_Update>(
                    "View_Dapper01", parameters, 1300);

                _dataService.SetHistoryRegisterKala01_Update(request.UserId, guardActivity);

                return Ok(guardActivity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadData Error");
                return BadRequest("خطا در لود داده‌ها");
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostRemove([FromBody] CRUDModel<HistoryRegisterKala01ViewModel_Update> value)
        {
            var repo = _context.Repository<HistoryRegisterKala01>();

            var entity = await repo.GetByIdAsync(value.Key);
            if (entity == null)
            {
                return NotFound("رکورد پیدا نشد");
            }

            repo.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
        public class CombinedInsertRequestModel
        {
            public DataManagerRequest DataRequest { get; set; }
            public CRUDModel<HistoryRegisterKala01ViewModel_Update> CrudModel { get; set; }
            public List<string> Roles { get; set; }
            public int? YeganUser { get; set; }
            public string StartDate { get; set; }
            public string UserId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostInsert([FromBody] CombinedInsertRequestModel model)
        {
            try
            {
                var dateDoc01 = model.StartDate;

                var parametersGet = new DynamicParameters();
                parametersGet.Add("@StartDate", dateDoc01);

                var allRecords = await _context.Dapper.ListAsync<HistoryRegisterKala01>(
                    "Get_HistoryRegisterKala01_ByStartDate", parametersGet, 1300);

                var lastDocNo = allRecords
                    .OrderByDescending(d => d.DocumentNO01)
                    .Select(u => u.DocumentNO01)
                    .FirstOrDefault();

                var viewModel = model.CrudModel.Value;

                // مقداردهی وضعیت‌ها
                viewModel.StatusConfirmation01 = 320;
                viewModel.StatusConfirmation02 = viewModel.StatusConfirmation03 =
                viewModel.StatusConfirmation04 = viewModel.StatusConfirmation05 =
                viewModel.StatusConfirmation06 = 319;

                var entity = _mapper.Map<HistoryRegisterKala01>(viewModel);
                entity.HistoryRegisterKala01ID = Guid.NewGuid().ToString();

                await _context.InsertAsync(entity);

                var parametersUpdate = new DynamicParameters();
                parametersUpdate.Add("@HistoryRegisterKala01ID", entity.HistoryRegisterKala01ID);

                await _context.Dapper.ListAsync<HistoryRegisterKala01ViewModel_Update>(
                    "Update_HistoryRegisterKala01_DocumentNO01", parametersUpdate, 1300);

                return Ok(1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OnPostInsert Error");
                return StatusCode(500, "خطایی در سرور رخ داده است");
            }
        }
        public class CombinedRequestModel
        {
            public DataManagerRequest DataRequest { get; set; }
            public CRUDModel<HistoryRegisterKala01ViewModel_Update> CrudModel { get; set; }
            public List<string> Roles { get; set; }
            public string UserId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostUpdate([FromBody] CombinedRequestModel model)
        {
            try
            {
                if (model.CrudModel?.Value == null)
                {
                    return BadRequest("Invalid data");
                }

                var data = await _context.HistoryRegisterKala01UW
                    .GetByIdAsync(model.CrudModel.Value.HistoryRegisterKala01ID);

                if (data == null)
                {
                    return NotFound("Record not found");

                }

                UpdateEntityBasedOnRoles(data, model.CrudModel.Value, model.Roles);

                _context.HistoryRegisterKala01UW.Update(data);
                await _context.SaveChangesAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@HistoryRegisterKala01ID", data.HistoryRegisterKala01ID);

                await _repository.ListAsync<HistoryRegisterKala01ViewModel_Update>(
                    "Update_HistoryRegisterKala01_DocumentNO01", parameters, 1300);

                return Ok(model.CrudModel.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OnPostUpdate Error");
                return StatusCode(500, "Internal server error");
            }
        }

        private static void UpdateEntityBasedOnRoles(HistoryRegisterKala01 entity,
            HistoryRegisterKala01ViewModel_Update viewModel, List<string> roles)
        {
            if (roles.Contains("StatusHistoryRegisterKalaConfirmation02"))
            {
                entity.Vartext01 = viewModel.Vartext01;
                entity.Vartext02 = viewModel.Vartext02;
                entity.Vartext03 = viewModel.Vartext03;
                entity.Vartext04 = viewModel.Vartext04;
                entity.Vartext05 = viewModel.Vartext05;
                entity.Vartext06 = viewModel.Vartext06;
                entity.Vartext09 = viewModel.Vartext09;
                entity.Vartext10 = viewModel.Vartext10;
                entity.Vartext11 = viewModel.Vartext11;
                entity.Vartext12 = viewModel.Vartext12;
                entity.Vartext13 = viewModel.Vartext13;
                entity.Vartext14 = viewModel.Vartext14;
                entity.Vartext15 = viewModel.Vartext15;
                entity.Vartext16 = viewModel.Vartext16;
                entity.Vartext17 = viewModel.Vartext17;
                entity.Vartext18 = viewModel.Vartext18;
                entity.Vartext19 = viewModel.Vartext19;
                entity.Vartext20 = viewModel.Vartext20;
            }

            if (roles.Contains("StatusHistoryRegisterKalaConfirmation04"))
            {
                entity.Vartext07 = viewModel.Vartext07;
            }
        }

        public class TestRequest
        {
            public string Name { get; set; }
        }
    }

    // کلاس LoadDataRequest
    //public class LoadDataRequest
    //{
    //    public string UserId { get; set; }
    //    public int Skip { get; set; }
    //    public int Take { get; set; }
    //    public string Filter { get; set; }
    //    public string Sort { get; set; }
    //}
}
