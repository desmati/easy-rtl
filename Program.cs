using System;
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
                        if (file.EndsWith("main.css"))
                        {
                            output = MakeCssRTL(System.IO.File.ReadAllLines(file));
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
            foreach (string s in css.Split('\n'))
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
    }
}