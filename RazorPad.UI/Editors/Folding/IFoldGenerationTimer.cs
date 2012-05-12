// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace RazorPad.UI.Editors.Folding
{
    public interface IFoldGenerationTimer : IDisposable
    {
        event EventHandler Tick;
        void Start();
    }
}