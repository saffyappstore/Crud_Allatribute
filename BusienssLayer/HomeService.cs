using DataLayer;
using Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using ViewModelLayer;

namespace BusienssLayer
{
    public class HomeService
    {
        public string optionListState()
        {
            var ddlstate = CountryStateList("state");
            var ddlOptionState = new JavaScriptSerializer().Serialize(ddlstate);
            return ddlOptionState;
        }
        public string optionListCountry()
        {
            var ddlcountry = CountryStateList("country");
            var ddlOption = new JavaScriptSerializer().Serialize(ddlcountry);
            return ddlOption;
        }
        //public List<CountriesDto> GetStatesAndCountries(string type)
        //{

        //    string filePath = "";
        //    if (type == "country")
        //    {
        //        filePath = HttpContext.Current.Server.MapPath("/Content/CountriesCitiesJson/countries.json");
        //    }
        //    else
        //    {
        //        filePath = HttpContext.Current.Server.MapPath("/Content/CountriesCitiesJson/states.json");
        //    }
        //    //deserialize JSON from file  
        //    string JsonData = System.IO.File.ReadAllText(filePath);
        //    StreamReader file = System.IO.File.OpenText(filePath);
        //    var countriesList = JsonConvert.DeserializeObject<List<CountriesDto>>(JsonData);
        //    return countriesList;
        //}

