using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums
{
   public class BacklogFormAttributesEnums
    {
        public enum AttributeType
        {
            [Description("Dropdown-CustomList")]
            DropdownCustomList=7,
            [Description("Editor")]
            Editor=5,
            [Description("Email")]
            Email = 47,
            [Description("Text")]
            Text = 21,
            [Description("Radio")]
            Radiobutton = 4,
            [Description("Checkbox")]
            Checkbox = 3,
        }

        public class BacklogSetupDropdownClasses
        {
            public const string simple = "Simple";
            public const string autoComplete = "Autocomplete";
            public const string multiAuto = "MultiAuto";
        }

        public enum ContactColumn
        {
            [Description("Referal")]
            Referal = 1,
            [Description("Name")]
            Name = 2,
            [Description("Timeframe To Start Franchise")]
            Timeframe = 3,
            [Description("Email")]
            Email = 4,
            [Description("Phone")]
            Phone = 5,
            [Description("State")]
            State = 6,
            [Description("FirstCity")]
            FirstCity = 7,
            [Description("SecondCity")]
            SecondCity = 8,
            [Description("Message")]
            Message = 9,
            [Description("ButtonText")]
            ButtonText = 10
        }
        public enum ColspanEnum : int
        {
            [Description("Half")]
            one = 1,
            [Description("Full")]
            two = 2
        }

        public enum StudentEnrollmentSection
        {
            [Description("Header")]
            Header = 1,

            [Description("Referal Information")]
            ReferalInformation = 2,

            [Description("Family Information")]
            FamilyInformation = 3,

            [Description("Contact No 1")]
            ContactNo1 = 4,

            [Description("Contact No 2")]
            ContactNo2 = 5,

            [Description("Student 1")]
            Student1 = 6,

            [Description("Footer")]
            Footer = 7,



            [Description("STEM Builders of Plymouth")]
            STEMBuildersofPlymouth = 8,

            [Description("Policies and Agreement")]
            PoliciesandAgreement = 9,

            [Description("Header")]
            FranchiseHeader = 10,
            [Description("Personal Details")]
            FranchisePersonalDetails = 11,

            [Description("Educational Details")]
            FranchiseEducationalDetails = 12,
            [Description("Employment Details")]
            FranchiseEmploymentDetails = 13,
            [Description("Franchise/Center Setup Details")]
            FranchiseSetupDetails = 14,
            [Description("Franchise/Center Partners")]
            FranchisePartners = 15,
            [Description("Electronic Signatures")]
            FranchiseElectronicSignature = 16,
            [Description("Payment Information")]
            PaymentInformation = 17,
            [Description("Personal Details")]
            EventEnrollPersonalDetails = 18,
            [Description("Contact Details")]
            EventEnrollContactDetails = 19,

        }

    }

}
