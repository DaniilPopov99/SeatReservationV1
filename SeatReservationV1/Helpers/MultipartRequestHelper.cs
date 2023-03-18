using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.ComponentModel;

namespace SeatReservationV1.Helpers
{
    public static class MultipartRequestHelper
	{
		public static bool IsMultipartContentType(string contentType)
		{
			return !string.IsNullOrEmpty(contentType)
				   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
		{
			var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;
			if (string.IsNullOrWhiteSpace(boundary))
			{
				throw new InvalidDataException("Missing content-type boundary.");
			}

			if (boundary.Length > lengthLimit)
			{
				throw new InvalidDataException(
					$"Multipart boundary length limit {lengthLimit} exceeded.");
			}

			return boundary;
		}

		public static async Task<byte[]> GetMultipartSectionContentBytes(MultipartSection section)
		{

			var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

			byte[] content;

			using (Stream targetStream = new MemoryStream())
			{
				if (hasContentDispositionHeader)
				{
					await section.Body.CopyToAsync(targetStream);
				}

				if (targetStream.Length > int.MaxValue)
				{
					throw new FileLoadException();
				}

				content = new byte[targetStream.Length];
				targetStream.Position = 0;
				targetStream.Read(content, 0, (int)targetStream.Length);
			}

			return content;
		}

		public static TResult TryGetMultipartSectionHeaderValue<TResult>(MultipartSection section, string key, bool notRequiredAndAllowDefault = false)
		{
			if (!section.Headers.TryGetValue(key, out var resultString) || string.IsNullOrEmpty(resultString))// || resultString == "null")
			{
				if (notRequiredAndAllowDefault)
				{
					return default;
				}

				throw new ArgumentException($"Header must have key \"{key}\" and nonempty valid value");
			}
			resultString = Uri.UnescapeDataString(resultString);

			var converter = TypeDescriptor.GetConverter(typeof(TResult));
			if (converter != null)
			{
				return (TResult)converter.ConvertFromString(resultString);
			}

			throw new ArgumentException($"Header must have key \"{key}\" and nonempty valid value");
		}
	}
}
