using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace POS_API.Utilities
{
    public static class IFormFileExtensions
    {
        public static string GetFilename(this IFormFile file)
        {
            return ContentDispositionHeaderValue.Parse(
                                                       input: file.ContentDisposition).FileName.Trim(trimChar: '"');
        }


        public static async Task<MemoryStream> GetFileStream(this IFormFile file)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(target: memoryStream);
            return memoryStream;
        }


        public static async Task<byte[]> GetFileArray(this IFormFile file)
        {
            var millstream = new MemoryStream();
            await file.CopyToAsync(target: millstream);
            return millstream.ToArray();
        }


        public static async Task<bool> SaveFileAsync(this IFormFile file,
                                                     string path)
        {
            try
            {
                await using var stream = new FileStream(path: path, mode: FileMode.Create);
                await file.CopyToAsync(target: stream);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}