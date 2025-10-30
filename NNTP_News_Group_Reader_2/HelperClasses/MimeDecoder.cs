using System.Text;
using System.Text.RegularExpressions;

public static class MimeDecoder
{
    // Matches MIME patterns like:
    // =?UTF-8?Q?...?=   or   =?ISO-8859-1?B?...?=
    private static readonly Regex MimeRegex = new Regex(@"=\?(.*?)\?([QB])\?(.*?)\?=", RegexOptions.IgnoreCase);

    /// <summary>
    /// Decodes MIME-encoded text (used in NNTP headers).
    /// Handles UTF-8, ISO-8859-1, ISO-8859-15 and Windows-1252, 
    /// and supports both Q and Base64 encodings.
    /// </summary>
    public static string Decode(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return MimeRegex.Replace(input, match =>
        {
            string charset = match.Groups[1].Value.ToUpper();
            string encodingType = match.Groups[2].Value.ToUpper();
            string encodedText = match.Groups[3].Value;

            // Try to pick the correct encoding, or fallback safely
            Encoding encoding;
            try
            {
                encoding = charset switch
                {
                    "ISO-8859-1" => Encoding.GetEncoding("ISO-8859-1"),
                    "ISO-8859-15" => Encoding.GetEncoding("ISO-8859-15"),
                    "WINDOWS-1252" => Encoding.GetEncoding("Windows-1252"),
                    _ => Encoding.UTF8
                };
            }
            catch
            {
                encoding = Encoding.UTF8;
            }

            string decoded;

            if (encodingType == "B")
            {
                // Base64 decode (used for some UTF-8 or ISO headers)
                byte[] bytes = Convert.FromBase64String(encodedText);
                decoded = SafeDecode(bytes, encoding);
            }
            else
            {
                // Q-encoding decode (replace =XX with actual character)
                decoded = Regex.Replace(encodedText, @"=([0-9A-F]{2})", m =>
                {
                    byte b = Convert.ToByte(m.Groups[1].Value, 16);
                    return SafeDecode(new[] { b }, encoding);
                });
                decoded = decoded.Replace("_", " ");
            }

            return decoded;
        });
    }

    /// <summary>
    /// Tries decoding bytes using the provided encoding, then UTF8 as fallback.
    /// Prevents weird "�" characters when server sends mixed encodings.
    /// </summary>
    private static string SafeDecode(byte[] bytes, Encoding primaryEncoding)
    {
        try
        {
            return primaryEncoding.GetString(bytes);
        }
        catch
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
