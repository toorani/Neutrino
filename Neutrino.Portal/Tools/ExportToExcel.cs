using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
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
        public static void WriteHtmlTable<T>(IEnumerable<T> data, TextWriter output)
        {
            //Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {

                    //  Create a form to contain the List
                    Table table = new Table();
                    TableRow row = new TableRow();
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                    foreach (PropertyDescriptor prop in props)
                    {
                        TableHeaderCell hcell = new TableHeaderCell();
                        hcell.Text = prop.Name;
                        hcell.BackColor = System.Drawing.Color.Yellow;
                        row.Cells.Add(hcell);
                    }

                    table.Rows.Add(row);

                    //  add each of the data item to the table
                    foreach (T item in data)
                    {
                        row = new TableRow();
                        foreach (PropertyDescriptor prop in props)
                        {
                            TableCell cell = new TableCell();
                            cell.Text = prop.Converter.ConvertToString(prop.GetValue(item));
                            row.Cells.Add(cell);
                        }
                        table.Rows.Add(row);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    output.Write(sw.ToString());
                }
            }

        }

        public static HttpResponseMessage WriteHtmlTable<T>(IEnumerable<T> data, string outputFileName, string excelTemplatePath,string caption = "")
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
                    captionElement.InnerHtml = caption;
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
    }
}