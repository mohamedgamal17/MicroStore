namespace MicroStore.ShoppingGateway.ClinetSdk.Entities
{
    [Serializable]
    public class ListResult<T>
    {
        public List<T> Items { get; set; }
    }
}
