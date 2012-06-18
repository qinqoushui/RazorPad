// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

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
                if (IsMultiLineBlock(syntaxTreeNode))
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
        }

        private bool IsMultiLineBlock(SyntaxTreeNode node)
        {
            var block = node as Block;

            // will be true for markup spans as well, but they are handled elsewhere
            if (!node.IsBlock || block == null || !block.Children.Any()) return false;

            return block.Children.First().Start.LineIndex != block.Children.Last().Start.LineIndex;
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
}