using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModelLayer
{
    public class ContactForm
    {
        public int ContactId { get; set; }
        public int CompanyKey { get; set; }
        public string DynamicFieldsData { get; set; }
        public List<ContactFormDetail> ContactFormDetails { get; set; }
        public Guid CompanyGuid { get; set; }
        public string ButtonText { get; set; }
        public string CompanyLogo { get; set; }
    }
    public class ContactFormDetail
    {
        public int SectionType { get; set; }
        public int FormDetailId { get; set; }
        public string FormDetailName { get; set; }
        public int? ColumnName { get; set; }
        public int attributeType { get; set; }
        public bool? isDefault { get; set; }
        public bool isActive { get; set; }
        public int? TabType { get; set; }
        public int? ColType { get; set; }
        public string FieldLabel { get; set; }
        public int order { get; set; }

        [AllowHtml]
        public string FieldValue { get; set; }
        public string FieldName { get; set; }
        public string FieldPlaceholder { get; set; }
        public string FieldInfo { get; set; }
        public int Colspan { get; set; }
        public List<DropdownListOptions> OptionsList { get; set; }
        public string Option { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string FieldClass { get; set; }
        public int? ddlType { get; set; }
        public string FieldErrorMessage { get; set; }
        public int FormDataId { get; set; }

    }

    public class DropdownListOptions
    {
        public int OptionId { get; set; }
        public string Option { get; set; }
        public bool? IsSelected { get; set; }
    }
    public class BacklogFormDataDTO
    {
        public int elementId { get; set; }

        [AllowHtml]
        public string FieldValue { get; set; }
        public string attrType { get; set; }
        public string ColumnName { get; set; }
        public int sectionType { get; set; }
        public int Sync { get; set; }
        public string sectionStudentList { get; set; }
        public Guid? RecordGuid { get; set; }
        public string AssignedStudentGuids { get; set; }
        public bool? IsEmptyParentSectionNewlyCreated { get; set; }
        public int? ParentSectionIndex { get; set; }
    }
    public class backlogCommonFormElementsDTO
    {
        public int SectionId { get; set; }
        public int elementId { get; set; }
        public int? BacklogType { get; set; }
        public int attrType { get; set; }
        public int order { get; set; }

        [AllowHtml]
        public string FieldName { get; set; }


           [AllowHtml]
        public string buttonText { get; set; }

        [AllowHtml]
        public string FieldLabel { get; set; }

        public int? ColName { get; set; }
        public string FieldInfo { get; set; }
        public string FieldPlaceholder { get; set; }
        public int Colspan { get; set; }

        public int DropdownType { get; set; }

        public string ListType { get; set; }

        public List<DropdownListOptions> OptionsList { get; set; }
        public string OptionsJSON { get; set; }

        public string Option { get; set; }

        public bool IsRequired { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string FieldValue { get; set; }
        public int FormDataId { get; set; }
        public string FieldClass { get; set; }
        public string FieldErrorMessage { get; set; }
        public int tabType { get; set; }
        public string ifOptionExist { get; set; }
        public int? Sync { get; set; }

        public backlogCommonFormElementsDTO()
        {
            OptionsList = new List<DropdownListOptions>();
        }

    }
    public class FormComponentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string type { get; set; }
        public string ddl_type { get; set; }
        public bool isDefault { get; set; }
        public int order { get; set; }
        public string color { get; set; }
    }

    public class StudentSectionDto
    {
        public int SectionCounter { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public string Data { get; set; }
        public int? SectionType { get; set; }
        public bool? isActive { get; set; }
        public int Order { get; set; }
        public List<StudentFormDetails> StudentFormDetails { get; set; }
        public bool IsDeleteableSection { get; set; }
        public bool IsShowEnableIcon { get; set; }
        public bool IsEditableSection { get; set; }
        public bool IsAllowToAddFields { get; set; }
        public string AssignedStudentId { get; set; }
        public string AssignedStudentGuids { get; set; }
        public int? StudentId { get; set; }
        public int? ParentId { get; set; }
        public bool IsParentActive { get; set; }
        public bool? IsContactCreated { get; set; }
        public bool IsUserLead { get; set; }
        public StudentSectionDto()
        {
            StudentFormDetails = new List<StudentFormDetails>();
        }


    }
    public class StudentFormDetails
    {
        public int? StudentId { get; set; }
        public int SectionType { get; set; }
        public int FormDetailId { get; set; }
        public bool IsStudentPortalAccessBtn { get; set; }
        public bool IsParentPortalAccessBtn { get; set; }
        public string FormDetailName { get; set; }
        public int? ColumnName { get; set; }
        public int attributeType { get; set; }
        public bool? isDefault { get; set; }
        public bool isActive { get; set; }
        public int? TabType { get; set; }
        public int? ColType { get; set; }
        public string FieldLabel { get; set; }
        public int order { get; set; }
        public string FieldValue { get; set; }
        public string FieldName { get; set; }
        public string FieldPlaceholder { get; set; }
        public string FieldInfo { get; set; }
        public int Colspan { get; set; }
        public List<DropdownListOptions> OptionsList { get; set; }
        public string Option { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string FieldClass { get; set; }
        public int? ddlType { get; set; }
        public string FieldErrorMessage { get; set; }
        public int FormDataId { get; set; }
        public string UserImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageColor { get; set; }
        public Guid? UserGuid { get; set; }
        public string FormFieldName { get; set; }
        public bool IsLead { get; set; }
    }
}
