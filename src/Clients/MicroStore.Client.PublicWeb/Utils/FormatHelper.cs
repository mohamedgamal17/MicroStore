namespace MicroStore.Client.PublicWeb.Utils
{
    public static class FormatHelper
    {
        public static string FormatOrderNumber(string orderNumber , string seprator = " ") 
        {
            List<string> subStrings = new List<string>();

            for (int i = 0; i < orderNumber.Length; i = i + 3)
            {
                if (i % 3 == 0)
                {
                    subStrings.Add(orderNumber.Substring(i, 3));
                }

            }

            var formatedOrderNumber = subStrings.JoinAsString(seprator);

            return formatedOrderNumber;
        }
    }
}
