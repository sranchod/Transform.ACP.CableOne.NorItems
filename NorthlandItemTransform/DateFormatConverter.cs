using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MtShellProgram
{
	public class DateFormatConverter : IsoDateTimeConverter
	{
		private TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null) return null;
			var easternTime = DateTime.Parse(reader.Value.ToString());
			return TimeZoneInfo.ConvertTimeFromUtc(easternTime, easternZone);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.DateFormatString = DateTimeFormat;
			writer.WriteValue(TimeZoneInfo.ConvertTimeToUtc((DateTime)value, easternZone));
		}

		public DateFormatConverter(string format)
		{
			DateTimeFormat = format;
			//DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
		}
	}
}
