namespace API_Simulacao_Hack.Util
{
    public static class DatetimeExtension
    {
        public static DateTime GetDataAtual(this DateTime dataAtual)
        {
            TimeZoneInfo brZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            dataAtual = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brZone);
            return dataAtual;
        }
    }
}
