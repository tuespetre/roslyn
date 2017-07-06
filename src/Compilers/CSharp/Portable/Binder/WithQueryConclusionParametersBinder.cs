// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.CSharp
{
    internal class WithQueryConclusionParametersBinder : WithLambdaParametersBinder
    {
        private QueryConclusionVariableSymbol _variableSymbol;
        private TypeSymbol _variableTypeSymbol;

        public WithQueryConclusionParametersBinder(
            QueryConclusionVariableSymbol variableSymbol,
            TypeSymbol variableTypeSymbol,
            LambdaSymbol lambdaSymbol, 
            Binder binder)
            : base(lambdaSymbol, binder)
        {
            _variableSymbol = variableSymbol ?? throw new ArgumentNullException(nameof(variableSymbol));
            _variableTypeSymbol = variableTypeSymbol ?? throw new ArgumentNullException(nameof(variableTypeSymbol));
        }

        protected override BoundExpression BindQueryConclusionVariable(SimpleNameSyntax node, QueryConclusionVariableSymbol qv, DiagnosticBag diagnostics)
        {
            if (qv.Equals(_variableSymbol))
            {
                var boundParameter 
                    = new BoundParameter(node, parameterMap[qv.Name].Single())
                        { WasCompilerGenerated = true };

                return new BoundQueryConclusionVariable(node, _variableSymbol, boundParameter, _variableTypeSymbol);
            }

            return base.BindQueryConclusionVariable(node, qv, diagnostics);
        }

        internal override void LookupSymbolsInSingleBinder(
            LookupResult result, string name, int arity, ConsList<Symbol> basesBeingResolved, LookupOptions options, Binder originalBinder, bool diagnose, ref HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            Debug.Assert(result.IsClear);

            if ((options & LookupOptions.NamespaceAliasesOnly) != 0)
            {
                return;
            }

            if (name.Equals(_variableSymbol.Name))
            {
                result.MergeEqual(originalBinder.CheckViability(_variableSymbol, arity, options, null, diagnose, ref useSiteDiagnostics));
            }
        }

        protected override void AddLookupSymbolsInfoInSingleBinder(LookupSymbolsInfo result, LookupOptions options, Binder originalBinder)
        {
            if (options.CanConsiderMembers())
            {
                result.AddSymbol(null, _variableSymbol.Name, 0);
            }
        }
    }
}
