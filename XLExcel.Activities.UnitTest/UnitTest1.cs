using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UiPath.XLExcel.Activities;
using UiPath.XLExcel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XLExcel.Activities.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestClass()]
        public class UtilsTests
        {
            public TestContext TestContext { get; set; }

            [ClassInitialize]
            public static void ClassInitialize(TestContext context)
            {

            }


            //[TestMethod()]
            [TestMethod]
            [DeploymentItem("SAXReaderTestData.xml")]
            [DeploymentItem("FilesUsedForTesting", "FilesUsedForTesting")]
            [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                       "|DataDirectory|\\SAXReaderTestData.xml", "Row",
                        DataAccessMethod.Sequential)]
            public void ReadSAXRangeTest()
            {

                //get input for the unit test
                string SheetName = (string)TestContext.DataRow["SheetName"];
                string FilePath = (string)TestContext.DataRow["FilePath"];
                string Range = (string)TestContext.DataRow["Range"];
                string OutputVerificationFilePath = (string)TestContext.DataRow["OutputVerification"];
                bool expectExceptions = Convert.ToBoolean(TestContext.DataRow["ExpectExceptions"].ToString());

                bool AddHeaders = Convert.ToBoolean((string)TestContext.DataRow["AddHeaders"]);

                string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string fullFilePath = string.Format("{0}\\{1}\\{2}", directory, "FilesUsedForTesting", FilePath);

                try
                {
                    //run the Read Range
                    ExcelRange excelRange = new ExcelRange(Range);
                    DataTable dt = Utils.ReadSAXRange(excelRange, fullFilePath, SheetName, AddHeaders);
                    if (expectExceptions) Assert.Fail(); // raises AssertionException

                    string dtReceived = dt.Dump();

                    //get validation text
                    string validationFilePath = string.Format("{0}\\{1}\\{2}", directory, "FilesUsedForTesting\\ExpectedResultsFile", OutputVerificationFilePath);
                    string validationValue = File.ReadAllText(validationFilePath);

                    Assert.AreEqual(validationValue, dtReceived);
                }
                catch (Exception ex)
                {
                    // Catches the assertion exception, and the test passes
                    Console.WriteLine("Found exception during unit test: " + ex.ToString() + "ExpectExceptions: " + expectExceptions.ToString());
                    if (!expectExceptions) Assert.Fail(); // raises AssertionException
                }



            }


        }
    }
}
