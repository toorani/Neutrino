namespace Neutrino.Identity
{
    public class IdentityUser : Espresso.Identity.IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool Deleted { get; set; }

    }
}
