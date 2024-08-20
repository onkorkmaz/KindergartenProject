using Business;
using Common;
using Entity;
using KindergartenProject.userControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace KindergartenProject
{
    public class CommonUIFunction : System.Web.UI.MasterPage
    {
        public static List<SeasonEntity> GetSeasonList(int year)
        {
            int id = 0;
            List<SeasonEntity> monthList = new List<SeasonEntity>();
            monthList.Add(new SeasonEntity(id++, year, 7, "Temmuz"));
            monthList.Add(new SeasonEntity(id++,year, 8, "Ağustos"));
            monthList.Add(new SeasonEntity(id++,year, 9, "Eylül"));
            monthList.Add(new SeasonEntity(id++,year, 10, "Ekim"));
            monthList.Add(new SeasonEntity(id++,year, 11, "Kasım"));
            monthList.Add(new SeasonEntity(id++,year, 12, "Aralık"));
            monthList.Add(new SeasonEntity(id++,year + 1, 1, "Ocak"));
            monthList.Add(new SeasonEntity(id++,year + 1, 2, "Şubat"));
            monthList.Add(new SeasonEntity(id++,year + 1, 3, "Mart"));
            monthList.Add(new SeasonEntity(id++,year + 1, 4, "Nisan"));
            monthList.Add(new SeasonEntity(id++,year + 1, 5, "Mayıs"));
            monthList.Add(new SeasonEntity(id++,year + 1, 6, "Haziran"));
            monthList.Add(new SeasonEntity(id++, year + 1, 7, "Temmuz"));
            monthList.Add(new SeasonEntity(id++, year + 1, 8, "Ağustus"));

            return monthList;
        }

        private void controlMenuVisibleForAuthority(HtmlGenericControl genericControl, List<AuthorityScreenEnum> authorityList)
        {
            bool authorityExists = false;

            foreach (AuthorityScreenEnum enm in authorityList)
            {
                AuthorityEntity entity = new AuthorityBusiness(new BasePage()._ProjectType, new BasePage()._AdminEntity.Id).GetAuthorityWithScreenAndTypeId(enm, new BasePage()._AdminEntity.AuthorityTypeId);
                if (entity != null)
                {
                    authorityExists = entity.HasAuthority;
                    break;
                }
            }
            genericControl.Visible = authorityExists;
        }

        internal void SetVisibility(AuthorityScreenEnum authorityScreen, HtmlGenericControl genericControl)
        {
            if (new BasePage()._AdminEntity.IsDeveleporOrSuperAdmin)
            {
                genericControl.Visible = true;
                return;
            }

            List<AuthorityScreenEnum> authortityList = new List<AuthorityScreenEnum>();
            authortityList = new List<AuthorityScreenEnum>();
            authortityList.Add(authorityScreen);
            controlMenuVisibleForAuthority(genericControl, authortityList);
        }

        public static void loadClass(ProjectType projectType, ref System.Web.UI.WebControls.DropDownList drpClassList, ref divInformation _divInformation, bool isAddUnSignedItem)
        {
            DataResultArgs<List<ClassEntity>> resultSet = new ClassBusiness(projectType).Get_ClassForStudent();

            if (resultSet.HasError)
            {
                _divInformation.ErrorText = resultSet.ErrorDescription;
                return;
            }
            else
            {
                List<ClassEntity> classList = resultSet.Result;
                List<ClassEntity> list = new List<ClassEntity>();

                list.Add(new ClassEntity() { Id = -1, Name = "-" });
                if (resultSet.Result != null)
                {

                    foreach (ClassEntity entity in classList)
                    {
                        list.Add(entity);
                    }
                }

                if (isAddUnSignedItem)
                {
                    list.Add(new ClassEntity() { Id = -2, Name = "Atanmayanlar" });
                }

                drpClassList.DataSource = list;
                drpClassList.DataValueField = "Id";
                drpClassList.DataTextField = "ClassAndMainTeacherName";
                drpClassList.DataBind();
            }
        }


        public void GenerateWordDocumentForStudentList(List<StudentEntity> studentList, bool isShowPrice, ProjectType projectType)
        {
            string desktopClassListPath = prepareFolder();

            string prefix = DateTime.Now.ToString("yyyyMMddhhmm");
            List<string> classNameList = studentList.GroupBy(o => o.ClassName).Select(o => o.Key).ToList();

            foreach (string className in classNameList)
            {
                string classNameDisplay = className;
                if (string.IsNullOrEmpty(className))
                {
                    classNameDisplay = "Atanamayan Kayıtlar";
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document, true))
                    {
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        Paragraph headingParagraph = body.AppendChild(new Paragraph());

                        // Set paragraph alignment to center
                        ParagraphProperties paraProps = new ParagraphProperties(
                            new Justification { Val = JustificationValues.Center }
                        );
                        headingParagraph.Append(paraProps);

                        // Add run with heading text
                        Run headingRun = headingParagraph.AppendChild(new Run());

                        // Set run properties (e.g., bold for heading)
                        RunProperties runProperties = new RunProperties(
                            new Bold(),
                            new Color() { Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF" },
                            new FontSize { Val = "32" } // Font size for heading (e.g., 16 pt)
                        );
                        headingRun.PrependChild(runProperties);

                        // Add text to the run
                        headingRun.AppendChild(new Text(classNameDisplay));
                        headingRun.AppendChild(new Break());


                        Paragraph para = body.AppendChild(new Paragraph());
                        Run run = para.AppendChild(new Run());

                        // Add a table
                        Table table = new Table();

                        // Define table properties
                        TableProperties tblProps = new TableProperties(
                            new TableStyle { Val = "TableGrid" },
                           new TableWidth { Type = TableWidthUnitValues.Pct, Width = "5000" }, // Set table width to 100% of the page
                            new TableBorders(
                                new TopBorder { Val = BorderValues.Single, Size = 4 },
                                new BottomBorder { Val = BorderValues.Single, Size = 4 },
                                new LeftBorder { Val = BorderValues.Single, Size = 4 },
                                new RightBorder { Val = BorderValues.Single, Size = 4 },
                                new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                                new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                            ));

                        table.Append(tblProps);
                        Paragraph paraCenter = getCenterBold("Ad Soyad");

                        TableRow tr = new TableRow();
                        TableCell tc1 = new TableCell(getCellProperties(),paraCenter);

                        Paragraph paraCenter2 = getCenterBold("Veli Adı");

                        TableCell tc2 = new TableCell(getCellProperties(),paraCenter2);

                        Paragraph paraCenter3 = getCenterBold("Doğum Tarihi");
                        TableCell tc3 = new TableCell(getCellProperties(),paraCenter3);

                        tr.Append(tc1);
                        tr.Append(tc2);
                        tr.Append(tc3);

                        if (isShowPrice)
                        {
                            Paragraph paraCenter4 = getCenterBold("Ücret");
                            TableCell tc4 = new TableCell(getCellProperties(),paraCenter4);
                            tr.Append(tc4);
                        }

                        table.Append(tr);


                        List<StudentEntity> lst = studentList.Where(o => o.ClassName == className).ToList();

                        foreach (StudentEntity entity in lst)
                        {
                            tr = new TableRow();
                            tc1 = new TableCell(getCellProperties(),new Paragraph(new Run(new Text(entity.FullName))));
                            tc2 = new TableCell(getCellProperties(), new Paragraph(new Run(new Text(entity.ParentName))));
                            tc3 = new TableCell(getCellProperties(), new Paragraph(new Run(new Text(entity.BirthdayWithFormatddMMyyyy))));

                            tr.Append(tc1);
                            tr.Append(tc2);
                            tr.Append(tc3);

                            if (isShowPrice)
                            {
                                List<PaymentEntity> listPayment = new PaymentBusiness(projectType).Get_Payment(entity.Id).Result;
                                TableCell tc4 = new TableCell(getCellProperties(), new Paragraph(new Run(new Text(listPayment.FirstOrDefault()?.AmountDesc))));
                                tr.Append(tc4);
                            }

                            table.Append(tr);

                        }

                        run.AppendChild(table);

                        wordDocument.Close();
                    }

                    byte[] wordBytes = memoryStream.ToArray();

                    string tempPath = Path.Combine(desktopClassListPath + "\\", prefix + "_" + classNameDisplay + ".docx");
                    File.WriteAllBytes(tempPath, wordBytes);
                }
            }
        }

        private TableCellProperties getCellProperties()
        {
            TableCellMargin cellMargin = new TableCellMargin()
            {
                TopMargin = new  TopMargin() { Width = "10", Type = TableWidthUnitValues.Dxa },   // 100 dxa üst boşluk  
                BottomMargin = new  BottomMargin() { Width = "10", Type = TableWidthUnitValues.Dxa }, // 100 dxa alt boşluk  
                LeftMargin = new  LeftMargin() { Width = "100", Type = TableWidthUnitValues.Dxa },   // 100 dxa sol boşluk  
                RightMargin = new  RightMargin() { Width = "100", Type = TableWidthUnitValues.Dxa }    // 100 dxa sağ boşluk  
            };

            TableCellProperties cellProperties = new TableCellProperties(cellMargin);
            return cellProperties;
        }

        private static Paragraph getCenterBold(String text)
        {
            Paragraph paraCenter = new Paragraph();
            ParagraphProperties paraPropsCenter = new ParagraphProperties(
                new Justification { Val = JustificationValues.Center }, new Bold()
            );
            paraCenter.Append(paraPropsCenter);

            RunProperties runPropertiesCenter = new RunProperties(new Bold());

            Run runCenter = new Run(runPropertiesCenter);
            runCenter.AppendChild(new Text(text));

            paraCenter.Append(runCenter);
            return paraCenter;
        }

        private static string prepareFolder()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string desktopClassListPath = desktopPath + "//Sınıf_Listesi";

            if (!Directory.Exists(desktopClassListPath))
            {
                Directory.CreateDirectory(desktopClassListPath);
            }

            string[] files = Directory.GetFiles(desktopClassListPath);

            // Delete each file
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                }
            }

            return desktopClassListPath;
        }
    }
}