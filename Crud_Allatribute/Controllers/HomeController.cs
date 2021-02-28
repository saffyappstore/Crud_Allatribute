using BusienssLayer;
using DataLayer;
using Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModelLayer;

namespace Crud_Allatribute.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UserEntities db = new UserEntities();
            ProfileViewModel model = new ProfileViewModel();
            ViewBag.Cities = db.tbl_city.Select(x=>new SelectListItem { 
              Value=x.Cityid.ToString(),
              Text=x.Cityname
            }).ToList();
  

            return View(model);
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        public ActionResult UserProfile(int? id)
        {
            UserEntities db = new UserEntities();
            ProfileViewModel model = new ProfileViewModel();
            HomeService homeService = new HomeService();
            //HomeViewModel
            var city = db.tbl_city.ToList();
            var a = homeService.optionListState();
            model.Cities = db.tbl_city.Select(x => new citymodel
            {
                cityid=x.Cityid,
                cityname=x.Cityname
            }).ToList();
            model.Countries = homeService.CountryStateList("Country");
            model.SelectedCountry = 233;
            var states= homeService.CountryStateList("states");
            model.States = states.Where(x => x.country_id == 233).ToList();
            //model.States = CountryStateList("states").Select(x => new SelectListItem
            //{
            //    Text = x.name,
            //    Value = x.id.ToString()
            //}).ToList();
            //model.SelectedState = model.States.Where(x => x.country_id == 233).Select(x => x.id).FirstOrDefault();
            //    //.Select(x => new int { 
               
                    
            //    //   });//; 1456;
    
            if (id > 0)
            {
                var userlist = homeService.AddOrEdit(id);
                var seletedcityIds = db.tbl_user.Where(x => x.Userid == id).Select(x => x.CityIds).FirstOrDefault();
                userlist.Cityids = seletedcityIds.Split(',').ToArray();
               //new SelectList(city, "Cityid", "Cityname");
                userlist.Countries = model.Countries;
                userlist.States = userlist.SelectedCountry != null ? states.Where(x => x.country_id == userlist.SelectedCountry).ToList() :new List<CountriesDto>();
                userlist.Cities = model.Cities;
                return View(userlist);
            }
           
            return View(model);
        }
        [HttpPost]
        public ActionResult UserProfile(ProfileViewModel model)
        {
            UserEntities db = new UserEntities();
            HomeService homeService = new HomeService();

            if (ModelState.IsValid)
            {
                if (model.Userid > 0)
                {
                    var userlist = db.tbl_user.Where(x => x.Userid == model.Userid).FirstOrDefault();
                    userlist.Userid = model.Userid;
                    userlist.Username = model.Username;
                    userlist.Email = model.Email;
                    userlist.Password = model.Password;
                    userlist.CityIds = string.Join(",", model.Cityids);//this is for multi select dropdown
                    userlist.cityid = model.Cityid;//this is for simple dropdown
                    userlist.Gender = model.Gender;
                    userlist.Startdate =Convert.ToDateTime(model.StartDate);
                    userlist.Enddate = Convert.ToDateTime( model.EndDate);
                    userlist.IsCsharp = model.IsCsharp == null ? false : model.IsCsharp;
                    userlist.IsJava = model.IsJava == null ? false : model.IsJava;
                    userlist.IsPaython = model.IsPaython == null ? false : model.IsPaython;
                    userlist.CountryID = model.SelectedCountry;
                    userlist.StateId = model.SelectedState;
                    if (model.UserImageFile != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(model.UserImageFile.FileName);
                        string extension = Path.GetExtension(model.UserImageFile.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssff") + extension;
                        userlist.ImagePath = "~/Image/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                        model.UserImageFile.SaveAs(filename);

                    }
                    db.Entry(userlist).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    var user = new tbl_user();
                    if (model.UserImageFile != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(model.UserImageFile.FileName);
                        string extension = Path.GetExtension(model.UserImageFile.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssff") + extension;
                        user.ImagePath = "~/Image/" + filename;
                        filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                        model.UserImageFile.SaveAs(filename);

                    }

                    user.Username = model.Username;
                    user.Email = model.Email;
                    user.Password = model.Password;
                    user.cityid = model.Cityid;//this is for simple dropdown
                    user.Gender = model.Gender;
                    user.Startdate = Convert.ToDateTime(model.StartDate);
                    user.Enddate = Convert.ToDateTime(model.EndDate);
                    user.IsCsharp = model.IsCsharp == null ? false : model.IsCsharp;
                    user.IsJava = model.IsJava == null ? false : model.IsJava;
                    user.IsPaython = model.IsPaython == null ? false : model.IsPaython;
                    user.CityIds = model.Cityids == null ? string.Empty : string.Join(",", model.Cityids);
                    user.CountryID = model.SelectedCountry;
                    user.StateId = model.SelectedState;
                    
                    db.tbl_user.Add(user);

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //var city = db.tbl_city.ToList();
            model.Cities = db.tbl_city.Select(x => new citymodel
            {
                cityid = x.Cityid,
                cityname = x.Cityname
            }).ToList();
            model.Countries = homeService.CountryStateList("Country");
            model.SelectedCountry = 233;
            model.States = homeService.CountryStateList("states");
            model.SelectedState = 1456;// new SelectList(city, "Cityid", "Cityname");
            return View(model);
        }

        #region StartSimpleGridIQuarible
        public ActionResult List()
        {
            UserEntities db = new UserEntities();
            HomeService homeservice = new HomeService();
            //try
            //{

            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            string filterItems = HttpContext.Request.Params["SearchItems"];


            //Custom column search fields  
            string UserID = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            string Email = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            string Gender = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            string StartDate = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            string Endate = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            string companies = HttpContext.Request.Params["franchiseIds"];
            //string sortColumn = HttpContext.Request.Params["columns[" + HttpContext.Request.Params["order[0][column]"] + "][data]"];
            //string dateString = @"20/01/0001";
            //var SsDate = StartDate != "" ? DateTime.ParseExact(dateString, @"d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date : DateTime.MinValue;
            //DateTime SDate = StartDate != "" ? Convert.ToDateTime(StartDate,
            //System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat) : DateTime.MinValue;
            //DateTime EDate = (Endate !="") ? Convert.ToDateTime(Endate,
            //System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat) : DateTime.MaxValue;

            DateTime? StartDt = StartDate != "" ? Convert.ToDateTime(StartDate) : (DateTime?)null;
            DateTime? EndDt = (Endate != "") ? Convert.ToDateTime(Endate) : (DateTime?)null;

            IQueryable<tbl_user> users = db.tbl_user.AsQueryable();

            //Getting the total count of Employee to display on the grid pagination.    
            long TotalRecordsCount = users.Count();

            /* Here I have done the filter on And condition (You can change to OR condition if required). Validating the search value is not null or empty and not containing only space. */

            #region filters  

            if (!string.IsNullOrEmpty(companies) && !string.IsNullOrWhiteSpace(companies))
            {
                int[] selectedCompanies = companies.Split(',').Select(x => int.Parse(x)).ToArray();
                users = users.Where(x => selectedCompanies.Contains(x.Userid));
            }

            if (!string.IsNullOrEmpty(UserID) && !string.IsNullOrWhiteSpace(UserID))
            {
                users = users.Where(x => x.Userid != null && x.Userid.ToString().Contains(UserID.ToString()));
            }
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrWhiteSpace(Email))
            {
                users = users.Where(x => x.Email != null && x.Email.ToLower().Contains(Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(Gender) && !string.IsNullOrWhiteSpace(Gender))
            {
                users = users.Where(x => x.Gender != null && x.Gender.ToLower().Contains(Gender.ToLower()));
            }

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(Endate))
            {
                //DateTime StartDt =Convert.ToDateTime(StartDate);
                //DateTime EndDt = Convert.ToDateTime(Endate);
                users = users.Where(x => x.Startdate >= StartDt && x.Enddate <= EndDt);
            }
            if (!string.IsNullOrEmpty(StartDate) && string.IsNullOrEmpty(Endate))
            {
                //DateTime StartDt = Convert.ToDateTime(StartDate);
                users = users.Where(x => x.Startdate >= StartDt);
            }

            if (!string.IsNullOrEmpty(Endate) && string.IsNullOrEmpty(StartDate))
            {
                //DateTime StartDt = Convert.ToDateTime(Endate);
                users = users.Where(x => x.Enddate <= EndDt);
            }

            #endregion


            //count of record after filter   
            long FilteredRecordCount = users.Count();


            /*Here we are allowing only one sorting at time. orderDir will hold asc or desc for sorting the column. */
            #region Sorting  
            switch (order)
            {
                case "1":
                    users = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? users.OrderByDescending(p => p.Userid) : users.OrderBy(p => p.Userid);
                    break;
                case "2":
                    users = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? users.OrderByDescending(p => p.Username) : users.OrderBy(p => p.Username);
                    break;
                case "3":
                    users = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? users.OrderByDescending(p => p.cityid) : users.OrderBy(p => p.cityid);
                    break;
                default:
                    users = users.OrderByDescending(p => p.Userid);
                    break;
            }
            #endregion
            /* Apply pagination to employee iqueryable, startRec will hold the record number from which we need to display and pageSize will hold the number of records to display. Then assign the values to EmployeeDetails model.  */
            var listemployee = users.Skip(startRec).Take(pageSize).ToList()
                .Select(x => new ProfileViewModel()
                {
                    Username = x.Username,
                    Email = x.Email,
                    Gender = x.Gender,
                    IsCsharp = (bool)x.IsCsharp == null ? false : (bool)x.IsCsharp,
                    IsJava = (bool)x.IsJava == null ? false : (bool)x.IsJava,
                    IsPaython = (bool)x.IsPaython == null ? false : (bool)x.IsPaython,
                    Password = x.Password,
                    Userid = x.Userid,
                    StartDate = x.Startdate.ToString(),
                    EndDate = x.Enddate.ToString(),
                    Action = "<a class='btnedit btn btn-sm btn-primary btnEditDelte' onclick='OnEditClick(" + x.Userid + ")' data-id='" + x.Userid + "'>Edit</a>  <a class='btndelete btnEditDelte btn btn-danger btn-sm' onclick='OnDeleteClick(" + x.Userid + ")'  data-id='" + x.Userid + "'>Delete</a>"

                }).ToList();

            return this.Json(new
            {
                draw = Convert.ToInt32(draw),
                recordsTotal = TotalRecordsCount,
                recordsFiltered = FilteredRecordCount,
                data = listemployee

            }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region DynamicGridsToList
        public ActionResult DynamicGrid()
        {
            UserEntities db = new UserEntities();
            HomeService homeservice = new HomeService();
            //try
            //{

            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            string filterItems = HttpContext.Request.Params["SearchItems"];
            string filterSearch = HttpContext.Request.Params["Search"];

            var users = homeservice.GetAllUsers();
            long TotalRecordsCount = users.Count();

            #region filters  
            if (filterSearch == "1")
            {
                users = homeservice.FilterItems(users, filterItems);

            }
            #endregion
            long FilteredRecordCount = users.Count();


            /*Here we are allowing only one sorting at time. orderDir will hold asc or desc for sorting the column. */
            #region Sorting  
            var sortColumnIndex = HttpContext.Request.Params["order[0][column]"];
            var sortOrder = HttpContext.Request.Params["order[0][dir]"];
            var sortColumnName = HttpContext.Request.Params["columns[" + HttpContext.Request.Params["order[0][column]"] + "][name]"];

            if (sortColumnName!=null)
            {
                var Sorted = homeservice.SortGrid(users, sortColumnIndex, sortOrder, sortColumnName);
                users = Sorted;
            }
            #endregion
            /* Apply pagination to employee iqueryable, startRec will hold the record number from which we need to display and pageSize will hold the number of records to display. Then assign the values to EmployeeDetails model.  */
            var listemployee = users.Skip(startRec).Take(pageSize).ToList();
            return this.Json(new
            {
                draw = Convert.ToInt32(draw),
                recordsTotal = TotalRecordsCount,
                recordsFiltered = FilteredRecordCount,
                data = listemployee

            }, JsonRequestBehavior.AllowGet);

        }
        #endregion


        //public List<CountriesDto> CountryStateList(string CountryOrState)
        //{
        //    string filePath = Server.MapPath("/Content/CountryStateList/countries.json");
        //    if (CountryOrState == "states")
        //    {
        //        filePath = Server.MapPath("/Content/CountryStateList/states.json");
        //    }            
        //    string jsonData = System.IO.File.ReadAllText(filePath);
        //    StreamReader file = System.IO.File.OpenText(filePath);
        //    var countrylist = JsonConvert.DeserializeObject<List<CountriesDto>>(jsonData);
        //    return countrylist;
        //}


        public JsonResult GetStateById(int CountryId)
        {
            HomeService homeService = new HomeService();
            var GetState= homeService.CountryStateList("states");
            var States = GetState.Where(x => x.country_id == CountryId).ToList();
            return Json(new {state=States },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteRecord(int Id)
        {
            UserEntities db = new UserEntities();
            var RelatedAnnouncement = db.tbl_user.Where(x => x.Userid == Id).FirstOrDefault();

            if (RelatedAnnouncement != null)
            {
                db.tbl_user.Remove(RelatedAnnouncement);
                db.SaveChanges();

                return Json(new { status = true, msg = "Announcement deleted" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, msg = "Some error occured, please contact administrator!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserDynamicGrid()
        {
            HomeService homeService = new HomeService();
            List<ColumnDto> ColumnsList = homeService.Columns("tblUsers");
            ViewBag.AllColumns = ColumnsList;
            ViewBag.ColumnName = ColumnsList.Select(x => x.ColumnText).ToList();

            return PartialView(ColumnsList);
        }
        public ActionResult DynamicTable()
        {
           

            return View();
        }

        [HttpGet]
        public ActionResult AddRemoveColumn(string TableName, string ModalFor)
        {
            HomeService homeService = new HomeService();
            ColumnGridDetailsDto gridInfo = homeService.GetGridInfo(TableName);
            ViewBag.TableName = TableName;
            ViewBag.ModalFor = ModalFor;
            return PartialView("_ColumnGridSettings", gridInfo);
        }

        public JsonResult RestoreGrid(string TableName)
        {
            HomeService homeService = new HomeService();
            ColumnGridDetailsDto gridInfo = homeService.GetDefaultGridInfo(TableName);
            return Json(gridInfo, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckMinWidth(int ColumnId, string TableName, int ColumnWidth)
        {
            HomeService homeService = new HomeService();

            KeyValuePair<bool, int> columnDetails = homeService.CheckColumnWidth(ColumnId, TableName, ColumnWidth);

            return Json(columnDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckColumnMandatory(int ColumnId, string TableName)
        {
            HomeService homeService = new HomeService();
            bool checkColumnMandatory = homeService.checkColumnIsMandatory(ColumnId, TableName);

            return Json(checkColumnMandatory, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveUpdateGridSettings(ColumnGridDetailsDto GridInfo, string TableName)
        {
            try
            {
                HomeService homeService = new HomeService();
                homeService.SaveUpdateGridInfo(GridInfo, TableName);
                return Json(true);
            }
            catch (Exception ex)
            {               
                return Json(false);

            }
        }

        public ActionResult ContactEnrollmentForm(int?id)
        {
            HomeService homeService = new HomeService();
            ContactForm contactForm = new ContactForm();
            contactForm.CompanyKey = 1;
            contactForm.ContactId = id ?? 0;
            contactForm.ContactFormDetails = homeService.ContactFormDetails(id);
            return View(contactForm);
        }
        [HttpPost]
        public ActionResult ContactEnrollmentForm(ContactForm contact)
        {
            HomeService homeService = new HomeService();
            UserEntities db = new UserEntities();

            List<BacklogFormDataDTO> dynamicFeildsvalue = new JavaScriptSerializer().Deserialize<List<BacklogFormDataDTO>>(contact.DynamicFieldsData);
            var firstname = dynamicFeildsvalue.Where(x => x.ColumnName == ((int)BacklogFormAttributesEnums.ContactColumn.Name).ToString()).FirstOrDefault();
            if (firstname != null)
            {
                var contactid = homeService.AddcontactId(firstname, contact.ContactId);
                foreach (var item in dynamicFeildsvalue)
                {
                    homeService.AddDynamicFieldData(item, contactid);
                }

            }
            
            return RedirectToAction("Index");
        }

        public ActionResult FormSetup()
        {
            UserEntities db = new UserEntities();
            List<cmatrix_contact_form_template_details> fromtemplate = db.cmatrix_contact_form_template_details.Where(x => x.ccftd_cab_type == 47 && x.ccftd_cmp_key==1)
                .OrderBy(x => x.ccftd_order).ToList();
            return View(fromtemplate);
        }

        public ActionResult openEditContactFieldPopup(int id, int type, string Name)
        {
            backlogCommonFormElementsDTO ObjectBacklogFormElementsDTO = new backlogCommonFormElementsDTO();
            ObjectBacklogFormElementsDTO.ifOptionExist = bool.FalseString;
            HomeService homeService = new HomeService();
            ViewBag.FieldColspan = "";
            ViewBag.Dropdowntypes = "";
            ViewBag.Name = Name;


            ObjectBacklogFormElementsDTO = homeService.EditContactFeild(id, type, Name);

            ViewBag.FieldColspan = GetColspans();
            ViewBag.Dropdowntypes = GetDropDownType();
            return PartialView(ObjectBacklogFormElementsDTO);
        }

        private List<FormComponentDto> GetColspans()
        {
            List<FormComponentDto> component = new List<FormComponentDto>();
            component.Add(new FormComponentDto { Id = 1,Description= BacklogFormAttributesEnums.ColspanEnum.one.ToString()});
            component.Add(new FormComponentDto { Id = 2, Description = BacklogFormAttributesEnums.ColspanEnum.one.ToString() });
            return component;
        }

        public List<FormComponentDto> GetDropDownType()
        {
            List<FormComponentDto> DropDownType = new List<FormComponentDto>();
            DropDownType.Add(new FormComponentDto { Id = 1, Description = BacklogFormAttributesEnums.BacklogSetupDropdownClasses.simple});
            DropDownType.Add(new FormComponentDto { Id = 2, Description = BacklogFormAttributesEnums.BacklogSetupDropdownClasses.autoComplete });
            DropDownType.Add(new FormComponentDto { Id = 3, Description = BacklogFormAttributesEnums.BacklogSetupDropdownClasses.multiAuto });
            return DropDownType;
        }

        public JsonResult AddUpdateContactTemplateElement(int elemId, int attrType, string name, string info, string FieldLabel, int colspan, int ddlType, string ddlListType, string OptionsList, bool isReq, string tabType, string FieldPlaceholder, string FieldErrorMessage, string buttonText)
        {
            UserEntities db = new UserEntities();

            try
            {
                cmatrix_contact_form_template_details objcmatrix_backlog_form_template_details = new cmatrix_contact_form_template_details();
                if (elemId > 0)
                {


                    objcmatrix_backlog_form_template_details = db.cmatrix_contact_form_template_details.Where(x => x.ccftd_key == elemId).FirstOrDefault();
                    if (objcmatrix_backlog_form_template_details.ccftd_ddl_type == 3 && ddlType != 3)
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }
                    objcmatrix_backlog_form_template_details = db.cmatrix_contact_form_template_details.Where(x => x.ccftd_key == elemId).FirstOrDefault();
                    objcmatrix_backlog_form_template_details.ccftd_field_name = name;
                    objcmatrix_backlog_form_template_details.ccftd_field_label = FieldLabel;
                    objcmatrix_backlog_form_template_details.ccftd_field_info = info;
                    objcmatrix_backlog_form_template_details.ccftd_col_span = colspan;
                    objcmatrix_backlog_form_template_details.ccftd_options = OptionsList;
                    objcmatrix_backlog_form_template_details.ccftd_ddl_type = ddlType;
                    objcmatrix_backlog_form_template_details.ccftd_field_placeholder = FieldPlaceholder;
                    objcmatrix_backlog_form_template_details.ccftd_is_req = isReq;
                    if (!string.IsNullOrEmpty(FieldErrorMessage) && isReq)
                    {
                        objcmatrix_backlog_form_template_details.ccftd_error_message = FieldErrorMessage;
                    }
                    else
                    {
                        objcmatrix_backlog_form_template_details.ccftd_error_message = "";
                    }
                    db.Entry(objcmatrix_backlog_form_template_details).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    var highestOrder = db.cmatrix_contact_form_template_details.Select(x => x.ccftd_order).DefaultIfEmpty(0).Max();
                    objcmatrix_backlog_form_template_details.ccftd_attr_type = attrType;
                    objcmatrix_backlog_form_template_details.ccftd_field_name = name;
                    objcmatrix_backlog_form_template_details.ccftd_field_label = FieldLabel;
                    objcmatrix_backlog_form_template_details.ccftd_field_info = info;
                    objcmatrix_backlog_form_template_details.ccftd_col_span = colspan;
                    objcmatrix_backlog_form_template_details.ccftd_options = OptionsList;
                    objcmatrix_backlog_form_template_details.ccftd_ddl_type = ddlType;
                    objcmatrix_backlog_form_template_details.ccftd_field_placeholder = FieldPlaceholder;
                    objcmatrix_backlog_form_template_details.ccftd_is_req = isReq;
                    objcmatrix_backlog_form_template_details.ccftd_is_active = true;
                    objcmatrix_backlog_form_template_details.ccftd_is_default = false;
                    objcmatrix_backlog_form_template_details.ccftd_error_message = FieldErrorMessage;
                    objcmatrix_backlog_form_template_details.ccftd_cab_type = 47;
                    objcmatrix_backlog_form_template_details.ccftd_cmp_key = 1;
                    objcmatrix_backlog_form_template_details.ccftd_tab_type = Convert.ToInt32(tabType);
                    objcmatrix_backlog_form_template_details.ccftd_order = highestOrder + 1;

                    db.cmatrix_contact_form_template_details.Add(objcmatrix_backlog_form_template_details);
                    db.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        //entolment form section franchise setup//make menu
        public ActionResult EnrollmentForm()
        {
            HomeService homeService = new HomeService();
            //1 cmpkey,44 cabkey,0 contactid
            UserEntities db = new UserEntities();
            List<StudentSectionDto> templateDetails = new List<StudentSectionDto>();
            templateDetails = db.cmatrix_student_enrollment_section.Where(x => x.cses_cmp_key == 1 && x.cses_cab_type == 44 && x.cses_isdeleted != true && x.cses_is_active != false).AsEnumerable().OrderBy(x => x.cses_order).Select(x => new StudentSectionDto
            {
                SectionId = x.cses_key,
                SectionName = x.cses_name.Replace("{}", "hello"),
                SectionType = x.cses_enum,
                Order = x.cses_order ?? 0,
                //Data = _dataInstance.StudentFormDataActions.GetDataDescription(x.cses_key),
                StudentFormDetails = homeService.GetStudentEnrollmentForm(x.cses_key, 44, 0) 
            }).ToList();
            return View(templateDetails);

        }
    }

}


   
    
