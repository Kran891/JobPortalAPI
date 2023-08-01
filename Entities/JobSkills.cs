namespace JobPortal.Entities
{
    public class JobSkills
    {
        public int Id { get; set; }
        public Skills Skill { get; set; }
        public Jobs job { get; set; }
    }
}
