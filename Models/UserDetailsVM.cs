namespace Garage_2._0.Models
{
    public class UserDetailsVM
    {
        public ApplicationUser User { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public double TotalCost { get; set; }
        public string Role { get; set; }
    }
}
