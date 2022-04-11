﻿using System.Net;

namespace ChurchManager.SharedKernel.Extensions
{
    public static class FormatText
    {
        public static string ConvertText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = WebUtility.HtmlEncode(text);

            text = text.Replace("\r\n", "<br />");
            text = text.Replace("\r", "<br />");
            text = text.Replace("\n", "<br />");
            text = text.Replace("\t", "&nbsp;&nbsp;");
            text = text.Replace("  ", "&nbsp;&nbsp;");

            return text;
        }
    }
}
