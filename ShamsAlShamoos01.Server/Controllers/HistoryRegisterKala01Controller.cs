


using AutoMapper;
 using ShamsAlShamoos01.Infrastructure.Persistence.Contexts;
using ShamsAlShamoos01.Infrastructure.Persistence.Repositories;
using ShamsAlShamoos01.Server.Services;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using ShamsAlShamoos01.Shared.Entities;
using ShamsAlShamoos01.Infrastructure.Persistence.UnitOfWork;
using ShamsAlShamoos01.Shared.Models;

namespace ShamsAlShamoos01.Server.Controllers
{

    [Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
    [Microsoft.AspNetCore.Mvc.ApiController]

    public class HistoryRegisterKala01Controller : ControllerBase
    {
        public System.Collections.Generic.List<HistoryRegisterKala01ViewModelcat> _View_DailyPlanEvidence02 { get; set; }
        public static List<HistoryRegisterKala01ViewModelcat> PersonellistDailyPlan01 = new List<HistoryRegisterKala01ViewModelcat>();

        public string jsonSelectedMelliCode01
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
            set
            {
                var json = JsonConvert.SerializeObject(value);
                HttpContext.Session.SetString("SelectedMelliCode01", json);
            }
        }



        Expression<Func<HistoryRegisterKala01, bool>> TblMasterFilter = item => 1 == 1;




        private readonly IUnitOfWork _context;
        private readonly ILogger<HistoryRegisterKala01Controller> _logger;
        private readonly IDapperGenericRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUsers> _userManager;
         //private readonly IHttpContextAccessor _httpContextAccessor;
         private readonly ApplicationDbContext _contextDB;
        private readonly APIDataService01 _DataService;

        private PersianCalendar pc = new PersianCalendar();

        //@inject DailyPlanDataService DataService
        //private readonly APIDataService01 _DataService;
        //private readonly APIDataService01<UnitCountAmar01ViewModelcat> _DataUnitCountAmar01Service;
        //private readonly APIDataService01<DailyPlanEvidenceStatAmar01Summary01ViewModelcat> _DataPersonalStatAmarSummary01;

        public HistoryRegisterKala01Controller(ApplicationDbContext contextDB, ILogger<HistoryRegisterKala01Controller> logger,
                                IUnitOfWork context, IMapper mapper,
                             APIDataService01 dataService,
                        UserManager<ApplicationUsers> userManager ,
                                IDapperGenericRepository repository )
        {
            _logger = logger;
            _context = context;
            _repository = repository;
            _contextDB = contextDB;
            _DataService = dataService;
            //_DataPersonalStatAmarSummary01 = dataPersonalStatAmarSummary01;
            //_DataUnitCountAmar01Service = dataUnitCountAmar01Service;

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



        //public class LoadDataRequest
        //{
        //    public string UserId { get; set; }
        //    //public List<string> Roles { get; set; } = new();
        //    public DataManagerRequest DataManager { get; set; } = new();
        //}

        [HttpPost]
        public async Task<IActionResult> LoadData([FromBody] LoadDataRequest request)
        {
            try
            {

                string whereClauseHistoryRegisterKala01 = "1=1";
                DateTime strStartDate = DateTime.Now.Date;    // بدون AddDays(0) فقط Date
                DateTime strEndDate = DateTime.Now.Date.AddDays(1);
  
                var parameters = new DynamicParameters();
                string WhatViewSelect = "dbo.View_HistoryRegisterKala03_Tbl";
                parameters.Add("@ViewSelect", WhatViewSelect);
                //parameters.Add("@WHERE", whereClauseHistoryRegisterKala01 + "  ORDER BY   Date01 desc ");
                parameters.Add("@WHERE", whereClauseHistoryRegisterKala01 + "   ");
                List<HistoryRegisterKala01ViewModel_Update> GaurdActivity01 = _repository.ListFilter<HistoryRegisterKala01ViewModel_Update>("View_Dapper01", parameters, commandTimeout: 1300);
                _DataService.SetHistoryRegisterKala01_Update(request.UserId, GaurdActivity01);  // ذخیره در سرویس

                var dataSource = GaurdActivity01.ToList();
                IEnumerable result = dataSource;
                return Ok(result);              // یا
             }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoadData Error");
                return BadRequest("خطا در لود داده‌ها");
            }
        }



        private string BuildRoleBasedWhereClause(List<string> userRoles, string baseCondition, string unitCondition, string RegunitCondition, bool includeStatus01P, string Status01P, int countDailyPlans01Ready)
        {
            var whereClause = $"{baseCondition} {unitCondition}";
            var RegwhereClause = $"{baseCondition} {RegunitCondition}";

            bool isPass = Status01P == "PassSignature01";
            bool isWait = Status01P == "WaitForSignature01";
            bool NotCleare01 = Status01P == "NotCleare01";

            if (userRoles.Contains("HistoryRegisterKalaYEGAN"))
            {
                //return $"{baseCondition} AND StatusConfirmation03 = 320 ";

                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation02"))
                {
                    if (isPass)
                        return $"{RegwhereClause} AND StatusConfirmation02 = 320";
                    if (isWait)
                        return $"{RegwhereClause} AND StatusConfirmation02 = 319";
                }

                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation03"))
                {
                    if (isPass)
                        return $"{RegwhereClause} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 320";
                    if (NotCleare01)
                        return $"{RegwhereClause} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 321";
                    if (isWait)
                        return $"   StatusConfirmation02 = 320 AND StatusConfirmation03 = 319 {RegunitCondition}";
                }

                if (!userRoles.Contains("StatusHistoryRegisterKalaConfirmation02,StatusHistoryRegisterKalaConfirmation03"))
                {
                    return $"{whereClause} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 320";

                }

            }

            if (userRoles.Contains("HistoryRegisterKalaYEGAN00"))
            {
                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation02"))
                {
                    if (isPass)
                        return $"{baseCondition} AND StatusConfirmation02 = 320";
                    if (isWait)
                        return $"{baseCondition} AND StatusConfirmation02 = 319";
                }

                if (userRoles.Contains("StatusHistoryRegisterKalaConfirmation03"))
                {
                    if (isPass)
                        return $"{whereClause} AND StatusConfirmation02 = 320 AND StatusConfirmation03 = 320";
                    if (isWait)
                        return $"   StatusConfirmation02 = 320 AND StatusConfirmation03 = 319 {unitCondition}";
                }
            }
            //AND
            if (userRoles.Contains("HistoryRegisterKalaALL") && Status01P == "AllPassSignature01")
            {
                return $"{baseCondition} AND StatusConfirmation03 = 320 ";
            }

            if (userRoles.Contains("HistoryRegisterKalaPayvar"))
            {
                return $"{baseCondition} AND StatusConfirmation03 = 320 AND TblLuLookupSubbId not in ('8','10','12','13')";
            }


            if (userRoles.Contains("HistoryRegisterKalaVazifeh"))
            {
                return $"{baseCondition} AND StatusConfirmation03 = 320 AND TblLuLookupSubbId  in ('8','10','12','13')";
            }




            //                            case "Payvar":
            //                userIdFilter = item => Convert.ToDecimal(item.DocumentNO01.Substring(0, 10).Trim().Replace("/", "")) >= Convert.ToDecimal(dm.StartDate.Replace("/", ""))
            //&& Convert.ToDecimal(item.DocumentNO01.Substring(0, 10).Trim().Replace("/", "")) < Convert.ToDecimal(dm.EndDate.Replace("/", ""))
            //&& (item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId != 8 && item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId != 10 && item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId != 12 && item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId != 13)
            //;
            //                break;



            //                break;

            //            case "Vazifeh":

            //                userIdFilter = item => Convert.ToDecimal(item.DocumentNO01.Substring(0, 10).Trim().Replace("/", "")) >= Convert.ToDecimal(dm.StartDate.Replace("/", ""))
            //&& Convert.ToDecimal(item.DocumentNO01.Substring(0, 10).Trim().Replace("/", "")) < Convert.ToDecimal(dm.EndDate.Replace("/", ""))
            //&& (item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId == 8 || item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId == 10 || item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId == 12 || item.oo_CrewDailyPesronel.ooDRJCOD.TblLuLookupSubbId == 13)

            //    ;












            return "1 = 0"; // دسترسی ندارند
        }



        [HttpPost]
        public async void OnPostRemove([FromBody] CRUDModel<HistoryRegisterKala01ViewModel_Update> value)
        {
            _context.HistoryRegisterKala01UW.DeleteById(value.Key.ToString());
            //await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "refresh");

            _context.save();

        }
        public class CombinedInsertRequestModel
        {
            public Syncfusion.Blazor.DataManagerRequest DataRequest { get; set; }
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
                var dateDoc02 = $"{pc.GetYear(now):0000}/{pc.GetMonth(now):00}/{pc.GetDayOfMonth(now):00}";
                var dateDoc01 = model.StartDate;
                var lastDocNo = _context.HistoryRegisterKala01UW
                      .Get(u => u.DocumentNO01.StartsWith(dateDoc01), q => q.OrderByDescending(d => d.DocumentNO01))
                      .Select(u => u.DocumentNO01)
                      .FirstOrDefault();

                //var newDocNo = CreateSerialNOPlan01.CreateNewNoFolder01(lastDocNo, dateDoc01);
                var viewModel = model.CrudModel.Value;
                //string UjobPesronelID = _context.UjobPesronel01UW.Get(u => u.IsHaveJob == true && u.UserID == model.UserId).Select(u => u.UjobPesronel01ID).Take(1).ToList().FirstOrDefault();

                //viewModel.DocumentNO01 = newDocNo;
                viewModel.StatusConfirmation01 = 320;
                viewModel.StatusConfirmation02 = viewModel.StatusConfirmation03 =
                viewModel.StatusConfirmation04 = viewModel.StatusConfirmation05 =
                viewModel.StatusConfirmation06 = 319;
                //viewModel.Vartext01 = viewModel.Vartext01.Trim();
                //viewModel.Vartext02 = viewModel.Vartext02.Trim();
                //viewModel.Vartext03 = viewModel.Vartext03.Trim();
                //viewModel.Vartext04 = viewModel.Vartext04.Trim();
                //viewModel.Vartext05 = viewModel.Vartext05.Trim();
                //viewModel.Vartext06 = viewModel.Vartext06.Trim();
                var entity = _mapper.Map<HistoryRegisterKala01>(viewModel);
                 entity.HistoryRegisterKala01ID = Guid.NewGuid().ToString();

                _context.HistoryRegisterKala01UW.Create(entity);
                _context.save();

                var parameters = new DynamicParameters();
                parameters.Add("@HistoryRegisterKala01ID", entity.HistoryRegisterKala01ID);

                var updatedData = _repository.ListFilter<HistoryRegisterKala01ViewModel_Update>(
                    "Update_HistoryRegisterKala01_DocumentNO01", parameters, commandTimeout: 1300);


                //string whereClauseHistoryRegisterKala01 = $"HistoryRegisterKala01ID IN ('{entity.HistoryRegisterKala01ID}') ";

                //var parameters1 = new DynamicParameters();
                //string WhatViewSelect = "dbo.View_HistoryRegisterKala03_Tbl";
                //parameters1.Add("@ViewSelect", WhatViewSelect);
                //parameters1.Add("@WHERE", whereClauseHistoryRegisterKala01 + " and strTextContent01 is not null ORDER BY UnitID01, DarajeeGheshrID, DRJ_COD DESC, EMP_NUM, Date01 ,TypeLetter01");

                //List<HistoryRegisterKala01ViewModel_Update> data11 = _repository.ListFilter<HistoryRegisterKala01ViewModel_Update>("View_Dapper01", parameters1, commandTimeout: 1300);

                //var finalRecord = data11.FirstOrDefault();


                //var viewModelResult = _mapper.Map<HistoryRegisterKala01ViewModel_Update>(finalRecord);
                //return new JsonResult(viewModelResult);


                //return new JsonResult(finalRecord);

                return Ok(1);


            }
            catch (Exception ex)
            {
                // TODO: Log exception ex
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
        //[HttpPut]
        //public async Task<IActionResult> OnPostUpdate([FromBody] LoadDataRequest request, [FromBody] CRUDModel<HistoryRegisterKala01ViewModel_Update> model.CrudModel)
        public async Task<IActionResult> OnPostUpdate([FromBody] CombinedRequestModel model)
        {





            try
            {
                if (model.CrudModel?.Value == null)
                    return BadRequest("Invalid data");

                var data = _context.HistoryRegisterKala01UW.GetById(model.CrudModel.Value.HistoryRegisterKala01ID);
                if (data == null)
                    return NotFound("Record not found");

                // به‌روزرسانی فیلدها

 

 
                if (model.Roles.Contains("StatusHistoryRegisterKalaConfirmation02"))
                {

                    data.Vartext01 = model.CrudModel.Value.Vartext01;
                    data.Vartext02 = model.CrudModel.Value.Vartext02;
                    data.Vartext03 = model.CrudModel.Value.Vartext03;
                    data.Vartext04 = model.CrudModel.Value.Vartext04;
                    data.Vartext05 = model.CrudModel.Value.Vartext05;
                    data.Vartext06 = model.CrudModel.Value.Vartext06;
                    //data.Vartext07 = model.CrudModel.Value.Vartext07;
                    //data.Vartext08 = model.CrudModel.Value.Vartext08;
                    data.Vartext09 = model.CrudModel.Value.Vartext09;
                    data.Vartext10 = model.CrudModel.Value.Vartext10;
                    data.Vartext11 = model.CrudModel.Value.Vartext11;
                    data.Vartext12 = model.CrudModel.Value.Vartext12;
                    data.Vartext13 = model.CrudModel.Value.Vartext13;
                    data.Vartext14 = model.CrudModel.Value.Vartext14;
                    data.Vartext15 = model.CrudModel.Value.Vartext15;
                    data.Vartext16 = model.CrudModel.Value.Vartext16;
                    data.Vartext17 = model.CrudModel.Value.Vartext17;
                    data.Vartext18 = model.CrudModel.Value.Vartext18;
                    data.Vartext19 = model.CrudModel.Value.Vartext19;
                    data.Vartext20 = model.CrudModel.Value.Vartext20;

                }
                if (model.Roles.Contains("StatusHistoryRegisterKalaConfirmation04"))
                {

                    //data.Vartext01 = model.CrudModel.Value.Vartext01;
                    //data.Vartext02 = model.CrudModel.Value.Vartext02;
                    //data.Vartext03 = model.CrudModel.Value.Vartext03;
                    //data.Vartext04 = model.CrudModel.Value.Vartext04;
                    //data.Vartext05 = model.CrudModel.Value.Vartext05;
                    data.Vartext07 = model.CrudModel.Value.Vartext07;

                }
 
                _context.HistoryRegisterKala01UW.Update(data);
                _context.save(); // استفاده از نسخه Async



                var parameters = new DynamicParameters();
                parameters.Add("@HistoryRegisterKala01ID", data.HistoryRegisterKala01ID);

                var updatedData = _repository.ListFilter<HistoryRegisterKala01ViewModel_Update>(
                    "Update_HistoryRegisterKala01_DocumentNO01", parameters, commandTimeout: 1300);


                //if (model.CrudModel.Value.StatusConfirmation02 == 320 && model.Roles.Contains("StatusHistoryRegisterKalaConfirmation02"))
                //{
                //    await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "refresh");

                //}

                //await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "refresh");
                //await Clients.Others.SendAsync("ReceiveUpdatedData", updatedRecord);

                return Ok(model.CrudModel.Value);
            }
            catch (Exception ex)
            {
                // لاگ خطا
                return StatusCode(500, "Internal server error");
            }
        }
    }
}