        public List<CountriesDto> CountryStateList(string CountryOrState)
        {
           
            //string filePath = Server.MapPath("/Content/CountryStateList/countries.json");
            //string filePath = ExecutionDirectoryPathName();
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Content\CountryStateList\countries.json";
            if (CountryOrState == "states")
            {

                filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Content\CountryStateList\states.json";
                //Server.MapPath("/Content/CountryStateList/states.json");
            }
            string jsonData = System.IO.File.ReadAllText(filePath);
            StreamReader file = System.IO.File.OpenText(filePath);
            var countrylist = JsonConvert.DeserializeObject<List<CountriesDto>>(jsonData);
            //return countrylist;
            var CountryStateData = countrylist.Select(x => new CountriesDto
            {
                name = x.name,
                id = x.id,
                country_id = x.country_id
            }).ToList();
            return CountryStateData;
        }
        //get
        public ProfileViewModel AddOrEdit(int? UserId)
        {
            UserEntities db = new UserEntities();
           var user= db.tbl_user.Where(x => x.Userid == UserId).Select(x => new ProfileViewModel
            {
                Username = x.Username,
                Email = x.Email,
                Gender = x.Gender,
                IsCsharp = (bool)x.IsCsharp == null ? false : (bool)x.IsCsharp,
                IsJava = (bool)x.IsJava == null ? false : (bool)x.IsJava,
                IsPaython = (bool)x.IsPaython == null ? false : (bool)x.IsPaython,
                Password = x.Password,
                Userid = x.Userid,
                Cityid = x.cityid,
                StartDate = x.Startdate.ToString(),
                EndDate = x.Enddate.ToString(),
                ImagePath = x.ImagePath != null ? x.ImagePath : "~/no-image-available-icon.jpg",
                SelectedCountry = (int)x.CountryID,
                SelectedState = (int)x.StateId,
                //Cityids=x.CityIds,             


            }).FirstOrDefault();

            return user;
        }
        public List<ProfileViewModel> GetAllUsers()
        {
            UserEntities db = new UserEntities();

            var users =db.tbl_user.Select(x => new ProfileViewModel()
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
            return users;
        }
        public List<ColumnDto> Columns(string tablename)
        {
            UserEntities db = new UserEntities();
            List<ColumnDto> ColumnsList = new List<ColumnDto>();
           var usercolumn= db.ColumnUsersSettings.Where(x => x.GridName == tablename).ToList();
            if(usercolumn.Count>0 && usercolumn != null)
            {
                ColumnsList = db.ColumnUsersSettings.Where(x => x.GridName == tablename && x.IsVisible==true).AsEnumerable()
          .Select(x => new ColumnDto
          {
              ColumnId = x.Id,
              ColumnText = x.ColumnText,
              GridName = x.GridName,
              ColumnFilterId = x.ColumnFilterId,
              ColumnType = x.ColumnType,
              ColumnWidth = x.ColumnWidth,
              OrderBy=x.ColumnOrder
              

          }).OrderBy(x=>x.OrderBy).ToList();
            }
            else
            {
               ColumnsList = db.ColumnSettings.Where(x => x.GridName == tablename && x.IsVisible == true).AsEnumerable()
       .Select(x => new ColumnDto
       {
           ColumnId = x.Id,
           ColumnText = x.ColumnText,
           GridName = x.GridName,
           ColumnFilterId = x.ColumnFilterId,
           ColumnType = x.ColumnType,
           ColumnWidth = x.ColumnWidth,
            OrderBy = x.ColumnOrder

       }).OrderBy(x=>x.OrderBy).ToList();
            }
         

            return ColumnsList;
        }


        public List<ProfileViewModel> FilterItems(List<ProfileViewModel> records, string filteritems)
        {

            if (filteritems != null)
            {
                
                var items = filteritems.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                string[] words = items[0].Split(',');
                var filterTypes = words[0].Replace("FilterType=", string.Empty);
                words[0] = filterTypes;
                Dictionary<string, string> filter = new Dictionary<string, string>();

                foreach (var item in items)
                {
                    if (item.Substring(item.IndexOf("=") + 1).Length > 0)
                    {
                        filter.Add(item.Substring(0, item.IndexOf("=")), item.Substring(item.IndexOf("=") + 1));

                    }
                    else
                    {
                        filter.Add(item.Substring(0, item.IndexOf("=")), "undefined");
                    }
                }

   
                if(filter.ContainsKey("Username") && (!(filter["Username"].Contains("undefined"))))
                {
                    records = records.Where(x => x.Username != null && x.Username.ToLower().Contains(filter["Username"].ToLower())).ToList();
                }
                if (filter.ContainsKey("Email") && (!(filter["Email"].Contains("undefined"))))
                {
                    records = records.Where(x => x != null && x.Email.ToLower().Contains(filter["Email"].ToLower())).ToList();
                }
                if (filter.ContainsKey("StartDate") && (!(filter["StartDate"].Contains("undefined"))) && (filter["EndDate"].Contains("undefined")))
                {
                    DateTime startDate = Convert.ToDateTime((filter["StartDate"]));
                    string strstartToDates = startDate.ToString("yyyy-MM-dd");

                    records = records.Where(x => x.StartDate != null && x.StartDate.Contains(strstartToDates)).ToList();
                }
                else if (filter.ContainsKey("EndDate") && (!(filter["EndDate"].Contains("undefined"))) && (filter["StartDate"].Contains("undefined")))
                {
                    DateTime endDate = Convert.ToDateTime((filter["EndDate"]));
                    string endToDates = endDate.ToString("MMM dd, yyyy");

                    records = records.Where(x => x.EndDate != null && x.EndDate.ToString().Contains(endToDates)).ToList();
                }
                else if (filter.ContainsKey("StartDate") && (!(filter["StartDate"].Contains("undefined"))) && (filter.ContainsKey("EndDate") && (!(filter["EndDate"].Contains("undefined")))))
                {
                    DateTime startDate = Convert.ToDateTime((filter["StartDate"]));
                    DateTime endDate = Convert.ToDateTime((filter["EndDate"]));
                    if (startDate.Equals(endDate))
                    {
                        records = records.Where(x => x.StartDate != null && x.StartDate.Equals(startDate)).ToList();
                    }
                   else if(endDate> startDate)
                    {
                        records = records.Where(x => x.StartDate != null && Convert.ToDateTime(x.StartDate)>= startDate && Convert.ToDateTime(x.EndDate)<=endDate).ToList();

                    }
                }
            }

            return records;
        }
        public List<ProfileViewModel> SortGrid(IEnumerable<ProfileViewModel> records, string sortColumnIndex,string sortOrder,string sortColumnName)
        {
            var OrderList = new List<ProfileViewModel>();
            switch (sortColumnName.Trim())
            {
                case "Username":
                    OrderList = sortOrder == "asc" ? records.OrderBy(x => x.Username).ToList() : records.OrderByDescending(x => x.Username).ToList();
                    break;
                case "Email":
                   OrderList = sortOrder == "asc" ? records.OrderBy(x => x.Email).ToList() : records.OrderByDescending(x => x.Email).ToList();
                    break;
                default:
                   OrderList = records.OrderByDescending(x => x.Username).ToList();
                    break;
            }


            return OrderList;
        }

        public ColumnGridDetailsDto GetGridInfo(string TableName)
        {
            ColumnGridDetailsDto gridDetailsDto = new ColumnGridDetailsDto();
            UserEntities db = new UserEntities();
            var checkUserTableExists = db.ColumnUsersSettings.Where(x => x.GridName == TableName).ToList();
            if (checkUserTableExists.Count > 0)
            {
                gridDetailsDto.visibleColumns = db.ColumnUsersSettings.Where(x => x.GridName == TableName && x.IsVisible == true).AsEnumerable()
         .Select(x => new ColumnDto
         {
             ColumnId = x.Id,
             ColumnText = x.ColumnText,
             GridName = x.GridName,
             ColumnFilterId = x.ColumnFilterId,
             ColumnType = x.ColumnType,
             ColumnWidth = x.ColumnWidth,
             OrderBy = x.ColumnOrder

         }).OrderBy(x => x.OrderBy).ToList();

                gridDetailsDto.hiddenColumns = db.ColumnUsersSettings.Where(x => x.GridName == TableName && x.IsVisible == false).AsEnumerable()
            .Select(x => new ColumnDto
            {
                ColumnId = x.Id,
                ColumnText = x.ColumnText,
                GridName = x.GridName,
                ColumnFilterId = x.ColumnFilterId,
                ColumnType = x.ColumnType,
                ColumnWidth = x.ColumnWidth,
                OrderBy = x.ColumnOrder
            }).OrderBy(x => x.OrderBy).ToList();
            }
            else
            {
                gridDetailsDto.visibleColumns = db.ColumnUsersSettings.Where(x => x.GridName == TableName && x.IsVisible == true).AsEnumerable()
     .Select(x => new ColumnDto
     {
         ColumnId = x.Id,
         ColumnText = x.ColumnText,
         GridName = x.GridName,
         ColumnFilterId = x.ColumnFilterId,
         ColumnType = x.ColumnType,
         ColumnWidth = x.ColumnWidth,
         OrderBy = x.ColumnOrder

     }).OrderBy(x => x.OrderBy).ToList();

                gridDetailsDto.hiddenColumns = db.ColumnUsersSettings.Where(x => x.GridName == TableName && x.IsVisible == false).AsEnumerable()
            .Select(x => new ColumnDto
            {
                ColumnId = x.Id,
                ColumnText = x.ColumnText,
                GridName = x.GridName,
                ColumnFilterId = x.ColumnFilterId,
                ColumnType = x.ColumnType,
                ColumnWidth = x.ColumnWidth,
                OrderBy = x.ColumnOrder
            }).OrderBy(x => x.OrderBy).ToList();

            }
            return gridDetailsDto;
        }
        public ColumnGridDetailsDto GetDefaultGridInfo(string TableName)
        {
            UserEntities db = new UserEntities();
            ColumnGridDetailsDto gridColumnsInfo = new ColumnGridDetailsDto();
            gridColumnsInfo.visibleColumns = db.ColumnSettings.Where(x => x.GridName == TableName && x.IsVisible==true).Select(x => new ColumnDto
            {
                ColumnId = x.Id,
                ColumnText = x.ColumnText,
                GridName = x.GridName,
                ColumnFilterId = x.ColumnFilterId,
                ColumnType = x.ColumnType,
                ColumnWidth = x.ColumnWidth,
                OrderBy = x.ColumnOrder
            }).OrderBy(x => x.OrderBy).ToList();

            gridColumnsInfo.hiddenColumns = db.ColumnSettings.Where(x => x.GridName == TableName && x.IsVisible == false).Select(x => new ColumnDto
            {
                ColumnId = x.Id,
                ColumnText = x.ColumnText,
                GridName = x.GridName,
                ColumnFilterId = x.ColumnFilterId,
                ColumnType = x.ColumnType,
                ColumnWidth = x.ColumnWidth,
                OrderBy = x.ColumnOrder
            }).OrderBy(x => x.OrderBy).ToList();
            return gridColumnsInfo;
        }

        public KeyValuePair<bool, int> CheckColumnWidth(int ColumnId, string TableName, int ColumnWidth)
        {
            UserEntities db = new UserEntities();

            string defaultcolumnMinWidth = db.ColumnSettings.Where(x=>x.Id==ColumnId).Select(x=>x.ColumnWidth).FirstOrDefault();
            int defaultvalue ;
            int.TryParse(defaultcolumnMinWidth,out defaultvalue);
           
            if (defaultcolumnMinWidth == null)
            {
                defaultvalue = 0;
            }

            if (ColumnWidth < defaultvalue)
            {
                return new KeyValuePair<bool, int>(false, defaultvalue);
            }
            else
            {
                return new KeyValuePair<bool, int>(true, defaultvalue);
            }
        }

        public bool checkColumnIsMandatory(int ColumnId, string TableName)
        {
            UserEntities db = new UserEntities();

            bool? isMandatory = db.ColumnSettings.Where(x => x.Id == ColumnId).Select(x => x.IsMandatory).First();

            return (bool)isMandatory;
        }

        public void SaveUpdateGridInfo(ColumnGridDetailsDto GridDetails, string TableName)
        {
            UserEntities db = new UserEntities();
            var checkUserTableExists = db.ColumnUsersSettings.Where(x => x.GridName == TableName).ToList();

            try
            {


                // Update Entries if record found
                if (checkUserTableExists.Count() > 0 && checkUserTableExists != null)
                {
                    int ColumnOrder = 1;

                    // update visible columns properties
                    foreach (var item in GridDetails.visibleColumns)
                    {
                        var gridColumn = db.ColumnUsersSettings.Where(x => x.GridName == TableName && x.ColumnText == item.ColumnText).FirstOrDefault();

                        if (gridColumn == null)
                        {
                            SaveColumnData(ColumnOrder, item, TableName);
                        }
                        else
                        {
                            gridColumn.IsVisible = true;
                            gridColumn.ColumnWidth = item.ColumnWidth;
                            gridColumn.ColumnOrder = item.OrderBy;
                            gridColumn.ColumnText = item.ColumnText;
                            db.Entry(gridColumn).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        ColumnOrder = ColumnOrder + 1;
                    }

                    //update hidden columns properties
                    foreach (var item in GridDetails.hiddenColumns)
                    {
                        var gridColumn = db.ColumnUsersSettings.Where(x => x.GridName == TableName && x.ColumnText == item.ColumnText).FirstOrDefault();

                        if (gridColumn == null)
                        {
                            SaveColumnData(0, item, TableName);
                        }
                        else
                        {
                            gridColumn.IsVisible = false;
                            gridColumn.ColumnWidth = item.ColumnWidth;
                            gridColumn.ColumnOrder = item.OrderBy;
                            gridColumn.ColumnText = item.ColumnText;
                            db.Entry(gridColumn).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                // Save Entries if record not found
                else
                {

                    int ColumnOrder = 1;

                    // save visible columns properties
                    foreach (var item in GridDetails.visibleColumns)
                    {
                        // get default column values against table name and matching column name
                        var defaultColumnValues = db.ColumnUsersSettings.Where(x => x.ColumnText == item.ColumnText && x.GridName == TableName).FirstOrDefault();
                        ColumnUsersSetting gridColumn = new ColumnUsersSetting();
                        gridColumn.IsVisible = true;
                        gridColumn.ColumnWidth = item.ColumnWidth;
                        gridColumn.ColumnOrder = ColumnOrder;
                        gridColumn.ColumnText = item.ColumnText;
                        gridColumn.GridName = TableName;
                        gridColumn.ColumnType = defaultColumnValues.ColumnType;
                        gridColumn.ColumnFilterId = defaultColumnValues.ColumnFilterId;
                        db.ColumnUsersSettings.Add(gridColumn);
                        db.SaveChanges();

                        ColumnOrder = ColumnOrder + 1;
                    }


                    // save hidden columns properties
                    foreach (var item in GridDetails.hiddenColumns)
                    {
                        // get default column values against table name and matching column name
                        var defaultColumnValues = db.ColumnUsersSettings.Where(x => x.ColumnText == item.ColumnText && x.GridName == TableName).FirstOrDefault();
                        ColumnUsersSetting gridColumn = new ColumnUsersSetting();
                        gridColumn.IsVisible = false;
                        gridColumn.ColumnWidth = item.ColumnWidth;
                        gridColumn.ColumnOrder = 0;
                        gridColumn.ColumnText = item.ColumnText;
                        gridColumn.GridName = TableName;
                        gridColumn.ColumnType = defaultColumnValues.ColumnType;
                        gridColumn.ColumnFilterId = defaultColumnValues.ColumnFilterId;
                        db.ColumnUsersSettings.Add(gridColumn);
                        db.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void SaveColumnData(int ColumnOrder, ColumnDto ColumnsData, string TableName)
        {
             UserEntities db = new UserEntities();
            var defaultColumnValues = db.ColumnUsersSettings.Where(x => x.ColumnText == ColumnsData.ColumnText && x.GridName == TableName).FirstOrDefault();
            ColumnUsersSetting gridColumn = new ColumnUsersSetting();
            gridColumn.IsVisible = true;
            gridColumn.ColumnWidth = ColumnsData.ColumnWidth;
            gridColumn.ColumnOrder = ColumnOrder;
            gridColumn.ColumnText = ColumnsData.ColumnText;
            gridColumn.GridName = TableName;
            gridColumn.ColumnType = defaultColumnValues.ColumnType;
            gridColumn.ColumnFilterId = defaultColumnValues.ColumnFilterId;
            db.ColumnUsersSettings.Add(gridColumn);
            db.SaveChanges();
             
        }

        public int AddcontactId(BacklogFormDataDTO item,int id)
        {
            UserEntities db = new UserEntities();
            int contactid = id;
            if (id == 0)
            {
                cmatrix_contact contact = new cmatrix_contact();
                contact.ccf_name = item.FieldValue;
                contact.ccf_created_date = DateTime.Now;
                db.cmatrix_contact.Add(contact); ;
                db.SaveChanges();
                contactid = contact.ccf_key;

            }
            else
            {
                var ids = db.cmatrix_contact.Where(x => x.ccf_key == id).FirstOrDefault();
                ids.ccf_name = item.FieldValue;
                db.Entry(ids).State = EntityState.Modified;
                db.SaveChanges();
            }

            return contactid;
        }

        public void AddDynamicFieldData(BacklogFormDataDTO item,int id)
        {
           
            UserEntities db = new UserEntities();
            var isAdded = db.cmatrix_contact_form_data.Where(x => x.ccfd_ccb_key == id && x.ccfd_ccftd_key==item.elementId).FirstOrDefault();
            if (isAdded!= null)
            {
                isAdded.ccfd_data = item.FieldValue;
                db.Entry(isAdded).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                cmatrix_contact_form_data data = new cmatrix_contact_form_data();
                data.ccfd_data = item.FieldValue;
                data.ccfd_ccftd_key = item.elementId;
                data.ccfd_ccb_key = id;
                db.cmatrix_contact_form_data.Add(data);
                db.SaveChanges();

            }

        }

        public backlogCommonFormElementsDTO EditContactFeild(int id, int type,string name)
        {
            backlogCommonFormElementsDTO ObjectBacklogFormElementsDTO = new backlogCommonFormElementsDTO();
            UserEntities db = new UserEntities();
            if (id > 0)
            {
                cmatrix_contact_form_template_details element = new cmatrix_contact_form_template_details();
                element = db.cmatrix_contact_form_template_details.Where(x => x.ccftd_key == id).FirstOrDefault();
                ObjectBacklogFormElementsDTO.elementId = element.ccftd_key;
                ObjectBacklogFormElementsDTO.BacklogType = element.ccftd_cab_type;
                ObjectBacklogFormElementsDTO.FieldName = element.ccftd_field_name;
                ObjectBacklogFormElementsDTO.ColName = element.ccftd_cab_col_name;
                ObjectBacklogFormElementsDTO.FieldLabel = element.ccftd_field_label;
                ObjectBacklogFormElementsDTO.attrType = element.ccftd_attr_type;
                ObjectBacklogFormElementsDTO.FieldInfo = element.ccftd_field_info;
                ObjectBacklogFormElementsDTO.Colspan = element.ccftd_col_span;
                ObjectBacklogFormElementsDTO.FieldErrorMessage = element.ccftd_error_message;
                if (element.ccftd_is_default == true && string.IsNullOrEmpty(element.ccftd_error_message) && element.ccftd_is_req)
                {
                    ObjectBacklogFormElementsDTO.FieldErrorMessage = element.ccftd_field_name + " is required!";
                }
                ObjectBacklogFormElementsDTO.FieldPlaceholder = string.IsNullOrEmpty(element.ccftd_field_placeholder) ? element.ccftd_field_name : element.ccftd_field_placeholder;
                if (!string.IsNullOrEmpty(element.ccftd_options))
                {
                    ObjectBacklogFormElementsDTO.OptionsList = new JavaScriptSerializer().Deserialize<List<DropdownListOptions>>(element.ccftd_options);
                }
                ObjectBacklogFormElementsDTO.DropdownType = element.ccftd_ddl_type == null ? 0 : (int)element.ccftd_ddl_type;
                //ObjectIncidentFormElementsDTO.ListType = element.cbftd_ddl_list_type;
                ObjectBacklogFormElementsDTO.IsRequired = element.ccftd_is_req;
                ObjectBacklogFormElementsDTO.IsDefault = element.ccftd_is_default == null ? true : (bool)element.ccftd_is_default;
                ObjectBacklogFormElementsDTO.tabType = element.ccftd_tab_type == null ? 0 : (int)element.ccftd_tab_type;

                //if (element.ccftd_attr_type == (int)BacklogFormAttributesEnums.AttributeTypes.CustomDropdownUserList || element.ccftd_attr_type == (int)BacklogFormAttributesEnums.AttributeTypes.DropdownCustomList || element.ccftd_attr_type == (int)BacklogFormAttributesEnums.AttributeTypes.Radiobutton)
                //{
                //    string OptionData = _dataAccess.ContactFormDataAction.GetAll().Where(x => x.ccfd_ccftd_key == element.ccftd_key && x.ccfd_data != null).Select(x => x.ccfd_data).FirstOrDefault();
                //    if (OptionData != null)
                //    {
                //        ObjectBacklogFormElementsDTO.ifOptionExist = bool.TrueString;
                //    }
                //}
                //if (element.ccftd_attr_type == (int)BacklogFormAttributesEnums.AttributeTypes.ButtonText)
                //{
                //    var ButtonText = _dataAccess.ContactFormTemplateAction.GetAll().Where(x.ccftd_attr_type == (int)BacklogFormAttributesEnums.AttributeTypes.ButtonText).Select(x => x.cmatrix_contact_form_data.FirstOrDefault().ccfd_data ?? "Submit").FirstOrDefault();
                //    ObjectBacklogFormElementsDTO.buttonText = ButtonText;
                //}
            }
            else
            {
                ObjectBacklogFormElementsDTO.attrType = type;
                ObjectBacklogFormElementsDTO.IsDefault = false;
                //ObjectIncidentFormElementsDTO.FieldName = type;
            }
            return ObjectBacklogFormElementsDTO;
        }

        public List<ContactFormDetail> ContactFormDetails(int ?Contactid)
        {
            UserEntities db = new UserEntities();
            var detail= db.cmatrix_contact_form_template_details.Where(x => x.ccftd_cmp_key == 1).OrderBy(x=>x.ccftd_order).ToList();
            List<ContactFormDetail> ContactFormDetails = new List<ContactFormDetail>();
            foreach (var item in detail)
            {
                var contactForm = new ContactFormDetail();
                contactForm.FormDetailId = item.ccftd_key;
                contactForm.TabType = item.ccftd_tab_type;
                contactForm.ddlType = item.ccftd_ddl_type;
                contactForm.IsActive = item.ccftd_is_active;
                contactForm.IsDefault = item.ccftd_is_default==null ? false :(bool)item.ccftd_is_default;
                contactForm.IsRequired = item.ccftd_is_req;

                if (!string.IsNullOrEmpty(item.ccftd_error_message))
                {
                    contactForm.FieldErrorMessage = item.ccftd_error_message;
                }
                else
                {
                    contactForm.FieldErrorMessage = $"{item.ccftd_field_name} is required!!";

                }
                contactForm.Option = item.ccftd_options;
                contactForm.FieldInfo = item.ccftd_field_info;
                contactForm.FieldName = item.ccftd_field_name;
                if (Contactid != null)
                {
                    contactForm.FieldValue = item.cmatrix_contact_form_data.FirstOrDefault(x => x.ccfd_ccb_key == Contactid)?.ccfd_data;
                }
                contactForm.order = item.ccftd_order;
                contactForm.attributeType = item.ccftd_attr_type;
                contactForm.ColType = item.ccftd_cab_col_name == null ? 0 : (int)item.ccftd_cab_col_name;
                contactForm.FieldLabel = item.ccftd_field_label;
                contactForm.Colspan = item.ccftd_col_span;
                contactForm.ColumnName = item.ccftd_cab_col_name;
                contactForm.FieldPlaceholder = item.ccftd_field_placeholder;
                if (contactForm.ddlType == 1)
                {
                    contactForm.FieldClass =BacklogFormAttributesEnums.BacklogSetupDropdownClasses.simple;
                }
                if ((int)BacklogFormAttributesEnums.AttributeType.DropdownCustomList==contactForm.attributeType || (int)BacklogFormAttributesEnums.AttributeType.Radiobutton==contactForm.attributeType)
                {
                    contactForm.OptionsList = OptionListData(item.ccftd_options);
                }
                ContactFormDetails.Add(contactForm);
            }
            return ContactFormDetails;
        }

        public List<StudentFormDetails> GetStudentEnrollmentForm(int sectionKey, int cabType, int? UserId = null, int? enums = null)
        {
            UserEntities db = new UserEntities();
            List<StudentFormDetails> studentSections = new List<StudentFormDetails>();
           var dataFileds = db.cmatrix_student_form_template_details.Where(y => y.csftd_cses_key == sectionKey && y.csftd_is_active != false && y.csftd_cab_type == cabType).OrderBy(x => x.csftd_order).ToList();

            List<StudentFormDetails> StudentFormDetails = new List<StudentFormDetails>();

            foreach (var item in dataFileds)
            {
                var StudentForm = new StudentFormDetails();
                StudentForm.SectionType = enums.HasValue ? enums.Value : 0;//enum is section like contact detail
                StudentForm.FormDetailId = item.csftd_key;
                StudentForm.TabType = item.csftd_tab_type == null ? default(int) : (int)item.csftd_tab_type;
                StudentForm.ddlType = item.csftd_ddl_type == null ? default(int) : (int)item.csftd_ddl_type;

                StudentForm.IsActive = item.csftd_is_active;
                StudentForm.IsDefault = item.csftd_is_default == null ? false : (bool)item.csftd_is_default;
                StudentForm.IsRequired = item.csftd_is_req;
                StudentForm.ColumnName = item.csftd_cab_col_name;

                StudentForm.Option = item.csftd_options;
                //if (item.csftd_attr_type == (int)BacklogFormAttributesEnums.AttributeTypes.Label)
                //{
                //    StudentForm.FieldInfo = HttpUtility.UrlDecode(item.csftd_field_info.Replace("{}", CompanyName));
                //}
                //else
                //{
                //    StudentForm.FieldInfo = item.csftd_field_info.Replace("{}", CompanyName);
                //}
                StudentForm.FieldName = item.csftd_field_name;
                StudentForm.FormFieldName = item.csftd_field_name;
                //if (franchiseId != null)
                //{
                //    StudentForm.FieldValue = item.cmatrix_student_form_data.FirstOrDefault(x => x.csfd_cmp_key == franchiseId)?.csfd_data;
                //}

                StudentForm.order = item.csftd_order;
                StudentForm.attributeType = item.csftd_attr_type;
                StudentForm.ColType = item.csftd_cab_col_name == null ? 0 : (int)item.csftd_cab_col_name;
                StudentForm.FieldLabel = item.csftd_field_label;
                StudentForm.Colspan = item.csftd_col_span;

                StudentForm.FieldPlaceholder = item.csftd_field_placeholder;
                StudentForm.FieldErrorMessage = item.csftd_validation_msg;//$"{item.csftd_field_label} is required!";
                if (StudentForm.ddlType == 1)
                {
                    StudentForm.FieldClass = BacklogFormAttributesEnums.BacklogSetupDropdownClasses.simple;
                }
                else if (StudentForm.ddlType == 2)
                {
                    StudentForm.FieldClass = BacklogFormAttributesEnums.BacklogSetupDropdownClasses.autoComplete;
                }
                else if (StudentForm.ddlType == 3)
                {
                    StudentForm.FieldClass = BacklogFormAttributesEnums.BacklogSetupDropdownClasses.multiAuto;
                }

                if ((int)BacklogFormAttributesEnums.AttributeType.DropdownCustomList == StudentForm.attributeType)
                {
                    StudentForm.OptionsList = GetRadioButtonOption(StudentForm.Option);
                }
                StudentFormDetails.Add(StudentForm);
            }
            return StudentFormDetails;
        }

        public List<DropdownListOptions> GetRadioButtonOption(string elem)
        {
            List<DropdownListOptions> OptList = new List<DropdownListOptions>();
            if (!string.IsNullOrEmpty(elem))
            {
                List<DropdownListOptions> objcustomddl = new JavaScriptSerializer().Deserialize<List<DropdownListOptions>>(elem);
                foreach (var item in objcustomddl)
                {
                    OptList.Add(new DropdownListOptions
                    {
                        OptionId = item.OptionId,
                        Option = item.Option,
                        IsSelected = item.IsSelected
                    });
                }
            }
            return OptList;
        }

        private List<DropdownListOptions> OptionListData(string elem)
        {
            List<DropdownListOptions> ListOptions = new List<DropdownListOptions>();
            if (!string.IsNullOrEmpty(elem))
            {
                List<DropdownListOptions> Options = new JavaScriptSerializer().Deserialize<List<DropdownListOptions>>(elem);
                foreach (var item in Options)
                {
                    ListOptions.Add(new DropdownListOptions
                    {
                        Option=item.Option,
                        OptionId=item.OptionId
                    });
                }
            }
            return ListOptions;
        }
    }
}
