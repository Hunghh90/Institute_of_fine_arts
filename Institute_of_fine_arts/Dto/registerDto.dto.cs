namespace Institute_of_fine_arts.Dto
{
    public class registerDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public DateTime JoinDate { get; set; }
        public int RoleId { get; set; }
    }
}
