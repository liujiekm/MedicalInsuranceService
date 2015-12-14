using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

using Jil;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace MedicalInsuranceService.Formatter
{

    /// <summary>
    /// 替换API 内部默认的Newton.json来处理JSON数据的方式
    /// </summary>
    public class JilFormatter : MediaTypeFormatter
    {

        private readonly Options _jilOptions;

        public JilFormatter()
        {
            _jilOptions = new Options(dateFormat: DateTimeFormat.ISO8601,includeInherited:true);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian:false,byteOrderMark:true,throwOnInvalidBytes:true));
        }

        public override bool CanReadType(Type type)
        {
            if(null==type)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (null == type)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }


        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream,HttpContent content,IFormatterLogger formatterLogger)
        {
            return Task.FromResult(this.DeserializeFromStream(type,readStream));
        }



        private object DeserializeFromStream(Type type,Stream readStream)
        {
            try
            {
                using (var reader = new StreamReader(readStream))
                {
                    MethodInfo method = typeof(JSON).GetMethod("Deserialize",new Type[] { typeof(TextReader),typeof(Options)});
                    MethodInfo gengric = method.MakeGenericMethod(type);
                    return gengric.Invoke(this, new object[] { reader, _jilOptions });
                }
            }
            catch
            {

                return null; ;
            }
        }


        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            JSON.Serialize(value,streamWriter,_jilOptions);
            streamWriter.Flush();
            return Task.FromResult(writeStream);
        }
    }
}