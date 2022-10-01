using MovePhotos.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovePhotos.Tests {
    [TestFixture]
    public class PhotoWorkerTests {
        [Test]
        public void Test() {
            //arrange
            var wrk = new PhotoWorker();
            var photoDate = "2022-08-17";
            //act
            var res = wrk.GetFolderName(photoDate);
            //assert
            Assert.AreEqual("2022-08-01 Common", res);
        }
    }
}
