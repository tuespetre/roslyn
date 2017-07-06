// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Extensions.ContextQuery;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.Completion.KeywordRecommenders
{
    internal class DoKeywordRecommender : AbstractSyntacticSingleKeywordRecommender
    {
        public DoKeywordRecommender()
            : base(SyntaxKind.DoKeyword)
        {
        }

        protected override bool IsValidContext(int position, CSharpSyntaxContext context, CancellationToken cancellationToken)
        {
            return
                context.IsStatementContext ||
                context.IsGlobalStatementContext ||
                IsValidContextForQueryConclusion(context);
        }

        private static bool IsValidContextForQueryConclusion(CSharpSyntaxContext context)
        {
            return context.TargetToken.Parent is QueryConclusionSyntax queryConclusionSyntax
                && !queryConclusionSyntax.Identifier.IsMissing
                && !queryConclusionSyntax.Identifier.IntersectsWith(context.Position);
        }
    }
}
