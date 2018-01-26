using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyRTL
{
    class Program
    {
        static void Main(string[] args)
        {
            var From = args.FirstOrDefault(x => x.ToLower().StartsWith("--from")).Split('=')[1].Replace("\"", "");
            var To = args.FirstOrDefault(x => x.ToLower().StartsWith("--to")).Split('=')[1].Replace("\"", "");
            Console.WriteLine("From: " + From);
            Console.WriteLine("To: " + To);
            System.IO.Directory.CreateDirectory(To);
            var filesList = System.IO.Directory.GetFiles(From, "*.*", System.IO.SearchOption.AllDirectories).ToList();
            //filesList.AddRange(System.IO.Directory.GetFiles(From, "*.html", System.IO.SearchOption.AllDirectories));
            var cssCount = 0;
            var htmlCount = 0;
            var jsCount = 0;
            foreach (var file in filesList)
            {
                var output = "";
                var fileType = System.IO.Path.GetExtension(file);
                var filePath = System.IO.Path.GetDirectoryName(file);
                Console.WriteLine(fileType + ", " + filePath);
                var converted = false;
                switch (fileType)
                {
                    case ".css":
                        if (file.EndsWith(".css"))
                        {
                            output = MakeCssRTL(CssBeautify(System.IO.File.ReadAllText(file)).Split("\n"));
                            cssCount++;
                            converted = true;
                        }
                        break;
                        //case ".html":
                        //    var fileContent = System.IO.File.ReadAllText(file);
                        //    fileContent = fileContent.Replace("left", "left_kbjaf6532tubhf76248hf82gvcsbjCTGVdcyVCGfct65467");
                        //    fileContent = fileContent.Replace("right", "left");
                        //    fileContent = fileContent.Replace("left_kbjaf6532tubhf76248hf82gvcsbjCTGVdcyVCGfct65467", "right");
                        //    fileContent = fileContent.Replace("<html>", "<html dir=\"rtl\">");
                        //    output = fileContent;
                        //    htmlCount++;
                        //    converted = true;
                        //    break;
                        //case ".js":
                        //    var jsContent = System.IO.File.ReadAllText(file);
                        //    jsContent = jsContent.Replace("left", "left_kbjaf6532tubhf76248hf82gvcsbjCTGVdcyVCGfct65467");
                        //    jsContent = jsContent.Replace("right", "left");
                        //    jsContent = jsContent.Replace("left_kbjaf6532tubhf76248hf82gvcsbjCTGVdcyVCGfct65467", "right");
                        //    output = jsContent;
                        //    jsCount++;
                        //    converted = true;
                        //    break;
                }
                var outputPath = (To + file.ToLower().Replace(From.ToLower(), "")).Replace(@"\\", @"\");
                Console.WriteLine("Output: " + outputPath);
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
                if (converted) System.IO.File.WriteAllText(outputPath, output); else System.IO.File.Copy(file, outputPath, true);
            }
            Console.WriteLine("CSS Files Converted: " + cssCount);
            Console.WriteLine("HTML Files Converted: " + htmlCount);
            Console.WriteLine("JS Files Converted: " + jsCount);
            Console.ReadKey();
        }

        public static string MakeCssRTL(string[] inCss)
        {
            var preparedCss = new System.Text.StringBuilder();
            foreach (var line in inCss)
            {
                var l = line.Trim();
                l = l.Replace("\t", " ");
                while (l.Contains("  ")) l = l.Replace("  ", " ");
                while (l.Contains(": ")) l = l.Replace(": ", ":");
                while (l.Contains(" :")) l = l.Replace(" :", ":");
                while (l.Contains("> ")) l = l.Replace("> ", ">");
                while (l.Contains(" >")) l = l.Replace(" >", ">");
                while (l.Contains(" ;")) l = l.Replace(" ;", ";");
                while (l.Contains("  ")) l = l.Replace("  ", " ");
                preparedCss.Append(l + Environment.NewLine);
            }
            var css = preparedCss.ToString();


            //left-right
            css = css.Replace("left", "lr_jbdsghsasahd62hsdks");
            css = css.Replace("right", "left");
            css = css.Replace("lr_jbdsghsasahd62hsdks", "right");

            //backward-forward
            css = css.Replace("backward", "bf_jbdsghsasahd62hsdks");
            css = css.Replace("forward", "backward");
            css = css.Replace("bf_jbdsghsasahd62hsdks", "forward");

            //.pager .next-.pager .previous
            css = css.Replace(".pager .next", "pnpp_jbdsghsasahd62hsdks");
            css = css.Replace(".pager .previous", ".pager .next");
            css = css.Replace("pnpp_jbdsghsasahd62hsdks", ".pager .previous");

            //.icon-next-.icon-prev
            css = css.Replace(".icon-next", "inip_jbdsghsasahd62hsdks");
            css = css.Replace(".icon-prev", ".icon-next");
            css = css.Replace("inip_jbdsghsasahd62hsdks", ".icon-prev");

            //.carousel-inner > .prev-.carousel-inner > .next
            css = css.Replace(".carousel-inner>.prev", "cipcin_jbdsghsasahd62hsdks");
            css = css.Replace(".carousel-inner>.next", ".carousel-inner > .prev");
            css = css.Replace("cipcin_jbdsghsasahd62hsdks", ".carousel-inner > .next");
            var i = 0;
            var o = new System.Text.StringBuilder();
            foreach (string s in css.Split("\n"))
            {
                i++;
                try
                {

                    string l = s.ToLower().Trim();
                    string l2 = s.Trim();
                    bool hasmp = false;
                    string mp = "";
                    if (l.Contains("margin:"))
                    {
                        mp = "margin";
                        hasmp = true;
                    }
                    if (l.Contains("padding:"))
                    {
                        mp = "padding";
                        hasmp = true;
                    }
                    if (hasmp)
                    {
                        string v = (l.Split(':')[1]).Replace(";", "").ToLower().Trim();
                        string top = "";
                        string right = "";
                        string bottom = "";
                        string left = "";
                        string[] p = v.Split(' ');
                        if (p.Length == 1)
                        {
                            top = p[0];
                            right = p[0];
                            bottom = p[0];
                            left = p[0];
                        }
                        if (p.Length == 2)
                        {
                            top = p[0];
                            right = p[1];
                            bottom = p[0];
                            left = p[1];
                        }
                        if (p.Length == 3)
                        {
                            top = p[0];
                            right = p[1];
                            bottom = p[2];
                            left = p[1];
                        }
                        if (p.Length == 4)
                        {
                            top = p[0];
                            right = p[3];
                            bottom = p[2];
                            left = p[1];
                        }
                        l2 = " " + mp + ":" + top + " " + right + " " + bottom + " " + left + ";";
                    }
                    o.Append(l2 + Environment.NewLine);
                }
                catch
                {

                }
            }
            var outstr = o.ToString()
                .Replace("font-family:sans-serif;", "font-family:'desmati','Tahoma';")
                .Replace("\"helvetica neue\"", "'desmati',Tahoma");
            //
            return outstr;
        }

        public static string CssBeautify(string style)
        {

            var index = 0;
            var blocks = new List<string>();
            var formatted = "";
            string ch;
            string ch2;
            var str = "";
            var state = State.Start;
            var depth = 0;
            string quote = "";
            var comment = false;
            var options = new
            {
                indent = "    ",
                openbrace = "end-of-line",
                autosemicolon = true,

            };
            var openbracesuffix = options.openbrace == "end-of-line";
            var autosemicolon = options.autosemicolon;

            depth = 0;
            state = State.Start;

            // We want to deal with LF (\n) only
            style = style.Replace("\r\n", "\n");
            var length = style.Length;

            while (index < length - 1)
            {
                ch = style.Substring(index, 1);
                ch2 = style.Substring(index + 1, 1);
                index += 1;
                // Inside a string literal?
                if (isQuote(quote))
                {
                    formatted += ch;
                    if (ch == quote)
                        quote = null;
                    if (ch == "\\" && ch2 == quote)
                    {
                        // Don't treat escaped character as the closing quote
                        formatted += ch2;
                        index += 1;
                    }
                    continue;
                }

                // Starting a string literal?
                if (isQuote(ch))
                {
                    formatted += ch;
                    quote = ch;
                    continue;
                }

                // Comment
                if (comment)
                {
                    formatted += ch;
                    if (ch == "*" && ch2 == "/")
                    {
                        comment = false;
                        formatted += ch2;
                        index += 1;
                    }
                    continue;
                }
                if (ch == "/" && ch2 == "*")
                {
                    comment = true;
                    formatted += ch;
                    formatted += ch2;
                    index += 1;
                    continue;
                }

                if (state == State.Start)
                {

                    if (blocks.Count() == 0)
                    {
                        if (isWhitespace(ch) && formatted.Length == 0)
                        {
                            continue;
                        }
                    }

                    // Copy white spaces and control characters
                    if (ch.ToCharArray()[0] <= ' ' || ch.ToCharArray()[0] >= 128)
                    {
                        state = State.Start;
                        formatted += ch;
                        continue;
                    }

                    // Selector or at-rule
                    if (isName(ch) || (ch == "@"))
                    {

                        // Clear trailing whitespaces and linefeeds.
                        str = formatted.TrimEnd();

                        if (str.Length == 0)
                        {
                            // If we have empty string after removing all the trailing
                            // spaces, that means we are right after a block.
                            // Ensure a blank line as the separator.
                            if (blocks.Count() > 0)
                            {
                                formatted = "\n\n";
                            }
                        }
                        else
                        {
                            // After finishing a ruleset or directive statement,
                            // there should be one blank line.
                            if (str.ToCharArray()[str.Length - 1] == '}' ||
                                    str.ToCharArray()[str.Length - 1] == ';')
                            {

                                formatted = str + "\n\n";
                            }
                            else
                            {
                                // After block comment, keep all the linefeeds but
                                // start from the first column (remove whitespaces prefix).
                                while (true)
                                {
                                    ch2 = formatted.Substring(formatted.Length - 1, 1);
                                    if (ch2 != " " && ch2.ToCharArray()[0] != 9)
                                        break;
                                    formatted = formatted.Substring(0, formatted.Length - 1);
                                }
                            }
                        }
                        formatted += ch;
                        state = (ch == "@") ? State.AtRule : State.Selector;
                        continue;
                    }
                }

                if (state == State.AtRule)
                {

                    // ';' terminates a statement.
                    if (ch == ";")
                    {
                        formatted += ch;
                        state = State.Start;
                        continue;
                    }

                    // '{' starts a block
                    if (ch == "{")
                    {
                        str = formatted.TrimEnd();
                        openBlock(ref formatted, ch2, openbracesuffix, ref depth, options.indent);
                        state = (str == "@font-face") ? State.Ruleset : State.Block;
                        continue;
                    }

                    formatted += ch;
                    continue;
                }

                if (state == State.Block)
                {

                    // Selector
                    if (isName(ch))
                    {

                        // Clear trailing whitespaces and linefeeds.
                        str = formatted.TrimEnd();

                        if (str.Length == 0)
                        {
                            // If we have empty string after removing all the trailing
                            // spaces, that means we are right after a block.
                            // Ensure a blank line as the separator.
                            if (blocks.Count() > 0)
                            {
                                formatted = "\n\n";
                            }
                        }
                        else
                        {
                            // Insert blank line if necessary.
                            if (str.Substring(str.Length - 1, 1) == "}")
                            {
                                formatted = str + "\n\n";
                            }
                            else
                            {
                                // After block comment, keep all the linefeeds but
                                // start from the first column (remove whitespaces prefix).
                                while (true)
                                {
                                    ch2 = formatted.Substring(formatted.Length - 1, 1);
                                    if (ch2 != " " && ch2.ToCharArray()[0] != 9) break;
                                    formatted = formatted.Substring(0, formatted.Length - 1);
                                }
                            }
                        }

                        appendIndent(ref formatted, depth, options.indent);
                        formatted += ch;
                        state = State.Selector;
                        continue;
                    }

                    // "}" resets the state.
                    if (ch == "}")
                    {
                        closeBlock(ref formatted, ref depth, autosemicolon, options.indent, ref blocks);
                        state = State.Start;
                        continue;
                    }

                    formatted += ch;
                    continue;
                }

                if (state == State.Selector)
                {

                    // '{' starts the ruleset.
                    if (ch == "{")
                    {
                        openBlock(ref formatted, ch2, openbracesuffix, ref depth, options.indent);
                        state = State.Ruleset;
                        continue;
                    }

                    // "}" resets the state.
                    if (ch == "}")
                    {
                        closeBlock(ref formatted, ref depth, autosemicolon, options.indent, ref blocks);
                        state = State.Start;
                        continue;
                    }

                    formatted += ch;
                    continue;
                }

                if (state == State.Ruleset)
                {

                    // "}" finishes the ruleset.
                    if (ch == "}")
                    {
                        closeBlock(ref formatted, ref depth, autosemicolon, options.indent, ref blocks);
                        state = State.Start;
                        if (depth > 0)
                        {
                            state = State.Block;
                        }
                        continue;
                    }

                    // Make sure there is no blank line or trailing spaces inbetween
                    if (ch == "\n")
                    {
                        formatted = formatted.TrimEnd();
                        formatted += "\n";
                        continue;
                    }

                    // property name
                    if (!isWhitespace(ch))
                    {
                        formatted = formatted.TrimEnd();
                        formatted += "\n";
                        appendIndent(ref formatted, depth, options.indent);
                        formatted += ch;
                        state = State.Property;
                        continue;
                    }
                    formatted += ch;
                    continue;
                }

                if (state == State.Property)
                {

                    // ':' concludes the property.
                    if (ch == ":")
                    {
                        formatted = formatted.TrimEnd();
                        formatted += ": ";
                        state = State.Expression;
                        if (isWhitespace(ch2))
                        {
                            state = State.Separator;
                        }
                        continue;
                    }

                    // "}" finishes the ruleset.
                    if (ch == "}")
                    {
                        closeBlock(ref formatted, ref depth, autosemicolon, options.indent, ref blocks);
                        state = State.Start;
                        if (depth > 0)
                        {
                            state = State.Block;
                        }
                        continue;
                    }

                    formatted += ch;
                    continue;
                }

                if (state == State.Separator)
                {

                    // Non-whitespace starts the expression.
                    if (!isWhitespace(ch))
                    {
                        formatted += ch;
                        state = State.Expression;
                        continue;
                    }

                    // Anticipate string literal.
                    if (isQuote(ch2))
                    {
                        state = State.Expression;
                    }

                    continue;
                }

                if (state == State.Expression)
                {

                    // "}" finishes the ruleset.
                    if (ch == "}")
                    {
                        closeBlock(ref formatted, ref depth, autosemicolon, options.indent, ref blocks);
                        state = State.Start;
                        if (depth > 0)
                        {
                            state = State.Block;
                        }
                        continue;
                    }

                    // ';' completes the declaration.
                    if (ch == ";")
                    {
                        formatted = formatted.TrimEnd();
                        formatted += ";\n";
                        state = State.Ruleset;
                        continue;
                    }

                    formatted += ch;

                    if (ch == "(")
                    {
                        if (formatted.ToCharArray()[formatted.Length - 2] == 'l' &&
                                formatted.ToCharArray()[formatted.Length - 3] == 'r' &&
                                formatted.ToCharArray()[formatted.Length - 4] == 'u')
                        {
                            // URL starts with '(' and closes with ')'.
                            state = State.URL;
                            continue;
                        }
                    }

                    continue;
                }

                if (state == State.URL)
                {


                    // ')' finishes the URL (only if it is not escaped).
                    if (ch == ")" && formatted.ToCharArray()[formatted.Length - 1] != '\\')
                    {
                        formatted += ch;
                        state = State.Expression;
                        continue;
                    }
                }

                // The default action is to copy the character (to prevent
                // infinite loop).
                formatted += ch;
            }

            formatted = String.Join("", blocks) + formatted;

            return formatted;
        }
        public enum State
        {
            Start = 0,
            AtRule = 1,
            Block = 2,
            Selector = 3,
            Ruleset = 4,
            Property = 5,
            Separator = 6,
            Expression = 7,
            URL = 8
        };

        public static bool isWhitespace(string c) =>
            (c == " ") || (c == "\n") || (c == "\t") || (c == "\r") || (c == "\f");

        public static bool isQuote(string c) => (c == "\'") || (c == "\"");

        // FIXME: handle Unicode characters
        public static bool isName(string str)
        {
            var ch = str.ToCharArray()[0];
            return (ch >= 'a' && ch <= 'z') ||
                (ch >= 'A' && ch <= 'Z') ||
                (ch >= '0' && ch <= '9') ||
                ("-_*.:#[]").Contains(str);
        }

        public static void appendIndent(ref string formatted, int depth, string indent)
        {
            for (var i = depth; i > 0; i -= 1) formatted += indent;
        }

        public static void openBlock(ref string formatted, string ch2, bool openbracesuffix, ref int depth, string indent)
        {
            formatted = formatted.TrimEnd();
            if (openbracesuffix)
                formatted += " {";
            else
            {
                formatted += "\n";
                appendIndent(ref formatted, depth, indent);
                formatted += '{';
            }
            if (ch2 != "\n") formatted += "\n";
            depth += 1;
        }

        public static void closeBlock(ref string formatted, ref int depth, bool autosemicolon, string indent, ref List<string> blocks)
        {
            char last;
            depth -= 1;
            formatted = formatted.TrimEnd();
            if (formatted.Length > 0 && autosemicolon)
            {
                last = formatted.ToCharArray()[formatted.Length - 1];
                if (last != ';' && last != '{') formatted += ';';
            }
            formatted += "\n";
            appendIndent(ref formatted, depth, indent);
            formatted += "}";
            blocks.Add(formatted);
            formatted = "";
        }

    }
}