using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Windows.Controls;

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
                        //typeTreeViewItem.Name = t.Name;
                        typeTreeViewItem.Header = t.Name;
                        namespaceTreeViewItem.Items.Add(typeTreeViewItem);
                        

                        var allFields = t.GetFields(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Static);
                        foreach(var field in allFields)
                        {
                            typeTreeViewItem.Items.Add(field);
                        }
                        var allProps = t.GetProperties(
                            BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Static);

                        foreach (var prop in allProps)
                        {
                            typeTreeViewItem.Items.Add(prop);
                        }
                        var allMethods = t.GetMethods(
                             BindingFlags.Instance |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Static);
                        foreach (var method in allMethods)
                        {
                            typeTreeViewItem.Items.Add(method);
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
