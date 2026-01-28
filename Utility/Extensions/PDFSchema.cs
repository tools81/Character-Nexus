using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace Utility
{
    public static class PDFSchema
    {
        public static byte[] Generate(Dictionary<string, string> dict, string path)
        {
            PdfReader reader = new PdfReader(path);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfStamper stamper = new PdfStamper(reader, stream);
                AcroFields formFields = stamper.AcroFields;

                foreach (var field in dict)
                {
                    formFields.SetField(field.Key, field.Value);
                }

                stamper.FormFlattening = true;
                stamper.Close();

                return stream.ToArray();  
            }
        }
    }
}
