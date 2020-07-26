namespace ParkyWeb
{
    public class StaticDetails
    {
        public const string ApiBaseUrl = "https://localhost:5001/";
        public static string NationalParksApiPath = $"{ApiBaseUrl} api/v1/nationalparks";
        public static string TrailsApiPath = $"{ApiBaseUrl} api/v1/trails";
    }
}
