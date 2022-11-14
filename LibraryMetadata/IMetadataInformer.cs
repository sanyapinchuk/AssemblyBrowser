using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryMetadata
{
    public interface IMetadataInformer
    {
        List<TreeViewItem> GetTreeMetaData(string pathLib);
    }
}
