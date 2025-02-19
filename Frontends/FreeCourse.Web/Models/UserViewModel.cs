namespace FreeCourse.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        // bu metot cagrıldıgında kullanıcı bilgilerini dondurur
        public IEnumerable<string> GetUserProps()
        {
            yield return UserName;
            yield return Email;
        }
    }
}
