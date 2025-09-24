public class BaseEntity<TKey>
{
    // Properties
    public TKey Id { get; set; } = default!; // PK
    public DateTime CreatedOn { get; set; }
    public DateTime LastModifiedOn { get; set; }
}

