using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShamsAlShamoos01.Infrastructure.Persistence.Contexts;
using ShamsAlShamoos01.Infrastructure.Persistence.Repositories;
using ShamsAlShamoos01.Infrastructure.Persistence.UnitOfWork;
using ShamsAlShamoos01.Server.Services;
using ShamsAlShamoos01.Shared.Entities;
using ShamsAlShamoos01.Shared.Models;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Globalization;
using System.Linq.Expressions;

namespace ShamsAlShamoos01.Server.Controllers
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

        public static List<HistoryRegisterKala01ViewModelcat> PersonellistDailyPlan01 = new();
        public List<HistoryRegisterKala01ViewModelcat> _View_DailyPlanEvidence02 { get; set; }

        public string JsonSelectedMelliCode01
        {
            get => HttpContext.Session.GetString("jsonSelectedMelliCode01") ?? "";
            set => HttpContext.Session.SetString("jsonSelectedMelliCode01", value);
        }

        public List<string> SelectedMelliCode01
        {
            get
            {
                var value = HttpContext.Session.GetString("SelectedMelliCode01");
                return string.IsNullOrEmpty(value) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(value);
            }
            set => HttpContext.Session.SetString("SelectedMelliCode01", JsonConvert.SerializeObject(value));
        }

        private readonly Expression<Func<HistoryRegisterKala01, bool>> _tblMasterFilter = item => true;

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

        public class TestRequest
        {
            public string Name { get; set; }
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

                var parameters = new DynamicParameters();
                parameters.Add("@ViewSelect", viewName);
                parameters.Add("@WHERE", whereClause);

                var guardActivity = _repository.ListFilter<HistoryRegisterKala01ViewModel_Update>(
                    "View_Dapper01", parameters, commandTimeout: 1300);

                _dataService.SetHistoryRegisterKala01_Update(request.UserId, guardActivity);

                return Ok(guardActivity.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadData Error");
                return BadRequest("خطا در لود داده‌ها");
            }
        }

        private static string BuildRoleBasedWhereClause(
            List<string> userRoles,
            string baseCondition,
            string unitCondition,
            string regUnitCondition,
            string status01P)
        {
            bool isPass = status01P == "PassSignature01";
            bool isWait = status01P == "WaitForSignature01";
            bool notClear = status01P == "NotCleare01";

            /* ---------------------------------------------------------
             *  سطح اول – نقش YEGAN
             * --------------------------------------------------------- */
            if (userRoles.Contains("HistoryRegisterKalaYEGAN"))
            {
                // نقش امضای دوم
                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation02"))
                {
                    if (isPass)
                    {
                        return $"{regUnitCondition} AND StatusConfirmation02 = 320";
                    }

                    if (isWait)
                    {
                        return $"{regUnitCondition} AND StatusConfirmation02 = 319";
                    }
                }

                // نقش امضای سوم
                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation03"))
                {
                    if (isPass)
                    {
                        return $"{regUnitCondition} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 320";
                    }

                    if (notClear)
                    {
                        return $"{regUnitCondition} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 321";
                    }

                    if (isWait)
                    {
                        return $"{regUnitCondition} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 319";
                    }
                }

                // اگر هیچ‌کدام از نقش‌های امضا ۲ و ۳ نبود
                if (!userRoles.Contains("StatusHistoryRegisterKalaConfirmation02") &&
                    !userRoles.Contains("StatusHistoryRegisterKalaConfirmation03"))
                {
                    return $"{unitCondition} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 320";
                }
            }

            /* ---------------------------------------------------------
             *  سطح دوم – نقش YEGAN00
             * --------------------------------------------------------- */
            if (userRoles.Contains("HistoryRegisterKalaYEGAN00"))
            {
                // امضا ۲
                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation02"))
                {
                    if (isPass)
                    {
                        return $"{baseCondition} AND StatusConfirmation02 = 320";
                    }

                    if (isWait)
                    {
                        return $"{baseCondition} AND StatusConfirmation02 = 319";
                    }
                }

                // امضا ۳
                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation03"))
                {
                    if (isPass)
                    {
                        return $"{unitCondition} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 320";
                    }

                    if (isWait)
                    {
                        return $"{unitCondition} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 319";
                    }
                }
            }

            /* ---------------------------------------------------------
             *  تاریخچه ALL
             * --------------------------------------------------------- */
            if (userRoles.Contains("HistoryRegisterKalaALL") &&
                status01P == "AllPassSignature01")
            {
                return $"{baseCondition} AND StatusConfirmation03 = 320";
            }

            /* ---------------------------------------------------------
             *  پایور
             * --------------------------------------------------------- */
            if (userRoles.Contains("HistoryRegisterKalaPayvar"))
            {
                return $"{baseCondition} AND StatusConfirmation03 = 320 AND TblLuLookupSubbId NOT IN ('8','10','12','13')";
            }

            /* ---------------------------------------------------------
             *  وظیفه
             * --------------------------------------------------------- */
            if (userRoles.Contains("HistoryRegisterKalaVazifeh"))
            {
                return $"{baseCondition} AND StatusConfirmation03 = 320 AND TblLuLookupSubbId IN ('8','10','12','13')";
            }

            /* ---------------------------------------------------------
             *  پیش‌فرض – عدم دسترسی
             * --------------------------------------------------------- */
            return "1 = 0";
        }

        [HttpPost]
        public async Task<IActionResult> OnPostRemove([FromBody] CRUDModel<HistoryRegisterKala01ViewModel_Update> value)
        {
            _context.HistoryRegisterKala01UW.DeleteById(value.Key.ToString());
            _context.Save();
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
                var now = DateTime.Now;
                var dateDoc02 = $"{_persianCalendar.GetYear(now):0000}/{_persianCalendar.GetMonth(now):00}/{_persianCalendar.GetDayOfMonth(now):00}";
                var dateDoc01 = model.StartDate;

                var lastDocNo = _context.HistoryRegisterKala01UW
                    .Get(u => u.DocumentNO01.StartsWith(dateDoc01), q => q.OrderByDescending(d => d.DocumentNO01))
                    .Select(u => u.DocumentNO01)
                    .FirstOrDefault();

                var viewModel = model.CrudModel.Value;

                // Set confirmation statuses
                viewModel.StatusConfirmation01 = 320;
                viewModel.StatusConfirmation02 = viewModel.StatusConfirmation03 =
                viewModel.StatusConfirmation04 = viewModel.StatusConfirmation05 =
                viewModel.StatusConfirmation06 = 319;

                var entity = _mapper.Map<HistoryRegisterKala01>(viewModel);
                entity.HistoryRegisterKala01ID = Guid.NewGuid().ToString();

                _context.HistoryRegisterKala01UW.Create(entity);
                _context.Save();

                // Update document number
                var parameters = new DynamicParameters();
                parameters.Add("@HistoryRegisterKala01ID", entity.HistoryRegisterKala01ID);

                _repository.ListFilter<HistoryRegisterKala01ViewModel_Update>(
                    "Update_HistoryRegisterKala01_DocumentNO01", parameters, commandTimeout: 1300);

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
                    return BadRequest("Invalid data");

                var data = _context.HistoryRegisterKala01UW.GetById(model.CrudModel.Value.HistoryRegisterKala01ID);
                if (data == null)
                    return NotFound("Record not found");

                UpdateEntityBasedOnRoles(data, model.CrudModel.Value, model.Roles);

                _context.HistoryRegisterKala01UW.Update(data);
                _context.Save();

                // Update document number
                var parameters = new DynamicParameters();
                parameters.Add("@HistoryRegisterKala01ID", data.HistoryRegisterKala01ID);

                _repository.ListFilter<HistoryRegisterKala01ViewModel_Update>(
                    "Update_HistoryRegisterKala01_DocumentNO01", parameters, commandTimeout: 1300);

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
                UpdateVartextFields02(entity, viewModel);
            }

            if (roles.Contains("StatusHistoryRegisterKalaConfirmation04"))
            {
                entity.Vartext07 = viewModel.Vartext07;
            }
        }

        private static void UpdateVartextFields02(HistoryRegisterKala01 entity, HistoryRegisterKala01ViewModel_Update viewModel)
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
    }
}