using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Timing;
using JetBrains.Annotations;
using Shesha.Authorization.Users;
using Shesha.Domain.Attributes;
using Shesha.Domain.Enums;
using Shesha.EntityHistory;
using Shesha.Extensions;

namespace Shesha.Domain
{
    [Entity(TypeShortAlias = "Shesha.Core.Person")]
    [Table("Core_Persons") /* pluralize() returns wrong version */]
    [Discriminator]
    [DisplayManyToManyAuditTrail(typeof(ShaRoleAppointedPerson), "Role", DisplayName = "Role Appointment")]
    public class Person : FullPowerEntity
    {
        [Display(Name = "Organisation")]
        public virtual Organisation PrimaryOrganisation { get; set; }

        public virtual Person Supervisor { get; set; }

        [StoredFile(IsVersionControlled = true)]
        public virtual StoredFile Photo { get; set; }

        [NotMapped]
        public virtual string FirstNameAndInitials =>
            FirstName + (string.IsNullOrEmpty(Initials) ? (string.IsNullOrEmpty(LastName) ? "" : $" {LastName[0]}"
                ) : Initials);

        [NotMapped]
        public virtual string FirstNameInitials => !string.IsNullOrWhiteSpace(FirstName) ? $" {FirstName[0]}" : "";

        /*
        public virtual bool IsRegistered { get; set; }
        public virtual Guid LinkId { get; set; }
        */

        [StringLength(13)]
        [Display(Name = "Identity Number")]
        public virtual string IdentityNumber { get; set; }

        [StoredFile(IsVersionControlled = true)]
        [Display(Name = "Signature")]
        public virtual StoredFile SignatureFile { get; set; }

        [StoredFile(IsVersionControlled = true)]
        [Display(Name = "Small Signature")]
        public virtual StoredFile SmallSignatureFile { get; set; }

        public virtual RefListPersonTitle? Title { get; set; }

        [StringLength(50)]
        [Display(Name = "First Name")]
        [Audited]
        public virtual string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        [Audited]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Initials override. If empty, the first letter of FirstName is taken.
        /// </summary>
        [StringLength(10), Display(Name = "Initials")]
        public virtual string Initials { get; set; }

        /// <summary>
        /// Custom short name (overrides calculated short name)
        /// </summary>
        [StringLength(60)]
        [Display(Name = "Custom Short Name")]
        public virtual string CustomShortName { get; set; }

        [StringLength(20)]
        public virtual string HomeNumber { get; set; }

        [StringLength(20)]
        [Display(Name = "Office Number")]
        public virtual string OfficeNumber { get; set; }

        [StringLength(20)]
        [Display(Name = "Mobile Number")]
        [Audited]
        public virtual string MobileNumber1 { get; set; }

        [StringLength(20)]
        [Display(Name = "Alternate Mobile Number")]
        public virtual string MobileNumber2 { get; set; }

        [StringLength(100), EmailAddress]
        [Display(Name = "Email Address")]
        [Audited]
        public virtual string EmailAddress1 { get; set; }

        [StringLength(100), EmailAddress]
        [Display(Name = "Alternative Email Address")]
        public virtual string EmailAddress2 { get; set; }

        [StringLength(20)]
        [Display(Name = "Fax Number")]
        public virtual string FaxNumber { get; set; }

        [Past]
        [Audited]
        [DisableDateTimeNormalization]
        public virtual DateTime? DateOfBirth { get; set; }

        [Audited]
        public virtual RefListGender? Gender { get; set; }

        [ReferenceList("Shesha.Core", "PreferredContactMethod")]
        public virtual int? PreferredContactMethod { get; set; }

        /// <summary>
        /// Calcuated property in the following format: FirstName + ' ' + LastName
        /// </summary>
        [EntityDisplayName]
        [ReadonlyProperty]
        public virtual string FullName { get; protected set; }

        /// <summary>
        /// NHibernate Calcuated property in the following format: LastName + ', ' + FirstName
        /// </summary>
        [ReadonlyProperty]
        public virtual string FullName2 { get; protected set; }

        [StringLength(100)]
        [Display(Name = "Department")]
        public virtual string OrganisationDepartment { get; set; }

        [StringLength(100)]
        [Display(Name = "Job Title")]
        public virtual string OrganisationJobTitle { get; set; }

        /// <summary>
        /// Returns the first available email address. i.e returns EmailAddress1 or EmailAddress2 if 
        /// they have a valid values, otherwise returns empty string.
        /// </summary>
        public virtual string AvailableEmail
        {
            get
            {
                if (!string.IsNullOrEmpty(EmailAddress1))
                    return EmailAddress1;
                else if (!string.IsNullOrEmpty(EmailAddress2))
                    return EmailAddress2;
                else
                    return string.Empty;
            }
        }

