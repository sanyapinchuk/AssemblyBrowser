using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace LibraryMetadata
{
    public class MetadataInformer : IMetadataInformer
    {
        public List<TreeViewItem> GetTreeMetaData(string pathLib)
        {
            var result = new List<TreeViewItem>();
            try
            {
                var assembly = Assembly.LoadFrom(pathLib);
                var namespaces = assembly.GetTypes()
                    .Select(t => t.GetTopLevelNamespace())
                    .Distinct();
                foreach (var ns in namespaces)
                {
                    var namespaceTreeViewItem = new TreeViewItem();
                    namespaceTreeViewItem.Header = ns;
                    result.Add(namespaceTreeViewItem);                    

                    var allTypes = assembly.GetTypes()
                        .Where(t=>t.Namespace == ns)
                        .Distinct();
                    foreach (var t in allTypes)
                    {
                        var typeTreeViewItem = new TreeViewItem();
                        typeTreeViewItem.Header = t.Name;
                        namespaceTreeViewItem.Items.Add(typeTreeViewItem);
                        

                        var allFields = t.GetFields(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Static);
                        if(allFields != null && allFields.Length > 0)
                        {
                            var fieldsTreeView = new TreeViewItem();
                            fieldsTreeView.Header = "FIELDS";
                            typeTreeViewItem.Items.Add(fieldsTreeView);
                            foreach (var field in allFields)
                            {
                                fieldsTreeView.Items.Add(field);
                            }
                        }


                        var allProps = t.GetProperties(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Static);
                        if (allProps != null && allProps.Length > 0)
                        {
                            var propsTreeView = new TreeViewItem();
                            propsTreeView.Header = "PROPERTIES";
                            typeTreeViewItem.Items.Add(propsTreeView);
                            foreach (var prop in allProps)
                            {
                                propsTreeView.Items.Add(prop);
                            }
                        }


                        var allMethods = t.GetMethods(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Static);
                        if (allMethods != null && allMethods.Length > 0)
                        {
                            var methodsTreeView = new TreeViewItem();
                            methodsTreeView.Header = "METHODS";
                            typeTreeViewItem.Items.Add(methodsTreeView);
                            foreach (var method in allMethods)
                            {
                                methodsTreeView.Items.Add(method);
                            }
                        }


                        var extansionMethodsForCurrType = GetExtensionMethods(assembly, t);
                        if (extansionMethodsForCurrType.Count() > 0)
                        {
                            var ExtMethodsTreeView = new TreeViewItem();
                            ExtMethodsTreeView.Header = "EXTANSION METHODS";
                            typeTreeViewItem.Items.Add(ExtMethodsTreeView);
                            foreach (var ExtMethod in extansionMethodsForCurrType)
                            {
                                ExtMethodsTreeView.Items.Add(ExtMethod);
                            }
                        }
                        
                    }
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            


            return result;
        }

        private IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly,
        Type extendedType)
        {
            var query = from type in assembly.GetTypes()
                        where type.IsSealed && !type.IsGenericType && !type.IsNested
                        from method in type.GetMethods(BindingFlags.Static
                            | BindingFlags.Public | BindingFlags.NonPublic)
                        where method.IsDefined(typeof(ExtensionAttribute), false)
                        where method.GetParameters()[0].ParameterType == extendedType
                        select method;
            return query;
        }
    }
    
    public static class MetadataInformerExtensions
    {
        public static string GetTopLevelNamespace(this Type t)
        {
            string ns = t.Namespace ?? "";
            int firstDot = ns.IndexOf('.');
            return firstDot == -1 ? ns : ns.Substring(0, firstDot);
        }
    }
}
