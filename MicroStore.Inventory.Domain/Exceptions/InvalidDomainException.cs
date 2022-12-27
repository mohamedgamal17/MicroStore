namespace MicroStore.Inventory.Domain.Exceptions
{
    public class InvalidDomainException : Exception
    {
        public Type EntityType { get; set; }
        public string ErrorType { get; set; }

        public InvalidDomainException(Type entityType,string errorType, string message) : base(message) 
        {
            EntityType = entityType;
            ErrorType = errorType;
        }
    }
}
