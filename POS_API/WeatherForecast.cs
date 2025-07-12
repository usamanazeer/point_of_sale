using System;

namespace POS_API
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + Convert.ToInt32(TemperatureC / 0.5556);
        public string Summary { get; set; }
    }
}
