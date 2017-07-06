// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Extensions.ContextQuery;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Shared.Extensions;

namespace Microsoft.CodeAnalysis.CSharp.Completion.KeywordRecommenders
{
    internal class YieldKeywordRecommender : AbstractSyntacticSingleKeywordRecommender
    {
        public YieldKeywordRecommender()
            : base(SyntaxKind.YieldKeyword)
        {
        }

        protected override bool IsValidContext(int position, CSharpSyntaxContext context, CancellationToken cancellationToken)
        {
            return context.IsStatementContext
                || IsValidContextForSelect(context)
                || IsValidContextForGroup(context);
        }

        private bool IsValidContextForSelect(CSharpSyntaxContext context)
        {
            var token = context.TargetToken;

            var select = token.GetAncestor<SelectClauseSyntax>();
            if (select == null)
            {
                return false;
            }

            if (select.Expression.Width() == 0)
            {
                return false;
            }
            
            // cases:
            //   select x.|
            //   select x.i|
            var lastCompleteToken = token.GetPreviousTokenIfTouchingWord(context.Position);
            if (lastCompleteToken.Kind() == SyntaxKind.DotToken)
            {
                return false;
            }

            var lastToken = select.Expression.GetLastToken(includeSkipped: true);
            if (lastToken == token)
            {
                return true;
            }

            return false;
        }

        private bool IsValidContextForGroup(CSharpSyntaxContext context)
        {
            var token = context.TargetToken;

            var group = token.GetAncestor<GroupClauseSyntax>();
            if (group == null)
            {
                return false;
            }

            if (group.ByExpression.Width() == 0 ||
                group.GroupExpression.Width() == 0)
            {
                return false;
            }

            var lastToken = group.ByExpression.GetLastToken(includeSkipped: true);

            if (lastToken == token)
            {
                return true;
            }

            return false;
        }
    }
}
