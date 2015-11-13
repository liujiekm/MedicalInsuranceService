using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace MedicalInsuranceService.Filter
{
    public class DeflateCompressionAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var content = actionExecutedContext.Response.Content;
            var bytes = content ==null?null: content.ReadAsByteArrayAsync().Result;

            var zlibbedContent = bytes == null ? new byte[0] : CompressionHelper.DeflateByte(bytes);
            actionExecutedContext.Response.Content = new ByteArrayContent(zlibbedContent);
            actionExecutedContext.Response.Content.Headers.Remove("Content-Type");
            actionExecutedContext.Response.Content.Headers.Add("Content-encoding","deflate");
            actionExecutedContext.Response.Content.Headers.Add("Content-Type", "application/json");
            base.OnActionExecuted(actionExecutedContext);
        }
    }



    public class CompressionHelper
    {
        public static byte[] DeflateByte(byte[] content)
        {
            if(null==content)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                using (var compressor = new DeflateStream(stream,CompressionMode.Compress,CompressionLevel.BestSpeed))
                {
                    compressor.Write(content,0,content.Length);
                }

                return stream.ToArray();
            }
        }
    }

}