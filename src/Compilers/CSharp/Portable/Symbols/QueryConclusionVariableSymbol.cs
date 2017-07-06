// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp.Symbols
{
    /// <summary>
    /// A QueryConclusionVariableSymbol represents an identifier introduced in a query expression as the
    /// identifier of a "yield into" query conclusion.
    /// </summary>
    internal class QueryConclusionVariableSymbol : Symbol, IQueryConclusionVariableSymbol
    {
        private readonly string _name;
        private readonly ImmutableArray<Location> _locations;
        private readonly Symbol _containingSymbol;

        internal QueryConclusionVariableSymbol(string name, Symbol containingSymbol, Location location)
        {
            _name = name;
            _containingSymbol = containingSymbol;
            _locations = ImmutableArray.Create<Location>(location);
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public override SymbolKind Kind
        {
            get
            {
                return SymbolKind.QueryConclusionVariable;
            }
        }

        public override ImmutableArray<Location> Locations
        {
            get
            {
                return _locations;
            }
        }

        public override ImmutableArray<SyntaxReference> DeclaringSyntaxReferences
        {
            get
            {
                SyntaxToken token = (SyntaxToken)_locations[0].SourceTree.GetRoot().FindToken(_locations[0].SourceSpan.Start);
                Debug.Assert(token.Kind() == SyntaxKind.IdentifierToken);
                CSharpSyntaxNode node = (CSharpSyntaxNode)token.Parent;
                Debug.Assert(node is QueryConclusionSyntax);
                return ImmutableArray.Create<SyntaxReference>(node.GetReference());
            }
        }

        public override bool IsExtern
        {
            get
            {
                return false;
            }
        }

        public override bool IsSealed
        {
            get
            {
                return false;
            }
        }

        public override bool IsAbstract
        {
            get
            {
                return false;
            }
        }

        public override bool IsOverride
        {
            get
            {
                return false;
            }
        }

        public override bool IsVirtual
        {
            get
            {
                return false;
            }
        }

        public override bool IsStatic
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns data decoded from Obsolete attribute or null if there is no Obsolete attribute.
        /// This property returns ObsoleteAttributeData.Uninitialized if attribute arguments haven't been decoded yet.
        /// </summary>
        internal sealed override ObsoleteAttributeData ObsoleteAttributeData
        {
            get { return null; }
        }

        public override Accessibility DeclaredAccessibility
        {
            get
            {
                return Accessibility.NotApplicable;
            }
        }

        public override Symbol ContainingSymbol
        {
            get
            {
                return _containingSymbol;
            }
        }

        public override void Accept(SymbolVisitor visitor)
        {
            visitor.VisitQueryConclusionVariable(this);
        }

        public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            return visitor.VisitQueryConclusionVariable(this);
        }

        internal override TResult Accept<TArg, TResult>(CSharpSymbolVisitor<TArg, TResult> visitor, TArg a)
        {
            return visitor.VisitQueryConclusionVariable(this, a);
        }

        public override void Accept(CSharpSymbolVisitor visitor)
        {
            visitor.VisitQueryConclusionVariable(this);
        }

        public override TResult Accept<TResult>(CSharpSymbolVisitor<TResult> visitor)
        {
            return visitor.VisitQueryConclusionVariable(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == (object)this)
            {
                return true;
            }

            var symbol = obj as QueryConclusionVariableSymbol;
            return (object)symbol != null
                && symbol._locations[0].Equals(_locations[0])
                && Equals(_containingSymbol, symbol.ContainingSymbol);
        }

        public override int GetHashCode()
        {
            return Hash.Combine(_locations[0].GetHashCode(), _containingSymbol.GetHashCode());
        }
    }
}
