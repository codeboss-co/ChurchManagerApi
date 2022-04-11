namespace ChurchManager.Persistence.Shared
{
    public abstract class AuditableEntity<TPrimaryKey> : Entity<TPrimaryKey>
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
