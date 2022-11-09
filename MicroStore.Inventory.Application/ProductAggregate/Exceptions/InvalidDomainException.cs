namespace MicroStore.Inventory.Application.ProductAggregate.Exceptions
{
    public class InvalidDomainException : Exception
    {
        public InvalidDomainException(string message) : base(message)
        {

        }
    }
}
