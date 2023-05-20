using EntityFrameworkDemo.DB;
namespace NewsWebApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    //public static bool CheckUser(string name, string pwd) {
    //        try
    //        {
    //            using DBContext context = new DBContext();
    //            var user = context.User.FirstOrDefault(s => s.Username == name && s.Password == pwd);
    //            if (user == null)
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                return true;
    //            }
    //        }
    //        catch (System.Exception err) {
    //            return false;
    //        }
    //    }
    }
}
