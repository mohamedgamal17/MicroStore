namespace MicroStore.TestBase.Json
{
    [Serializable]
    public class JsonWrapper <T>
    {
        public List<T> Data { get; set; }

    }
}
