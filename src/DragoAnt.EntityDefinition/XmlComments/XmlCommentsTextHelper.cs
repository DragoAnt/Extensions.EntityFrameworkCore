﻿using System.Net;
using System.Text.RegularExpressions;

namespace DragoAnt.EntityDefinition.XmlComments;

//based on https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/95cb4d370e08e54eb04cf14e7e6388ca974a686e/src/Swashbuckle.AspNetCore.SwaggerGen/XmlComments/XmlCommentsTextHelper.cs
public static class XmlCommentsTextHelper
{
    private static readonly Regex RefTagPattern = new(@"<(see|paramref) (name|cref)=""([TPF]{1}:)?(?<display>.+?)"" ?/>");
    private static readonly Regex CodeTagPattern = new(@"<c>(?<display>.+?)</c>");
    private static readonly Regex ParaTagPattern = new(@"<para>(?<display>.+?)</para>", RegexOptions.Singleline);

    public static string Humanize(string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        //Call DecodeXml at last to avoid entities like &lt and &gt to break valid xml          

        return text
            .NormalizeIndentation()
            .HumanizeRefTags()
            .HumanizeCodeTags()
            .HumanizeParaTags()
            .DecodeXml();
    }

    private static string NormalizeIndentation(this string text)
    {
        var lines = text.Split('\n');
        var padding = GetCommonLeadingWhitespace(lines);

        var padLen = padding?.Length ?? 0;

        // remove leading padding from each line
        for (int i = 0, l = lines.Length; i < l; ++i)
        {
            var line = lines[i].TrimEnd('\r'); // remove trailing '\r'

            if (padLen != 0 && line.Length >= padLen && line.Substring(0, padLen) == padding)
            {
                line = line[padLen..];
            }

            lines[i] = line;
        }

        // remove leading empty lines, but not all leading padding
        // remove all trailing whitespace, regardless
        return string.Join("\r\n", lines.SkipWhile(string.IsNullOrWhiteSpace)).TrimEnd();
    }

    private static string? GetCommonLeadingWhitespace(string[] lines)
    {
        if (null == lines)
        {
            throw new ArgumentException("lines");
        }

        if (lines.Length == 0)
        {
            return null;
        }

        var nonEmptyLines = lines
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        if (nonEmptyLines.Length < 1)
        {
            return null;
        }

        var padLen = 0;

        // use the first line as a seed, and see what is shared over all nonEmptyLines
        var seed = nonEmptyLines[0];
        for (int i = 0, l = seed.Length; i < l; ++i)
        {
            if (!char.IsWhiteSpace(seed, i))
            {
                break;
            }

            if (nonEmptyLines.Any(line => line[i] != seed[i]))
            {
                break;
            }

            ++padLen;
        }

        return padLen > 0 ? seed[..padLen] : null;
    }

    private static string HumanizeRefTags(this string text)
    {
        return RefTagPattern.Replace(text, (match) => match.Groups["display"].Value);
    }

    private static string HumanizeCodeTags(this string text)
    {
        return CodeTagPattern.Replace(text, (match) => "{" + match.Groups["display"].Value + "}");
    }

    private static string HumanizeParaTags(this string text)
    {
        return ParaTagPattern.Replace(text, (match) => "<br>" + match.Groups["display"].Value);
    }

    private static string DecodeXml(this string text)
    {
        return WebUtility.HtmlDecode(text);
    }

}