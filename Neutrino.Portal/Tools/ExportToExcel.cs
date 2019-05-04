﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Neutrino.Portal.Tools
{
    public static class ExportToExcel
    {
        /// <summary>
        /// convert Generic data to TSV (Tab Separated Values) Format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="output"></param>
        public static void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }

        public static HttpResponseMessage GetExcelFile<T>(IEnumerable<T> data, string outputFileName, string excelTemplatePath, string caption = "")
        {
            HtmlDocument htmlExcelTemplate = new HtmlDocument();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            htmlExcelTemplate.Load(excelTemplatePath);

            var tableNode = htmlExcelTemplate.DocumentNode.SelectSingleNode("//table");
            if (tableNode != null)
            {
                var captionElement = tableNode.Element("caption");
                if (string.IsNullOrWhiteSpace(caption) == false && captionElement != null)
                {

                    var caption_span = HtmlNode.CreateNode($"<span style='font-weight: bold;font-family:Tahoma;font-size:14px'>{caption} </span>");
                    captionElement.AppendChild(caption_span);
                }
                var tbodyElement = tableNode.Element("tbody");
                if (tbodyElement != null)
                {
                    var trbodyElement = tbodyElement.Element("tr");

                    string trTemplate = trbodyElement.InnerHtml.ToLower();
                    trbodyElement.RemoveAllChildren();
                    tbodyElement.RemoveAllChildren();
                    HtmlNode trHtmlNode;
                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        string trdata = trTemplate;
                        foreach (PropertyDescriptor prop in props)
                        {
                            trdata = trdata.Replace(prop.Name.ToLower(), prop.Converter.ConvertToString(prop.GetValue(item)));
                        }
                        trHtmlNode = HtmlNode.CreateNode("<tr></tr>");
                        trHtmlNode.InnerHtml = trdata;
                        tbodyElement.AppendChild(trHtmlNode);

                    }
                }
            }
            byte[] bytes = Encoding.UTF8.GetBytes(tableNode.OuterHtml);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName + ".xls"
            };
            return result;
        }

        public static HttpResponseMessage GetExcelFile<T>(List<T> data, string outputFileName, string excelTemplatePath
            , string caption = ""
            , Func<List<T>, string, string> generatorHeader = null
            , Func<T, string, string> generatorRowBody = null
            , Func<T, IEnumerable<object>> getLoopObjects = null)
        {
            HtmlDocument htmlExcelTemplate = new HtmlDocument();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            htmlExcelTemplate.Load(excelTemplatePath);

            var tableNode = htmlExcelTemplate.DocumentNode.SelectSingleNode("//table");
            if (tableNode != null)
            {
                var captionElement = tableNode.Element("caption");
                if (string.IsNullOrWhiteSpace(caption) == false && captionElement != null)
                {

                    var caption_span = HtmlNode.CreateNode($"<span style='font-weight: bold;font-family:Tahoma;font-size:14px'>{caption} </span>");
                    captionElement.AppendChild(caption_span);
                }

                var theadElement = tableNode.Element("thead");
                var loopHeaderNode = theadElement.SelectSingleNode("//loop");


                if (loopHeaderNode != null)
                {
                    if (generatorHeader != null)
                    {
                        string header = generatorHeader(data, loopHeaderNode.InnerHtml);
                        theadElement.InnerHtml = theadElement.InnerHtml.Replace(loopHeaderNode.OuterHtml, header);
                    }
                    else if (getLoopObjects != null && data.Count != 0)
                    {
                        string loopHeaderTemplate = string.Empty;
                        loopHeaderTemplate = loopHeaderNode.InnerHtml.ToLower();
                        loopHeaderNode.RemoveAll();

                        IEnumerable<object> nestedObjects = getLoopObjects(data.First());
                        var loopValues = "";
                        foreach (object nestedObj in nestedObjects)
                        {
                            var nestedProps = TypeDescriptor.GetProperties(nestedObj);
                            var loopHeader = loopHeaderTemplate;
                            foreach (PropertyDescriptor prop in nestedProps)
                            {
                                loopHeader = loopHeader.Replace(prop.Name.ToLower(), prop.Converter.ConvertToString(prop.GetValue(nestedObj)));
                            }
                            loopValues += loopHeader;

                        }
                        theadElement.InnerHtml = theadElement.InnerHtml.Replace("<loop></loop>", loopValues);
                    }
                }


                var tbodyElement = tableNode.Element("tbody");
                if (tbodyElement != null)
                {
                    var trbodyElement = tbodyElement.Element("tr");
                    HtmlNode loopBodyNode = trbodyElement.SelectSingleNode("//loop");
                    string loopBodyTemplate = string.Empty;
                    if (loopBodyNode != null)
                    {
                        loopBodyTemplate = loopBodyNode.InnerHtml.ToLower();
                        loopBodyNode.RemoveAll();
                    }

                    string trTemplate = trbodyElement.InnerHtml.ToLower();
                    trbodyElement.RemoveAllChildren();
                    tbodyElement.RemoveAllChildren();

                    HtmlNode trHtmlNode;
                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        string trdata = trTemplate;
                        foreach (PropertyDescriptor prop in props)
                        {
                            trdata = trdata.Replace(prop.Name.ToLower(), prop.Converter.ConvertToString(prop.GetValue(item)));
                        }
                        if (string.IsNullOrWhiteSpace(loopBodyTemplate) == false)
                        {
                            if (generatorRowBody != null)
                            {
                                var loopValues = generatorRowBody(item, loopBodyTemplate);
                                trdata = trdata.Replace("<loop></loop>", loopValues);
                            }
                            else if (getLoopObjects != null)
                            {
                                IEnumerable<object> nestedObjects = getLoopObjects(item);
                                var loopValues = "";
                                foreach (object nestedObj in nestedObjects)
                                {
                                    var nestedProps = TypeDescriptor.GetProperties(nestedObj);
                                    var loopRecord = loopBodyTemplate;
                                    foreach (PropertyDescriptor prop in nestedProps)
                                    {
                                        loopRecord = loopRecord.Replace(prop.Name.ToLower(), prop.Converter.ConvertToString(prop.GetValue(nestedObj)));
                                    }
                                    loopValues += loopRecord;

                                }
                                trdata = trdata.Replace("<loop></loop>", loopValues);
                            }
                        }

                        trHtmlNode = HtmlNode.CreateNode("<tr></tr>");
                        trHtmlNode.InnerHtml = trdata;
                        tbodyElement.AppendChild(trHtmlNode);

                    }
                }
            }
            byte[] bytes = Encoding.UTF8.GetBytes(tableNode.OuterHtml);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new MemoryStream(bytes);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = outputFileName + ".xls"
            };
            return result;
        }
    }
}