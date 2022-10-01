using NUnit.Framework;
using SuspiciousFolders.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousFolders.Tests {
    [TestFixture]
    public class FolderWorkerTests {
        [Test]
        public void GetPhotoModifiedTimeTest() {
            //arrange
            var wrk = new FolderWorker();
            var path = @"c:\tempPhototest33\testPhoto\IMG_0007.JPG";
            //act
            var res = wrk.GetPhotoModifiedTime(path);
            //assert
            Assert.AreEqual("2022-09-29", res);

        }

        [Test]
        public void GetFolderDateTests() {
            //arrange
            var wrk = new FolderWorker();

            var path = @"c:\tempPhoto\2012-06-01 Common";
            //act
            var res = wrk.GetFolderDate(path);
            //assert
            Assert.AreEqual("2012-06-01", res);
        }

        [Test]
        public void GetFolderDateTests2() {
            //arrange
            var wrk = new FolderWorker();
            var path = @"c:\tempPhoto\2012st";
            //act
            var res = wrk.GetFolderDate(path);
            //assert
            Assert.AreEqual(null, res);
        }

        [Test]
        public void IsPhotoFitFolderTest0() {
            //arrange
            var wrk = new FolderWorker();
            var folder = "2022-09-29";
            var photo = "2022-09-29";
            //act
            var res = wrk.IsPhotoFitFolder(folder, photo);
            //assert
            Assert.AreEqual(true, res);
        }
        [Test]
        public void IsPhotoFitFolderTest1() {
            //arrange
            var wrk = new FolderWorker();
            var folder = "2022-09-29";
            var photo = "2022-09-15";
            //act
            var res = wrk.IsPhotoFitFolder(folder, photo);
            //assert
            Assert.AreEqual(false, res);
        }
      
        [Test]
        public void IsPhotoFitFolderTest2() {
            //arrange
            var wrk = new FolderWorker();
            var folder = "2022-09-01";
            var photo = "2022-09-15";
            //act
            var res = wrk.IsPhotoFitFolder(folder, photo);
            //assert
            Assert.AreEqual(true, res);
        }
        [Test]
        public void IsPhotoFitFolderTest3() {
            //arrange
            var wrk = new FolderWorker();
            var folder = "2022-09-01";
            var photo = "2022-09-29";
            //act
            var res = wrk.IsPhotoFitFolder(folder, photo);
            //assert
            Assert.AreEqual(true, res);
        }
        [Test]
        public void IsPhotoFitFolderTest4() {
            //arrange
            var wrk = new FolderWorker();
            var folder = "2022-09-29";
            var photo = "2022-09-01";
            //act
            var res = wrk.IsPhotoFitFolder(folder, photo);
            //assert
            Assert.AreEqual(false, res);
        }

    }
}