        public virtual string TitleName
        {
            get
            {
                return this.GetReferenceListDisplayText(e => e.Title);
            }
        }

        public virtual string FullNameWithTitle => (string.IsNullOrEmpty(TitleName) ? "" : (TitleName + " ")) + FullName;

        /// <summary>
        /// Title, initial and last name
        /// </summary>
        public virtual string ShortNameWithTitle =>
            (string.IsNullOrEmpty(TitleName) ? "" : (TitleName + " ")) +
            ShortName;

        /// <summary>
        /// Title, initial and last name
        /// </summary>
        public virtual string ShortName2WithTitle =>
            (string.IsNullOrEmpty(TitleName) ? "" : (TitleName + " ")) +
            ShortName2;

        /// <summary>
        /// Returns true if any form of contact details are available i.e. telephone or email.
        /// </summary>
        public virtual bool HasContactDetails =>
            (!string.IsNullOrEmpty(MobileNumber1)
             || !string.IsNullOrEmpty(MobileNumber2)
             || !string.IsNullOrEmpty(EmailAddress1)
             || !string.IsNullOrEmpty(EmailAddress2));

        /// <summary>
        /// Person's mobile number
        /// </summary>
        public virtual string MobileNumber => !string.IsNullOrEmpty(MobileNumber1)
            ? MobileNumber1
            : MobileNumber2;

        /// <summary>
        /// Short name (initials first)
        /// </summary>
        public virtual string ShortName =>
            string.IsNullOrEmpty(CustomShortName)
                ? (string.IsNullOrEmpty(Initials)
                      ? (string.IsNullOrEmpty(FirstName) ? "" : ($" {FirstName[0]}" + " "))
                      : (Initials + " ")) +
                  LastName
                : CustomShortName;

        /// <summary>
        /// Short name (last name first)
        /// </summary>
        public virtual string ShortName2 =>
            string.IsNullOrEmpty(CustomShortName)
                ? LastName + " " +
                  (string.IsNullOrEmpty(Initials)
                      ? (string.IsNullOrEmpty(FirstName) ? "" : $" {FirstName[0]}")
                      : Initials)
                : CustomShortName;

        [Display(Name = "Type of account")]
        public virtual RefListTypeOfAccount? TypeOfAccount { get; set; }

        [Display(Name = "Require a change of password")]
        public virtual bool RequireChangePassword { get; set; }

        private class IsLockedEventCreator : EntityHistoryEventCreatorBase<Person, bool>
        {
            public override EntityHistoryEventInfo CreateEvent(EntityChangesInfo<Person, bool> change)
            {
                var text = change.NewValue ? "User locked" : "User unlocked";
                return CreateEvent(text, text);
            }
        }
        [Display(Name = "Account is locked")]
        [AuditedAsEvent(typeof(IsLockedEventCreator))]
        public virtual bool IsLocked { get; set; }

        [Display(Name = "Email Address Confirmed")]
        public virtual bool EmailAddressConfirmed { get; set; }

        [Display(Name = "Mobile Number Confirmed")]
        public virtual bool MobileNumberConfirmed { get; set; }

        [Display(Name = "Authentication Guid")]
        [StringLength(36)]
        public virtual string AuthenticationGuid { get; set; }

        [Display(Name = "Authentication Guid Expiry Date")]
        public virtual DateTime? AuthenticationGuidExpiresOn { get; set; }

        /// <summary>
        /// One Time Passwords by SMS
        /// </summary>
        [DisplayFormat(DataFormatString = "Yes|No")]
        [Display(Name = "Use SMS Based One-Time-Passwords")]
        [AuditedBoolean("SMS Based One-Time-Passwords enabled", "SMS Based One-Time-Passwords disabled")]
        public virtual bool OtpEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsContractor { get; set; }

        /// <summary>
        /// User record, may be null for non registered users
        /// </summary>
        [CanBeNull]
        public virtual User User { get; set; }

        public virtual Area AreaLevel1 { get; set; }
        public virtual Area AreaLevel2 { get; set; }
        public virtual Area AreaLevel3 { get; set; }
        public virtual Area AreaLevel4 { get; set; }

        public override string ToString()
        {
            return FullName;
        }

        public virtual bool IsMobileVerified { get; set; }
        public virtual string SecurityPin { get; set; }
        [MultiValueReferenceList("Shesha.Core", "CommonLanguage")]
        public virtual int? PreferredLanguage { get; set; }
    }
}
