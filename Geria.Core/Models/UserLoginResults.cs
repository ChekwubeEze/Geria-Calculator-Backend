using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geria.Core.Models
{
    public enum UserLoginResults
    {
        ///<summary>
        ///Login Successful
        ///</summary>
        SUCCESSFUL = 1,
        ///<summary>
        ///Customer does not exist (email or username)
        ///</summary>
        USER_DOES_NOT_EXIST = 2,
        ///<summary>
        ///Wrong Password
        ///</summary>
        WRONG_PASSWORD = 3,
        ///<summary>
        ///Have not been verified
        ///</summary>
        NOT_VERIFIED = 4,
        ///<summary>
        ///Customer has been Deleted
        ///</summary>
        DELETED = 5,
        ///<summary>
        ///Customer not registered
        ///</summary>
        NOT_REGISTERED = 6,
        ///<summary>
        ///Locked out
        ///</summary>
        LOCKED_OUT = 7,
        ///<summary>
        USER_NOT_ACTIVE = 8,
        ///</summary>
    }
}
