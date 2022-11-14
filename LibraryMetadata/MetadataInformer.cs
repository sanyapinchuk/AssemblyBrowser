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
                        bool isOnlyExtasionMethods = true;
                       

                        var typeTreeViewItem = new TreeViewItem();
                        typeTreeViewItem.Header = t.Name;
                        
                        

                        var allFields = t.GetFields(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Static);
                        if(allFields != null && allFields.Length > 0)
                        {
                            isOnlyExtasionMethods = false;
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
                            isOnlyExtasionMethods = false;
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
                            BindingFlags.Static)
                            .Where(m => !m.IsDefined(typeof(ExtensionAttribute), false));
                        if (allMethods != null && allMethods.Count() > 0)
                        {
                            isOnlyExtasionMethods = false;
                            var methodsTreeView = new TreeViewItem();
                            methodsTreeView.Header = "METHODS";
                            typeTreeViewItem.Items.Add(methodsTreeView);
                            foreach (var method in allMethods)
                            {
                                methodsTreeView.Items.Add(method);
                            }
                        }


                        var extensionMethodsForCurrType = GetExtensionMethods(assembly, t);
                        if (extensionMethodsForCurrType.Count() > 0)
                        {
                            if (isOnlyExtasionMethods) // if all static class contains only
                            {                          // extMethods for types from this assembly
                                continue;
                            }
                            var ExtMethodsTreeView = new TreeViewItem();
                            ExtMethodsTreeView.Header = "EXTENSION METHODS";
                            typeTreeViewItem.Items.Add(ExtMethodsTreeView);
                            foreach (var ExtMethod in extensionMethodsForCurrType)
                            {
                                ExtMethodsTreeView.Items.Add(ExtMethod);
                            }
                        }
                        namespaceTreeViewItem.Items.Add(typeTreeViewItem);

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
