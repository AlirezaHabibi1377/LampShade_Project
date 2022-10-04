using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0_Framework.Application
{
    public class ApplicationMessages
    {
        public const string DuplicatedRecord = ".امکان اضافه کردن رکورد تکراری وجود ندارد. لطفا مجدد تلاش بفرمایید";
        public const string RecordNotFound = "رکورد با اطلاعات درخواست شده یافت نشد. لطفا مجدد تلاش بفرمایید.";

        public static string PasswordsNotMatch = "پسورد با تکرار آن مطابقت ندارد";
        public static string WrongUserPass = "کاربری با این نام کاربری و رمز عبور یافت نشد";
    }
}