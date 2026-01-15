namespace Garage_2._0.Models
{
    public class MemberDetailsVM
    {
        public ApplicationUser Member { get; set; }
        public List<ParkedVehicle> Vehicles { get; set; }
        public double TotalCost { get; set; }

    }
}
