namespace Userr
{
    //this class is made to store the id, username, password and "user type"
    //of all users including administrators and customers. Our
    //administrator (if needed) and customer classes will be inherited from
    //this class. Because admins and customers have these properties in common.

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
    }
}