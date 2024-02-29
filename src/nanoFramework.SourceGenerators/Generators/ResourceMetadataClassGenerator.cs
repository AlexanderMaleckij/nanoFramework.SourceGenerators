using System.Collections.Generic;
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
    internal sealed class ResourceMetadataClassGenerator : IResourceMetadataClassGenerator
    {
        public string GenerateSource(ResourceMetadataGenerationOptions options)
        {
            Guard.ThrowIfNull(options, nameof(options));

            var ctorParameters = new List<ParameterSyntax>
            {
                CreateParameter("id", Constants.ResourceId.EnumIdentifier)
            };

            var ctorStatements = new List<StatementSyntax>
            {
                CreateAssignmentExpression(Constants.ResourceMetadata.PropertyNames.Id, "id")
            };

            var classMembers = new List<MemberDeclarationSyntax>
            {
                CreatePublicGetOnlyProperty(Constants.ResourceMetadata.PropertyNames.Id, Constants.ResourceId.EnumIdentifier)
            };

            if (options.ShouldGenerateName)
            {
                ctorParameters.Add(CreateParameter("name", SyntaxKind.StringKeyword));
                ctorStatements.Add(CreateAssignmentExpression(Constants.ResourceMetadata.PropertyNames.Name, "name"));
                classMembers.Add(CreatePublicGetOnlyProperty(Constants.ResourceMetadata.PropertyNames.Name, SyntaxKind.StringKeyword));
            }

            if (options.ShouldGenerateUriPath)
            {
                ctorParameters.Add(CreateParameter("uriPath", SyntaxKind.StringKeyword));
                ctorStatements.Add(CreateAssignmentExpression(Constants.ResourceMetadata.PropertyNames.UriPath, "uriPath"));
                classMembers.Add(CreatePublicGetOnlyProperty(Constants.ResourceMetadata.PropertyNames.UriPath, SyntaxKind.StringKeyword));
            }

            if (options.ShouldGenerateSize)
            {
                ctorParameters.Add(CreateParameter("size", SyntaxKind.LongKeyword));
                ctorStatements.Add(CreateAssignmentExpression(Constants.ResourceMetadata.PropertyNames.Size, "size"));
                classMembers.Add(CreatePublicGetOnlyProperty(Constants.ResourceMetadata.PropertyNames.Size, SyntaxKind.LongKeyword));
            }

            if (options.ShouldGenerateContentType)
            {
                ctorParameters.Add(CreateParameter("contentType", SyntaxKind.StringKeyword));
                ctorStatements.Add(CreateAssignmentExpression(Constants.ResourceMetadata.PropertyNames.ContentType, "contentType"));
                classMembers.Add(CreatePublicGetOnlyProperty(Constants.ResourceMetadata.PropertyNames.ContentType, SyntaxKind.StringKeyword));
            }

            if (options.ShouldGenerateContentEncoding)
            {
                ctorParameters.Add(CreateParameter("contentEncoding", SyntaxKind.StringKeyword));
                ctorStatements.Add(CreateAssignmentExpression(Constants.ResourceMetadata.PropertyNames.ContentEncoding, "contentEncoding"));
                classMembers.Add(CreatePublicGetOnlyProperty(Constants.ResourceMetadata.PropertyNames.ContentEncoding, SyntaxKind.StringKeyword));
            }

            classMembers.Add(
                ConstructorDeclaration(Constants.ResourceMetadata.ClassIdentifier)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword))
                    .AddParameterListParameters(ctorParameters.ToArray())
                    .WithBody(Block(ctorStatements.ToArray()))
            );

            var classDeclaration = ClassDeclaration(
                attributeLists: default,
                modifiers: TokenList(
                    Token(SyntaxKind.InternalKeyword),
                    Token(SyntaxKind.SealedKeyword)
                ),
                identifier: Identifier(Constants.ResourceMetadata.ClassIdentifier),
                typeParameterList: default,
                parameterList: default,
                baseList: default,
                constraintClauses: default,
                members: List(classMembers)
            );

            var namespaceDeclaration = NamespaceDeclaration(ParseName(Constants.ResourceMetadataNamespace))
                .AddMembers(classDeclaration);

            var compilationUnit = CompilationUnit()
                .AddMembers(namespaceDeclaration);

            return SyntaxTree(compilationUnit, encoding: Encoding.UTF8)
                .GetRoot().NormalizeWhitespace().ToString();
        }
    }
}
