namespace Kvota.Models.UserAuth
{
    public class CurrentUser
    {
        //public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public Dictionary<string, string> Claims { get; set; }
    }
}
