using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Model2.Assemblies.Interfaces;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Css.Tree;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Resx.ResourceDefaultLanguage;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace SampleReSharperPlugin
{
    public static class PsiExtensionMethods
    {        
        [CanBeNull]
        private static IEnumerable<IPsiSourceFile> GetPsiSourceFiles([NotNull] this ISolution solution)
        {            
            var psiServices = solution.GetPsiServices();                                   

            var modules = psiServices.Modules.GetModules();                                                

            var psiSourceFiles = new List<IPsiSourceFile>();

            foreach (var psiModule in modules)
                psiSourceFiles.AddRange(psiModule.SourceFiles);

            return psiSourceFiles;
        }


        [CanBeNull]
        private static IEnumerable<ICSharpFile> GetFilesForOpenedProject([NotNull] this ISolution solution)
        {
            var psiSourceFiles = solution.GetPsiSourceFiles();            

            if (psiSourceFiles == null) return null;

            var files = new List<ICSharpFile>();

            foreach (var psiSourceFile in psiSourceFiles)
                files.AddRange(psiSourceFile.GetPsiFiles<CSharpLanguage>().SafeOfType<ICSharpFile>());

            return files;
        }


        [CanBeNull]
        public static IEnumerable<ICSharpTypeDeclaration> GetTypeDeclarationsForOpenedProject(
            [NotNull] this ISolution solution)
        {
            var files = solution.GetFilesForOpenedProject();
            if (files == null) return null;

            var types = new List<ICSharpTypeDeclaration>();

            foreach (var file in files)
                types.AddRange(file.TypeDeclarationsEnumerable);

            return types;
        }


        [CanBeNull]
        public static IEnumerable<T> GetMemberDeclarations<T>(
            [NotNull] this ICSharpTypeDeclaration typeDeclaration) where T : class, IClassMemberDeclaration
        {
            //   var genType = typeof(T);

            //var classDeclaration = (IClassDeclaration) typeDeclaration;

            //if (genType == typeof(IMethodDeclaration))
            //{
            //    var result = classDeclaration.MethodDeclarationsEnumerable;
            //    return (IEnumerable<T>) (IEnumerable<IMethodDeclaration>)result;
            //}
            //else
            //{
                //var result = from declaration in typeDeclaration.MemberDeclarations
                //    where declaration as T == true
                //    select declaration;                
            //}

            return Enumerable.OfType<T>(typeDeclaration.MemberDeclarations);
        }


        [CanBeNull]
        public static IEnumerable<IDeclaredElement> GetReferencedElements(this ITreeNode node)
        {
            var result = new List<IDeclaredElement>();
            var parentExpression = node.GetParentOfType<IExpression>();
            if (parentExpression == null) return null;            
                                  
            var references = parentExpression.GetReferences();            

            foreach (var reference in references)
            {
                var declaredElement = reference.Resolve().DeclaredElement;
                if (declaredElement != null)
                    result.Add(declaredElement);
            }

            return result.Count == 0 ? null : result;
        }


        [CanBeNull]
        public static T GetParentOfType<T>(this ITreeNode node) where T : class, ITreeNode
        {
            while (node != null)
            {
                var typedNode = node as T;
                if (typedNode != null)
                    return typedNode;

                node = node.Parent;
            }
            return null;
        }
    }
}