// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp
{
    internal class QueryConclusionUnboundLambdaState : UnboundLambdaState
    {
        private readonly QueryConclusionSyntax _syntax;
        private readonly QueryConclusionVariableSymbol _variableSymbol;
        private readonly TypeSymbol _variableTypeSymbol;

        public QueryConclusionUnboundLambdaState(
            QueryConclusionSyntax syntax,
            QueryConclusionVariableSymbol variableSymbol,
            TypeSymbol variableTypeSymbol,
            Binder binder, 
            UnboundLambda unboundLambdaOpt) 
            : base(binder, unboundLambdaOpt)
        {
            _syntax = syntax ?? throw new ArgumentNullException(nameof(syntax));
            _variableSymbol = variableSymbol ?? throw new ArgumentNullException(nameof(variableSymbol));
            _variableTypeSymbol = variableTypeSymbol ?? throw new ArgumentNullException(nameof(variableTypeSymbol));
        }

        public override MessageID MessageID => MessageID.IDS_FeatureQueryConclusions;

        public override bool HasSignature => true;

        public override bool HasExplicitlyTypedParameterList => false;

        public override int ParameterCount => 1;

        public override bool IsAsync => false;

        public override Location ParameterLocation(int index) => ValidateParameterIndex(index, _syntax.Identifier).GetLocation();

        public override string ParameterName(int index) => ValidateParameterIndex(index, _syntax.Identifier).Text;

        public override TypeSymbol ParameterType(int index) => ValidateParameterIndex(index, _variableTypeSymbol);

        public override RefKind RefKind(int index) => ValidateParameterIndex(index, CodeAnalysis.RefKind.None);

        protected override BoundBlock BindLambdaBody(LambdaSymbol lambdaSymbol, Binder lambdaBodyBinder, DiagnosticBag diagnostics)
        {
            return lambdaBodyBinder.BindLambdaExpressionAsBlock(_syntax.Expression, diagnostics);
        }

        public override Binder ParameterBinder(LambdaSymbol lambdaSymbol, Binder binder)
        {
            return new WithQueryConclusionParametersBinder(_variableSymbol, _variableTypeSymbol, lambdaSymbol, binder);
        }

        private TResult ValidateParameterIndex<TResult>(int index, TResult result)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return result;
        }
    }
}
