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
        RazorHtmlReader razorHtmlReader;
        IRazorHtmlReaderFactory htmlReaderFactory;

        public HtmlFoldParser(IRazorHtmlReaderFactory htmlReaderFactory)
        {
            this.htmlReaderFactory = htmlReaderFactory;
        }

        public IEnumerable<NewFolding> GetFolds(string html)
        {
            ClearPreviousFolds();

            razorHtmlReader = CreateHtmlReader(html);

            GetHtmlFolds();
            GetRazorFolds();

            SortFoldsByStartOffset();
            return folds;
        }

        private void GetHtmlFolds()
        {
            while (razorHtmlReader.Read())
            {
                if (razorHtmlReader.IsEmptyElement)
                {
                    // No folds for empty elements.
                }
                else if (razorHtmlReader.IsEndElement)
                {
                    AddFoldForCompletedElement();
                }
                else
                {
                    SaveFoldStartOnStack();
                }
            }
        }

        private void GetRazorFolds()
        {
            SaveRazorFoldsStartOnStack(razorHtmlReader.CodeSpans);
        }
        
        void ClearPreviousFolds()
        {
            folds.Clear();
        }

        RazorHtmlReader CreateHtmlReader(string html)
        {
           return htmlReaderFactory.CreateHtmlReader(html);
        }

        void SaveFoldStartOnStack()
        {
            var fold = new HtmlElementFold()
                           {
                               ElementName = razorHtmlReader.Value,
                               StartOffset = razorHtmlReader.Offset,
                               Line = razorHtmlReader.Line
                           };
            foldStack.Push(fold);
        }

        private void SaveRazorFoldsStartOnStack(IEnumerable<SyntaxTreeNode> nodes)
        {
            foreach (var syntaxTreeNode in nodes)
            {
                folds.Add(new RazorElementFold
                {
                    ElementName = RazorCodeSpanParser.GetBlockName(syntaxTreeNode as Block),
                    StartOffset = syntaxTreeNode.Start.AbsoluteIndex,
                    Line = syntaxTreeNode.Start.LineIndex,
                    EndOffset = syntaxTreeNode.Start.AbsoluteIndex + syntaxTreeNode.Length
                });
            }
        }
        
        void AddFoldForCompletedElement()
        {
            if (foldStack.Any())
            {
                var fold = foldStack.Pop();
                if (fold.ElementName == razorHtmlReader.Value)
                {
                    fold.EndOffset = razorHtmlReader.EndOffset;
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
            if (razorHtmlReader.Line > fold.Line)
            {
                folds.Add(fold);
            }
        }

        void SortFoldsByStartOffset()
        {
            folds.Sort((fold1, fold2) => fold1.StartOffset.CompareTo(fold2.StartOffset));
        }
    }

    public class RazorCodeSpanParser
    {
        public static string GetBlockName(Block block)
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
                    return GetHelperBlockName(block);
                case BlockType.Markup:
                    break;
                case BlockType.Section:
                    return GetSectionBlockName(block);
                case BlockType.Template:
                    break;
                case BlockType.Comment:
                    return GetCommentBlockName(block);
                default:
                    return defaultName;
            }
            return defaultName;
        }

        private static string GetCommentBlockName(Block block)
        {
            return "/* ... */";
        }

        private static string GetSectionBlockName(Block block)
        {
            var sectionName = "";
            if (block.Children != null)
            {
                var sectionHeader =
                    block.Children.FirstOrDefault(c => c.GetType() == typeof (SectionHeaderSpan)) as SectionHeaderSpan;
                sectionName = sectionHeader != null ? sectionHeader.SectionName : "";
            }
            return string.Format("@section {0}", sectionName);
        }

        private static string GetHelperBlockName(Block block)
        {
            var headerName = "";
            var helperHeader = block.Children.FirstOrDefault(c => c.GetType() == typeof (HelperHeaderSpan)) as HelperHeaderSpan;
            if (helperHeader != null)
            {
                headerName =
                    helperHeader.Content.Substring(0, helperHeader.Content.IndexOf("(", StringComparison.Ordinal)).Trim();
            }
            return string.Format("@helper {0}", headerName);
        }
    }
}