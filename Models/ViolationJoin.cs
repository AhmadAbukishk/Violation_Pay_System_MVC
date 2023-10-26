namespace Traffic_Violation.Models
{
    public class ViolationJoin
    {
        public ProjectViolation violation { get; set; }
        public ProjectViolationType violationType { get; set; }
        public ProjectViolationState violationState { get; set; }
        public ProjectCar car { get; set; }


    }
}
