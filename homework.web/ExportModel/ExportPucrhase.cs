using homework.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using GemBox.Document;

namespace homework.web.ExportModel
{
    public class ExportPucrhase : PurchaseViewModel
    {
        public string Format { get; set; } = "PDF";
        public SaveOptions Options => this.FormatMappingDictionary[this.Format];
        public IDictionary<string, SaveOptions> FormatMappingDictionary => new Dictionary<string, SaveOptions>()
        {
            ["DOCX"] = new DocxSaveOptions(),
            ["HTML"] = new HtmlSaveOptions() { EmbedImages = true },
            ["RTF"] = new RtfSaveOptions(),
            ["TXT"] = new TxtSaveOptions(),
            ["PDF"] = new PdfSaveOptions(),
            ["XPS"] = new XpsSaveOptions(),
            ["XML"] = new XmlSaveOptions(),
            ["BMP"] = new ImageSaveOptions(ImageSaveFormat.Bmp),
            ["PNG"] = new ImageSaveOptions(ImageSaveFormat.Png),
            ["JPG"] = new ImageSaveOptions(ImageSaveFormat.Jpeg),
            ["GIF"] = new ImageSaveOptions(ImageSaveFormat.Gif),
            ["TIF"] = new ImageSaveOptions(ImageSaveFormat.Tiff)
        };
    }
}
