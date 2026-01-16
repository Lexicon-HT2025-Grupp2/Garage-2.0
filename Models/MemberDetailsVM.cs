namespace Garage_2._0.Models
{
    public class MemberDetailsVM
    {
        public ApplicationUser Member { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public double TotalCost { get; set; }

    }
}
