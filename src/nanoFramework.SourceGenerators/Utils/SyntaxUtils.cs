using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace nanoFramework.SourceGenerators.Utils
{
    internal static class SyntaxUtils
    {
        public static ArgumentSyntax CreateArgument(string value)
        {
            return Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value)));
        }

        public static ArgumentSyntax CreateArgument(long value)
        {
            return Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value)));
        }

        public static ExpressionStatementSyntax CreateAssignmentExpression(string left, string right)
        {
            return ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(left),
                    IdentifierName(right)
                )
            );
        }

        public static ParameterSyntax CreateParameter(string name, SyntaxKind type)
        {
            return Parameter(Identifier(name)).WithType(PredefinedType(Token(type)));
        }

        public static ParameterSyntax CreateParameter(string name, string type)
        {
            return Parameter(Identifier(name)).WithType(ParseTypeName(type));
        }

        public static PropertyDeclarationSyntax CreatePublicGetOnlyProperty(string name, SyntaxKind type)
        {
            return PropertyDeclaration(PredefinedType(Token(type)), name)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithAccessorList(
                    AccessorList(
                        SingletonList(
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                        )
                    )
                );
        }

        public static PropertyDeclarationSyntax CreatePublicGetOnlyProperty(string name, string type)
        {
            return PropertyDeclaration(ParseTypeName(type), name)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithAccessorList(AccessorList(
                    SingletonList(
                        AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                    )
                ));
        }
    }
}
