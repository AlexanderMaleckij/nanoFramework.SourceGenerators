using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using nanoFramework.SourceGenerators.Models;
using nanoFramework.SourceGenerators.Utils;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static nanoFramework.SourceGenerators.Utils.SyntaxUtils;

namespace nanoFramework.SourceGenerators.Generators
{
    internal sealed class ResourceMetadataProviderClassGenerator : IResourceMetadataProviderClassGenerator
    {
        public string GenerateSource(
            IEnumerable<ResourceMetadata> values,
            ResourceMetadataGenerationOptions metadataClassOptions,
            ResourceMetadataProviderGenerationOptions providerGenerationOptions)
        {
            Guard.ThrowIfNull(values, nameof(values));
            Guard.ThrowIfNull(metadataClassOptions, nameof(metadataClassOptions));
            Guard.ThrowIfNull(providerGenerationOptions, nameof(providerGenerationOptions));

            var providerClassMembers = List<MemberDeclarationSyntax>();

            if (providerGenerationOptions.ShouldGenerateFindByName)
            {
                providerClassMembers = providerClassMembers.Add(CreateFindByNameMethod(metadataClassOptions, values));
            }

            if (providerGenerationOptions.ShouldGenerateFindByUriPath)
            {
                providerClassMembers = providerClassMembers.Add(CreateFindByUriPathMethod(metadataClassOptions, values));
            }

            var classDeclaration = ClassDeclaration(
                attributeLists: default,
                modifiers: TokenList(
                    Token(SyntaxKind.InternalKeyword),
                    Token(SyntaxKind.StaticKeyword)
                ),
                identifier: Identifier(Constants.ResourcesMetadataProvider.ClassIdentifier),
                typeParameterList: default,
                parameterList: default,
                baseList: default,
                constraintClauses: default,
                members: providerClassMembers
            );

            var namespaceDeclaration = NamespaceDeclaration(ParseName(Constants.ResourceMetadataNamespace))
                .AddMembers(classDeclaration);

            var compilationUnit = CompilationUnit()
                .AddMembers(namespaceDeclaration);

            return SyntaxTree(compilationUnit, encoding: Encoding.UTF8)
                .GetRoot().NormalizeWhitespace().ToString();
        }

        private static MethodDeclarationSyntax CreateFindByNameMethod(
            ResourceMetadataGenerationOptions metadataClassOptions,
            IEnumerable<ResourceMetadata> metadataValues)
        {
            var methodParameterName = "name";

            return MethodDeclaration(
                attributeLists: default,
                modifiers: TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)),
                returnType: ParseTypeName(Constants.ResourceMetadata.ClassIdentifier),
                explicitInterfaceSpecifier: default,
                identifier: Identifier(Constants.ResourcesMetadataProvider.MethodNames.FindByName),
                typeParameterList: default,
                parameterList: ParameterList().AddParameters(CreateParameter(methodParameterName, SyntaxKind.StringKeyword)),
                constraintClauses: default,
                body: default,
                expressionBody: ArrowExpressionClause(
                    CreateResourceMetadataSwitchExpressionBy(
                        methodParameterName,
                        metadataClassOptions,
                        metadataValues.Where(x => x.Name != null),
                        value => ConstantPattern(
                            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value.Name))
                        )
                    )
                ),
                semicolonToken: Token(SyntaxKind.SemicolonToken)
            );
        }

        private static MethodDeclarationSyntax CreateFindByUriPathMethod(
            ResourceMetadataGenerationOptions metadataClassOptions,
            IEnumerable<ResourceMetadata> metadataValues)
        {
            var methodParameterName = "uriPath";

            return MethodDeclaration(
                attributeLists: default,
                modifiers: TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)),
                returnType: ParseTypeName(Constants.ResourceMetadata.ClassIdentifier),
                explicitInterfaceSpecifier: default,
                identifier: Identifier(Constants.ResourcesMetadataProvider.MethodNames.FindByUriPath),
                typeParameterList: default,
                parameterList: ParameterList().AddParameters(CreateParameter(methodParameterName, SyntaxKind.StringKeyword)),
                constraintClauses: default,
                body: default,
                expressionBody: ArrowExpressionClause(
                    CreateResourceMetadataSwitchExpressionBy(
                        methodParameterName,
                        metadataClassOptions,
                        metadataValues.Where(x => x.UriPath != null),
                        value => ConstantPattern(
                            LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value.UriPath))
                        )
                    )
                ),
                semicolonToken: Token(SyntaxKind.SemicolonToken)
            );
        }

        private static SwitchExpressionSyntax CreateResourceMetadataSwitchExpressionBy(
            string parameterName,
            ResourceMetadataGenerationOptions metadataClassOptions,
            IEnumerable<ResourceMetadata> metadataValues,
            Func<ResourceMetadata, PatternSyntax> pattern)
        {
            return SwitchExpression(
                IdentifierName(parameterName),
                SeparatedList<SwitchExpressionArmSyntax>()
                    // Adding arms that return new class instance.
                    .AddRange(
                        metadataValues.Select(value =>
                            SwitchExpressionArm(
                                pattern(value),
                                CreateResourceMetadataInstance(value, metadataClassOptions)
                            )
                        )
                    )
                    // Adding a discard arm: _ => null
                    .Add(
                        SwitchExpressionArm(
                            DiscardPattern(),
                            LiteralExpression(SyntaxKind.NullLiteralExpression)
                        )
                    )
            );
        }

        private static ObjectCreationExpressionSyntax CreateResourceMetadataInstance(
            ResourceMetadata value,
            ResourceMetadataGenerationOptions metadataClassOptions)
        {
            var nullArgument = Argument(LiteralExpression(SyntaxKind.NullLiteralExpression));

            ExpressionSyntax idExpression = LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(value.Id));

            if (value.Id < 0)
            {
                idExpression = ParenthesizedExpression(idExpression);
            }

            var argumentList = SeparatedList<ArgumentSyntax>()
                .Add(Argument(
                    CastExpression(
                        ParseTypeName(Constants.ResourceId.EnumIdentifier),
                        idExpression
                    )
                ));

            if (metadataClassOptions.ShouldGenerateName)
            {
                argumentList = value.Name is null
                    ? argumentList.Add(nullArgument)
                    : argumentList.Add(CreateArgument(value.Name));
            }

            if (metadataClassOptions.ShouldGenerateUriPath)
            {
                argumentList = value.UriPath is null
                    ? argumentList.Add(nullArgument)
                    : argumentList.Add(CreateArgument(value.UriPath));
            }

            if (metadataClassOptions.ShouldGenerateSize)
            {
                argumentList = argumentList.Add(CreateArgument(value.Size));
            }

            if (metadataClassOptions.ShouldGenerateContentType)
            {
                argumentList = value.ContentType is null
                    ? argumentList.Add(nullArgument)
                    : argumentList.Add(CreateArgument(value.ContentType));
            }

            if (metadataClassOptions.ShouldGenerateContentEncoding)
            {
                argumentList = value.ContentEncoding is null
                    ? argumentList.Add(nullArgument)
                    : argumentList.Add(CreateArgument(value.ContentEncoding));
            }

            return ObjectCreationExpression(
                ParseTypeName(Constants.ResourceMetadata.ClassIdentifier),
                ArgumentList(argumentList),
                default
            );
        }
    }
}
