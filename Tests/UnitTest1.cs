using Xunit;
using LibraryMetadata;
using System.Windows.Controls;
using System;
using System.Threading;

namespace Tests
{
    
    public class UnitTest1
    {
        [Fact]
        public void CollectionNotNull()
        {
            //Arrange
            IMetadataInformer metadataInformer = new MetadataInformer();
            string path = "C:/Users/Asus/Desktop/учеба/спп/3/AssemblyBrowser/TestLibrary/bin/Debug/net6.0/TestLibrary.dll";
            //Act
            var treeItem = metadataInformer.GetTreeMetaData(path);
            //Assert
            Assert.True(treeItem != null);
            // Assert.True(result.Count == 1 && result[0].ThreadMethods.Count == 1);
        }
        [Fact]
        [STAThread]
        public void CollectionNotEmpty()
        {
            Thread thread = new Thread(() =>
            {
                //Arrange
                IMetadataInformer metadataInformer = new MetadataInformer();
                string path = "C:/Users/Asus/Desktop/учеба/спп/3/AssemblyBrowser/TestLibrary/bin/Debug/net6.0/TestLibrary.dll";
                //Act
                var treeItem = metadataInformer.GetTreeMetaData(path);
                //Assert
                Assert.True(treeItem.Count > 0);
                
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            
        }
        [Fact]
        public void CollectionEmptyWithEmptyPath()
        {
            //Arrange
            IMetadataInformer metadataInformer = new MetadataInformer();
            string path = "";
            //Act
            var treeItem = metadataInformer.GetTreeMetaData(path);
            //Assert
            Assert.True(treeItem.Count==0);
            // Assert.True(result.Count == 1 && result[0].ThreadMethods.Count == 1);
        }
        [Fact]
        [STAThread]
        public void CollectionMethodsContainsInheritedMethods()
        {
            Thread thread = new Thread(() =>
            {
                //Arrange
                IMetadataInformer metadataInformer = new MetadataInformer();
                string path = "C:/Users/Asus/Desktop/учеба/спп/3/AssemblyBrowser/TestLibrary/bin/Debug/net6.0/TestLibrary.dll";
                //Act
                var treeItem = metadataInformer.GetTreeMetaData(path);
                //Assert
                Assert.True(((TreeViewItem)((TreeViewItem)treeItem[2].Items[0]).Items[2]).Items.Count >= 6);
                // Assert.True(result.Count == 1 && result[0].ThreadMethods.Count == 1);


            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
                    }
    }
}