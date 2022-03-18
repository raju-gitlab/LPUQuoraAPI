using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.MODEL.Master
{
    public class AuthModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public GenderModel Gender { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string UserImagePath { get; set; }
        public string UserOrganizationName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }
        public Guid Userguid { get; set; }
        public int UserType { get; set; }
        public bool Twofactorauthentication { get; set; }
    }
    public class PasswordAuthorizationModel
    {
        public string UserId { get; set; }
        public Guid Authorizationkey { get; set; }
        public bool IsUsed { get; set; }
        public DateTimeOffset RequestTime{ get; set; }
        public DateTime ValidateTill{ get; set; }
    }
}
