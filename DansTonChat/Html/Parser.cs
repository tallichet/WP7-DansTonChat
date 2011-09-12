using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace DansTonChat.Html
{
    public class Parser
    {

        public Content ParseHtml(string html)
        {
            Content result = new Content();

            StringReader reader = new StringReader(html);
            StringWriter writer = new StringWriter();

            string baliseTitle = "";

            ContentParserState state = ContentParserState.Normal;

            StringWriter currentItem = null;
            while (reader.Peek() > -1)
            {
                char c = Convert.ToChar(reader.Read());
                switch (state)
                {
                    case ContentParserState.Normal:
                        if (c == '<')
                        {
                            state = ContentParserState.BaliseTitle;
                            currentItem = new StringWriter();
                        }
                        else if (c == '&')
                        {
                            state = ContentParserState.SpecialChar;
                            currentItem = new StringWriter();
                        }
                        else
                            writer.Write(c);
                        break;
                    case ContentParserState.BaliseTitle:
                        if (c == '>')
                        {
                            baliseTitle = currentItem.ToString();
                            state = ContentParserState.Normal;                            
                        }
                        if (c == ' ')
                        {
                            baliseTitle = currentItem.ToString();
                            if (currentItem.ToString().Equals("img", StringComparison.OrdinalIgnoreCase))
                                state = ContentParserState.BaliseImg;
                            else
                                state = ContentParserState.Balise;
                            currentItem = new StringWriter();
                        }
                        else
                            currentItem.Write(c);
                        break;
                    case ContentParserState.Balise:
                        if (c == '>')
                            state = ContentParserState.Normal; // BaliseContent;
                        else if (c == '/')
                            state = ContentParserState.BaliseEnding;
                        break;
                    case ContentParserState.BaliseEnding:
                        if (c == '>')
                        {
                            state = ContentParserState.Normal;
                            //if (baliseTitle == "br")
                            //    writer.WriteLine();
                        }
                        else
                            state = ContentParserState.Balise;
                        break;
                    case ContentParserState.BaliseContent:
                        if (c == '<')
                            state = ContentParserState.Balise;
                        break;
                    case ContentParserState.BaliseImg:
                        if (c == ' ')
                            currentItem = new StringWriter();
                        else if (c == '=')
                        {
                            if (currentItem.ToString().Equals("src", StringComparison.OrdinalIgnoreCase))
                            {
                                state = ContentParserState.BaliseImgSrc;
                                currentItem = new StringWriter();
                            }
                        }
                        else
                            currentItem.Write(c);
                        break;
                    case ContentParserState.BaliseImgSrc:
                        if (c == ' ' || c == '>')
                        {
                            string imgsrc = currentItem.ToString();
                            if (imgsrc[0] == '\'' || imgsrc[0] == '"')
                                imgsrc = imgsrc.Substring(1, imgsrc.Length - 2);
                            result.Images.Add(imgsrc);
                            state = ContentParserState.Balise;
                            currentItem = new StringWriter();
                        }
                        else
                            currentItem.Write(c);
                        break;
                    case ContentParserState.SpecialChar:
                        if (c == ';')
                        {
                            switch (currentItem.ToString())
                            {
                                case "nbsp":
                                    writer.Write(" ");
                                    break;
                                case "amp":
                                    writer.Write("&");
                                    break;
                                case "quot":
                                    writer.Write("\"");
                                    break;
                                case "lt":
                                    writer.Write("<");
                                    break;
                                case "gt":
                                    writer.Write(">");
                                    break;

                                case "#8217":
                                    writer.Write("’");
                                    break;
                                case "#8230":
                                    writer.Write("…");
                                    break;
                                case "#8220":
                                    writer.Write("“");
                                    break;
                                case "#8221":
                                    writer.Write("”");
                                    break;
                                case "#8243":
                                    writer.Write("″");
                                    break;
                                case "#215":
                                    writer.Write("×");
                                    break;
                                default:
                                    Debug.WriteLine("unknown: &" + currentItem.ToString() + ";");
                                    break;
                            }
                            state = ContentParserState.Normal;
                        }
                        else
                            currentItem.Write(c);
                        break;
                }
            }
            result.Text = writer.ToString();

            return result;
        }

        protected enum ContentParserState
        {
            Normal,
            BaliseTitle,
            Balise,
            BaliseEnding,
            BaliseContent,
            BaliseImg,
            BaliseImgSrc,
            SpecialChar
        }
    }
}
