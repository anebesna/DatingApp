namespace DatingAppAPI.Extensions;

public static class DateTimeExtensions
{
    public static int CalculateAge(this DateOnly bday)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - bday.Year;
        if (bday > today.AddYears(-age)) age--;
        return age;
    }
}