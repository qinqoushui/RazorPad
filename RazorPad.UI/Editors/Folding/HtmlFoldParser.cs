// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Razor;
using System.Web.Razor.Parser;
using System.Web.Razor.Parser.SyntaxTree;
using ICSharpCode.AvalonEdit.Folding;

namespace RazorPad.UI.Editors.Folding
{
    public class HtmlFoldParser : IFoldParser
    {
        List<NewFolding> folds = new List<NewFolding>();
        Stack<HtmlElementFold> foldStack = new Stack<HtmlElementFold>();
        HtmlReader htmlReader;
        IHtmlReaderFactory htmlReaderFactory;

        public HtmlFoldParser(IHtmlReaderFactory htmlReaderFactory)
        {
            this.htmlReaderFactory = htmlReaderFactory;
        }

        public IEnumerable<NewFolding> GetFolds(string html)
        {
            ClearPreviousFolds();
            htmlReader = CreateHtmlReader(html);
            while (htmlReader.Read())
            {
                if (htmlReader.IsEmptyElement)
                {
                    // No folds for empty elements.
                }
                else if (htmlReader.IsEndElement)
                {
                    AddFoldForCompletedElement();
                }
                else
                {
                    SaveFoldStartOnStack();
                }
            }

            GetRazorFolds(html);
            SortFoldsByStartOffset();
            return folds;
        }

        private void GetRazorFolds(string markup)
        {
            var razorEngineHost = new RazorEngineHost(RazorCodeLanguage.GetLanguageByExtension("cshtml"));
            var engine = new RazorTemplateEngine(razorEngineHost);
            var results = engine.ParseTemplate(new StringReader(markup));
            //var parser = new RazorParser(new CSharpCodeParser(), new HtmlMarkupParser());
            //var results = parser.Parse(new StringReader(markup));
            SaveRazorFoldsStartOnStack(results.Document.Children.Where(c => c.IsBlock));
        }

        private void SaveRazorFoldsStartOnStack(IEnumerable<SyntaxTreeNode> nodes)
        {
            foreach (var syntaxTreeNode in nodes)
            {
                var fold = new RazorElementFold()
                {
                    ElementName = GetBlockName(syntaxTreeNode as Block),
                    StartOffset = syntaxTreeNode.Start.AbsoluteIndex,
                    Line = syntaxTreeNode.Start.LineIndex,
                    EndOffset = syntaxTreeNode.Start.AbsoluteIndex + syntaxTreeNode.Length
                };
                folds.Add(fold);
            }
        }

        private string GetBlockName(Block block)
        {
            const string defaultName = "...";
            switch (block.Type)
            {
                case BlockType.Statement:
                    return "@{...}";
                case BlockType.Directive:
                    break;
                case BlockType.Functions:
                    return "@funtions";
                case BlockType.Expression:
                    break;
                case BlockType.Helper:
                    var headerName = "";
                    var helperHeader = block.Children.FirstOrDefault(c => c.GetType() == typeof(HelperHeaderSpan)) as HelperHeaderSpan;
                    if (helperHeader != null)
                    {
                        headerName = helperHeader.Content.Substring(0, helperHeader.Content.IndexOf("(", StringComparison.Ordinal)).Trim();
                    }
                    return string.Format("@helper {0}", headerName);
                case BlockType.Markup:
                    break;
                case BlockType.Section:
                    var sectionName = "";
                    if (block.Children != null)
                    {
                        var sectionHeader =
                            block.Children.FirstOrDefault(c => c.GetType() == typeof(SectionHeaderSpan)) as SectionHeaderSpan;
                        sectionName = sectionHeader != null ? sectionHeader.SectionName : "";
                    }
                    return string.Format("@section {0}", sectionName);
                case BlockType.Template:
                    break;
                case BlockType.Comment:
                    break;
                default:
                    return defaultName;
            }
            return defaultName;
        }


        void ClearPreviousFolds()
        {
            folds.Clear();
        }

        HtmlReader CreateHtmlReader(string html)
        {
            return htmlReaderFactory.CreateHtmlReader(html);
        }

        void SaveFoldStartOnStack()
        {
            var fold = new HtmlElementFold()
                           {
                               ElementName = htmlReader.Value,
                               StartOffset = htmlReader.Offset,
                               Line = htmlReader.Line
                           };
            foldStack.Push(fold);
        }

        void AddFoldForCompletedElement()
        {
            if (foldStack.Any())
            {
                var fold = foldStack.Pop();
                if (fold.ElementName == htmlReader.Value)
                {
                    fold.EndOffset = htmlReader.EndOffset;
                    AddFoldIfEndElementOnDifferentLineToStartElement(fold);
                }
                else
                {
                    AddFoldForCompletedElement();
                }
            }
        }

        void AddFoldIfEndElementOnDifferentLineToStartElement(HtmlElementFold fold)
        {
            if (htmlReader.Line > fold.Line)
            {
                folds.Add(fold);
            }
        }

        void SortFoldsByStartOffset()
        {
            folds.Sort((fold1, fold2) => fold1.StartOffset.CompareTo(fold2.StartOffset));
        }
    }
}