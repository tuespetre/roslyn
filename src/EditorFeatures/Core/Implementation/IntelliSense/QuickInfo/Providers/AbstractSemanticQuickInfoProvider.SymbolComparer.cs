// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Shared.Utilities;

namespace Microsoft.CodeAnalysis.Editor.Implementation.IntelliSense.QuickInfo
{
    internal abstract partial class AbstractSemanticQuickInfoProvider
    {
        private class SymbolComparer : IEqualityComparer<ISymbol>
        {
            public static readonly SymbolComparer Instance = new SymbolComparer();

            private SymbolComparer()
            {
            }

            public bool Equals(ISymbol x, ISymbol y)
            {
                if (x is ILabelSymbol || x is ILocalSymbol || x is IRangeVariableSymbol || x is IQueryConclusionVariableSymbol)
                {
                    return object.ReferenceEquals(x, y);
                }

                return SymbolEquivalenceComparer.Instance.Equals(x, y);
            }

            public int GetHashCode(ISymbol obj)
            {
                if (obj is ILabelSymbol || obj is ILocalSymbol || obj is IRangeVariableSymbol || obj is IQueryConclusionVariableSymbol)
                {
                    return obj.GetHashCode();
                }

                return SymbolEquivalenceComparer.Instance.GetHashCode(obj);
            }
        }
    }
}
